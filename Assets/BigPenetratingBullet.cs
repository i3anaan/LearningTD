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

	public void OnCollisionEnter2D (Collision2D other)
	{
		print ("Collided with: " + other);
		//if (other.gameObject is BasicCreep) {
		//	((BasicCreep)other.gameObject).hit (damage);
		//}
	}

	public override void target (BasicCreep creep)
	{
		float step = Time.fixedDeltaTime * speed;
		this.direction = Vector3.MoveTowards (this.transform.position, creep.transform.position, step) - this.transform.position;
	}
}
