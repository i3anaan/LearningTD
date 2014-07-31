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
		private Board board;


		void FixedUpdate ()
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

		private virtual Field getCurrentField ()
		{
				return board.getField ((int)Mathf.Round (this.transform.position.x), (int)Mathf.Round (this.transform.position.y));
		}

		public virtual void hit (int damage)
		{
				health = health - damage;
				if (health <= 0) {
						die ();
				}
		}

		public virtual void setDestination (Vector3 dest)
		{
				this.destination = dest;
		}

		public virtual void die ()
		{
				Destroy (this.gameObject);
				getCurrentField ().deaths++;
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
				//return getCurrentField ().nextField;
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
}
