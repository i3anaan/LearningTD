using UnityEngine;
using System.Collections;

public class BigPenetratingBullet : AbstractBullet
{
	private Vector3 direction;
	public int lifeTime;

	public virtual void FixedUpdate ()
	{
		this.transform.position = this.transform.position + direction;
		lifeTime--;
		if (lifeTime < 0) {
			Destroy (this.gameObject);
		}
	}
	public virtual void OnTriggerEnter2D (Collider2D other)
	{
		//print ("Trigger collision!");
		//print ("OnTriggernEnter2D with: " + other);
		BasicCreep creepHit = other.gameObject.GetComponent<BasicCreep> ();
		if (creepHit != null) {
			creepHit.hit (damage, false);
		}
	}
	public override void target (BasicCreep creep)
	{
		float step = Time.fixedDeltaTime * speed;
		this.direction = Vector3.MoveTowards (this.transform.position, creep.transform.position, step) - this.transform.position;
	}
}
