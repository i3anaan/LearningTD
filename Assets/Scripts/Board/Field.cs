using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Field : MonoBehaviour
{
		public BasicTower tower;
		public Field nextField;
		public Board board;
		public TextMesh debug;

		public int costToReach = int.MaxValue;
		public int cost = 1;


		void FixedUpdate ()
		{
				debug.text = "";
		}

		void OnMouseDrag ()
		{
				if (!tower) {
						print ("Transformed into tower");
						tower = true;
						this.renderer.material = towerMaterial;
						//this.deaths++;
						//board.updateRouting ();
				}
		}

		public List<Field> getNeighbours ()
		{
				List<Field> result = new List<Field> ();
				Vector3 pos = this.transform.position;
				for (int o=-1; o<2; o=o+2) {
						Field neighbour = board.getField ((int)(pos.x + o), (int)(pos.y));
						if (neighbour != null) {
								result.Add (neighbour);
						}
						neighbour = board.getField ((int)(pos.x), (int)(pos.y + o));
						if (neighbour != null) {
								result.Add (neighbour);
						}
				}
				return result;
		}

		public int getRoutingScore ()
		{
				return cost + (tower ? 50 : 0);
		}
}