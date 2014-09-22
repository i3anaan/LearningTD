using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicCreep : MonoBehaviour
{
    //Stats / Settings
	public int health;
	public float speed;
	public int towerDamage;
    public int goldBounty;
    public int endDamage;  //TODO better name
    private int maxHealth;
    public bool goldIndicator;

    //Routing
	public Field destination;	
	public int stupidity;
	public float destinationOffsetRange;
	public int routingType;
    public float fieldDetectionRange;    //Keep <=0.5;
    private Field oldCurrentField;
    private Vector2 currentDestinationOffset;
    
    //References
    private Board board;
    private AbstractWave wave;
	
    //HealthBar
	public bool showHPbar;
	public Vector3 healthBarOffset;
	public SpriteRenderer healthbar;
	
    //Stored variables
    [HideInInspector]
    public int incomingDamage;
	private float stepLeft;
	

	public virtual void Start ()
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
				//Somewhat close to a field.
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
		int damageDone = Mathf.Min (damage, health);
		health = health - damageDone;

		//avoid based on impact location
		//increaseRoutingScore (getCurrentField (), damage);
		//Avoid based on location when the bullet target was aquired.
		increaseRoutingScore (bullet.targetAquiredField, damageDone);


		if (health <= 0) {
			killed ();
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
			
			reachEnd();
			stepLeft = 0;
		}
	}

	public virtual Vector2 getRandomDestinationOffset ()
	{
		float angle = Random.value * 2 * Mathf.PI;
		float offsetMagnitude = Random.value * destinationOffsetRange;
		return new Vector2 (Mathf.Cos (angle) * offsetMagnitude, Mathf.Sin (angle) * offsetMagnitude);
	}

	public virtual void killed ()
	{
        wave.goldEarned(goldBounty);

        if (goldIndicator) {
            FloatingText ft = Instantiate(wave.getFloatingText_Gold(), Camera.main.WorldToViewportPoint(this.transform.position)+new Vector3(0,0,Random.Range(-0.1f,0.1f)), Quaternion.identity) as FloatingText;
            ft.guiText.text = this.goldBounty + "g";
        }

        Destroy (this.gameObject);
		board.updateRouting (routingType);
	}

    public virtual void reachEnd()
    {
        print("Reached end");
        wave.creepReachedEnd(this);
        Destroy(this.gameObject);
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
			if (f.costToReach [routingType] < minCost) {
				minCost = f.costToReach [routingType];
			}
		}
		
		List<Field> viable = new List<Field> ();
		foreach (Field f in getCurrentField().getNeighbours()) {
			if (f.costToReach [routingType] <= minCost + stupidity) {
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
			fieldToIncrease.cost [routingType] = fieldToIncrease.cost [routingType] + score;
		}
	}


    public void setWave(AbstractWave wave)
    {
        this.wave = wave;
    }

}
