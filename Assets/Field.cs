using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Field : MonoBehaviour
{

		public bool tower = false;
		public int fireRate;
		public int fireCooldown;
		public double fireRange;
		public Bullet bulletType;
		public Material towerMaterial;
		public Field nextField;
		public Board board;
		public TextMesh debug;

		public int costToReach = int.MaxValue;
		public int deaths = 1;


		void FixedUpdate ()
		{
				if (tower) {
						fireCooldown++;
						if (fireCooldown >= fireRate) {
								fire ();
						}
				}
				//debug.text = getRoutingScore () + "";
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

		private void fire ()
		{
				GameObject[] creeps = GameObject.FindGameObjectsWithTag ("Creep");
				double minDist = fireRange;
				Creep closest = null;
				foreach (GameObject gameObject in creeps) {
						Creep creep = gameObject.GetComponent<Creep> ();
						if (creep != null) {
								if (getDistance (creep) < minDist) {
										closest = creep;
										minDist = getDistance (creep);
								} 
						}
				}

				if (closest != null) {
						shootAt (closest);
				}
		}

		private void shootAt (Creep creep)
		{
				Bullet bullet = Instantiate (bulletType, this.transform.position, Quaternion.identity) as Bullet;
				bullet.target (creep);
				bullet.transform.parent = this.transform;
				fireCooldown = 0;
		}

		private double getDistance (Creep creep)
		{
				return Vector3.Distance (this.transform.position, creep.transform.position);
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
				return deaths + (tower ? 50 : 0);
		}
}