using UnityEngine;
using System.Collections;

public class BigPenetratingBullet : StraightMovingBullet
{
	public bool destroyOnHit;


	public virtual void OnTriggerEnter2D (Collider2D other)
	{
		//print ("Trigger collision!");
		//print ("OnTriggernEnter2D with: " + other);
		BasicCreep creepHit = other.gameObject.GetComponent<BasicCreep> ();
		if (creepHit != null) {
			creepHit.hit (this, damage, false);
		}
		if (destroyOnHit) {
			Destroy (this.gameObject);
		}
	}
}
