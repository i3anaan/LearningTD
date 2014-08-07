using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicCreep : MonoBehaviour
{

	public int health;
	public int incomingDamage;
	public float speed;
	public Vector3 destination;	
	public int stupidity;

	public bool showHPbar;
	public SpriteRenderer healthbar;

	private Board board;
	private int maxHealth;

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
		float step = Time.fixedDeltaTime * speed;
		float zAngle = Vector3.Angle (Vector3.up, destination - this.transform.position);
		this.transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, -zAngle));
		this.transform.position = Vector3.MoveTowards (this.transform.position, destination, step);
		if (this.transform.position == destination) {
			Field nextField = decideBestField ();
			if (nextField != null) {
				destination = nextField.transform.position;
			} else {
				print ("Reached end");
				die ();
			}
		}
	}

	public virtual Field getCurrentField ()
	{
		//print ("Board: " + board + "  x,y: " + (int)Mathf.Round (this.transform.position.x) + ", " + (int)Mathf.Round (this.transform.position.y));
		return board.getField ((int)Mathf.Round (this.transform.position.x), (int)Mathf.Round (this.transform.position.y));
	}

	public virtual void hit (int damage, bool announcedPreviously)
	{
		health = health - damage;
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
		}
	}

	public virtual void setDestination (Vector3 dest)
	{
		this.destination = dest;
	}

	public virtual void die ()
	{
		Destroy (this.gameObject);
		getCurrentField ().cost++;
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

	public virtual bool dieing ()
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


}
