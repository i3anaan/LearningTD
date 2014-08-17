using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicCreep : MonoBehaviour
{

	public int health;
	public int incomingDamage;
	public float speed;
	public int towerDamage;
	public Field destination;	
	public int stupidity;
	public float destinationOffsetRange;

	//Keep <=0.5;
	public float fieldDetectionRange;

	public bool showHPbar;
	public Vector3 healthBarOffset;
	public SpriteRenderer healthbar;

	private Board board;
	private int maxHealth;
	private float stepLeft;
	private Field oldCurrentField;
	private Vector2 currentDestinationOffset;

	public virtual void Awake ()
	{
		maxHealth = this.health;
		if (!showHPbar) {
			healthbar.enabled = false;
		} else {
			updateHealthBar ();
		}
	}



	public void FixedUpdate ()
	{
		stepLeft = Time.fixedDeltaTime * speed;
		while (stepLeft>0) {
			actOnPosition ();
			stepLeft = moveTowardsDestination (stepLeft);


			//Prevent unity editor freezes, should never happen.
			if (stepLeft == Time.fixedDeltaTime * speed) {
				Debug.LogError ("STEP NOT LOWERED. Infinite loop protection activated.");
				stepLeft = 0;
			}
		}
	}

	private void actOnPosition ()
	{

		Field newCurrentField = getCurrentField ();
		if (newCurrentField != oldCurrentField && newCurrentField != destination) {
			setNewDestination ();
		}
		if (newCurrentField != null) {
			if (Vector2.Distance (getCurrentField ().transform.position, this.transform.position) < fieldDetectionRange) {
				//Somewhat close to a tower.
				if (getCurrentField ().isBlocking ()) {
					stepLeft = 0;
					destination.damageTower (this.towerDamage);
				}
			}
		}

		if (Vector3.Distance (this.transform.position, destination.transform.position) <= destinationOffsetRange) {
			setNewDestination ();
		}

		oldCurrentField = newCurrentField;
	}

	protected virtual float moveTowardsDestination (float step)
	{
		//2 Possible angle calculate methods.
		//float zAngle = Vector3.Angle (Vector3.up, destination - this.transform.position) * Mathf.Sign (Vector3.Cross (Vector3.up, destination - this.transform.position).z);
		Vector3 desPos = destination.transform.position + ((Vector3)currentDestinationOffset);
		float zAngle = Mathf.Atan2 (desPos.y - this.transform.position.y, desPos.x - this.transform.position.x) * Mathf.Rad2Deg - 90;





		float distance = Vector3.Distance (desPos, this.transform.position);



		this.transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, zAngle));
		healthbar.transform.rotation = Quaternion.identity;
		healthbar.transform.position = this.transform.position + healthBarOffset;
		this.transform.position = Vector3.MoveTowards (this.transform.position, desPos, step);
		return step - distance;
	}

	public virtual Field getCurrentField ()
	{
		return board.getField ((int)Mathf.Round (this.transform.position.x), (int)Mathf.Round (this.transform.position.y));
	}

	public virtual void hit (AbstractBullet bullet, int damage, bool announcedPreviously)
	{
		health = health - damage;

		//avoid based on impact location
		//increaseRoutingScore (getCurrentField (), damage);
		//Avoid based on location when the bullet target was aquired.
		increaseRoutingScore (bullet.targetAquiredField, damage);


		if (health <= 0) {
			die ();
		}
		if (announcedPreviously) {
			incomingDamage = incomingDamage - damage;
		}
		if (showHPbar) {
			healthbar.enabled = true;
			updateHealthBar ();
		} else {
			healthbar.enabled = false;
			//TODO possibly performance issue
		}
	}

	public virtual void setDestination (Field dest)
	{
		this.destination = dest;
	}

	public virtual void setNewDestination ()
	{
		Field nextField = decideBestField ();
		if (nextField != null) {
			destination = nextField;
			currentDestinationOffset = getRandomDestinationOffset ();
		} else {
			print ("Reached end");
			die ();
			stepLeft = 0;
		}
	}

	public virtual Vector2 getRandomDestinationOffset ()
	{
		float angle = Random.value * 2 * Mathf.PI;
		float offsetMagnitude = Random.value * destinationOffsetRange;
		return new Vector2 (Mathf.Cos (angle) * offsetMagnitude, Mathf.Sin (angle) * offsetMagnitude);
	}

	public virtual void die ()
	{
		Destroy (this.gameObject);
		board.updateRouting ();
	}

	public virtual void setBoard (Board board)
	{
		this.board = board;
	}

	public virtual Field decideBestField ()
	{
		if (getCurrentField () == board.endField) {
			return null;
		}
		int minCost = int.MaxValue;		
		foreach (Field f in getCurrentField().getNeighbours()) {
			if (f.costToReach < minCost) {
				minCost = f.costToReach;
			}
		}
		
		List<Field> viable = new List<Field> ();
		foreach (Field f in getCurrentField().getNeighbours()) {
			if (f.costToReach <= minCost + stupidity) {
				viable.Add (f);
			}
		}
		return viable [(int)(Random.value * viable.Count)];
	}

	public virtual bool isDieing ()
	{
		return (health - incomingDamage) <= 0;
	}

	public virtual void addIncomingDamage (int damage)
	{
		this.incomingDamage = this.incomingDamage + damage;
	}

	public void updateHealthBar ()
	{
		float percent = (float)health / maxHealth;
		healthbar.gameObject.transform.localScale = new Vector3 (percent, 0.1f, 1f);
		healthbar.color = new Color ((1 - percent), percent, 0);
	}

	private void increaseRoutingScore (Field fieldToIncrease, int score)
	{
		if (fieldToIncrease != null) {
			fieldToIncrease.cost = fieldToIncrease.cost + score;
		}
	}


}
