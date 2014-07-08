using UnityEngine;
using System.Collections;

public class Creep : MonoBehaviour
{

		public int health;
		public float speed;
		public Vector3 destination;	
		private Board board;


		void FixedUpdate ()
		{
				float step = Time.fixedDeltaTime * speed;
				float zAngle = Vector3.Angle (Vector3.up, destination - this.transform.position);
				this.transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, -zAngle));
				this.transform.position = Vector3.MoveTowards (this.transform.position, destination, step);
				if (this.transform.position == destination) {
						//print ("Board = " + board);
						//print ("AtField: " + (int)this.transform.position.x + ", " + (int)this.transform.position.y);
						Field nextField = getCurrentField ().nextField;
						if (nextField != null) {
								destination = nextField.transform.position;
						} else {
								print ("Reached end");
								die ();
						}
				}
		}

		private Field getCurrentField ()
		{
				return board.getField ((int)Mathf.Round (this.transform.position.x), (int)Mathf.Round (this.transform.position.y));
		}

		public void hit (int damage)
		{
				health = health - damage;
				if (health <= 0) {
						die ();
				}
		}

		public void setDestination (Vector3 dest)
		{
				this.destination = dest;
		}

		public void die ()
		{
				Destroy (this.gameObject);
				getCurrentField ().deaths++;
				board.updateRouting ();

		}

		public void setBoard (Board board)
		{
				this.board = board;
		}
}
