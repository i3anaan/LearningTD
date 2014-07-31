using UnityEngine;
using System.Collections;
using System;
public class AbstractBullet : MonoBehaviour
{
	protected BasicCreep targetCreep;	
	public float speed;
	public int damage;
	public bool announcesDamage; //Same as DontOverkill

	public virtual void target (BasicCreep creep)
	{
		this.targetCreep = creep;
	}
}
