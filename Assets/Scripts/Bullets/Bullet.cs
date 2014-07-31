﻿using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
		private Creep targetCreep;	
		public float speed;
		public int damage;
		
		void FixedUpdate ()
		{
				if (targetCreep != null) {
						float step = Time.fixedDeltaTime * speed;
						if (step > Vector3.Distance (this.transform.position, targetCreep.transform.position)) {
								//Kill
								targetCreep.hit (damage);
								Destroy (this.gameObject);
						} else {
								this.transform.position = Vector3.MoveTowards (this.transform.position, targetCreep.transform.position, step);
						}
				}
				if (targetCreep == null) {
						Destroy (this.gameObject);
				}
		}

		public void target (Creep creep)
		{
				this.targetCreep = creep;
		}
}
