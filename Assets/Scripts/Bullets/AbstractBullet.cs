using UnityEngine;
using System.Collections;
using System;
public class AbstractBullet : MonoBehaviour
{
	protected BasicCreep targetCreep;
	[HideInInspector]
	public float
		speed;
	[HideInInspector]
	public int
		damage;
    [HideInInspector]
    public float accuracyPenalty;


    [HideInInspector]
	public Field
		targetAquiredField; //Field the target was in when this bullet was fired;



	public bool announcesDamage; //Same as DontOverkill

	public virtual void target (BasicCreep creep)
	{
		this.targetCreep = creep;
	}
}
