using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BasicTower : MonoBehaviour
{
	public new bool enabled = false;
	public AbstractBullet bulletType;
	public int fireRate;
	public double fireRange;
	public int bulletDamage;
	public float bulletSpeed;
	public int towerHealth;
	public bool isBlocking = true;
	private int fireCooldown;
	public Field fieldPlacedOn;
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
		AbstractBullet bullet = Instantiate (bulletType, this.transform.position, Quaternion.identity) as AbstractBullet;
		bullet.transform.parent = this.transform;
		bullet.damage = bulletDamage;
		bullet.speed = bulletSpeed;
		bullet.targetAquiredField = creep.getCurrentField ();

		bullet.target (creep);
		fireCooldown = 0;
	}
	
	public virtual double getDistance (BasicCreep creep)
	{
		return Vector3.Distance (this.transform.position, creep.transform.position);
	}

	public virtual int getCost ()
	{
		return 50;
	}

	public virtual void takeDamage (int damage)
	{
		this.towerHealth = this.towerHealth - damage;
		if (towerHealth < 0) {
			die ();
		}
	}

	public void die ()
	{
		this.fieldPlacedOn.tower = null;
		Destroy (this.gameObject);
	}
}
