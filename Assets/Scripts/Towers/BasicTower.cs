using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicTower : PassiveTower
{
	public AbstractBullet bulletType;
	public int fireRate;
	public double fireRange;
	public int bulletDamage;
	public float bulletSpeed;
    public float bulletAccuracyPenalty;
	private int fireCooldown;
	public AbstractTargetter targetter;

	public virtual void FixedUpdate ()
	{
		if (enabled) {
			fireCooldown++;
			if (fireCooldown >= fireRate) {
				fire ();
			}
		}
	}
	
	public virtual void fire ()
	{
		BasicCreep target = targetter.chooseCreep (this);

		if (target != null) {
			shootAt (target);
		}
	}
	
	public virtual void shootAt (BasicCreep creep)
	{
        Vector3 bulletSpawnPos = this.transform.position;
        bulletSpawnPos.z = -1;
        AbstractBullet bullet = Instantiate(bulletType, bulletSpawnPos, Quaternion.identity) as AbstractBullet;
		bullet.transform.parent = this.transform;
		bullet.damage = bulletDamage;
		bullet.speed = bulletSpeed;
        bullet.accuracyPenalty = bulletAccuracyPenalty;
		bullet.targetAquiredField = creep.getCurrentField ();

		bullet.target (creep);
		fireCooldown = 0;
	}
}
