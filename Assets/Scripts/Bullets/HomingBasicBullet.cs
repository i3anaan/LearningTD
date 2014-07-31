using UnityEngine;
using System.Collections;

public class HomingBasicBullet : AbstractBullet
{


	public void Start ()
	{
		if (announcesDamage) {
			targetCreep.addIncomingDamage (damage);
		}
	}

	public virtual void FixedUpdate ()
	{
		if (targetCreep != null) {
			float step = Time.fixedDeltaTime * speed;
			if (step > Vector3.Distance (this.transform.position, targetCreep.transform.position)) {
				//Kill
				targetCreep.hit (damage, announcesDamage);
				Destroy (this.gameObject);
			} else {
				this.transform.position = Vector3.MoveTowards (this.transform.position, targetCreep.transform.position, step);
			}
		}
		if (targetCreep == null) {
			Destroy (this.gameObject);
		}
	}
}
