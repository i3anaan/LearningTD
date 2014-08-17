using UnityEngine;
using System.Collections;

public class BigPenetratingBullet : AbstractBullet
{
	private Vector3 direction;
	public int lifeTime;
	public bool destroyOnHit;

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
			creepHit.hit (this, damage, false);
		}
		if (destroyOnHit) {
			Destroy (this.gameObject);
		}
	}
	public override void target (BasicCreep creep)
	{
		float step = Time.fixedDeltaTime * speed;
		//TODO not yet working;
		//float zAngle = Vector3.Angle (Vector3.up, destination - this.transform.position) * -Mathf.Sign (Vector3.Cross (Vector3.up, destination - this.transform.position).z);
		Vector3 dirNor = (creep.transform.position - this.transform.position).normalized;
		if (dirNor.magnitude == 0) {
			dirNor = new Vector3 (Random.value * 2 - 1, Random.value * 2 - 1, 0).normalized;
		}
		this.direction = dirNor * step;
	}
}
