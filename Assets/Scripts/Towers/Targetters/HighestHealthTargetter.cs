using System;
using UnityEngine;
public class HighestHealthTargetter : AbstractTargetter
{
	//pure
	public override BasicCreep chooseCreep (BasicTower tower)
	{
		GameObject[] creeps = GameObject.FindGameObjectsWithTag ("Creep");
		double highestHealth = 0;
		BasicCreep highestHealthCreep = null;
		foreach (GameObject gameObject in creeps) {
			BasicCreep creep = gameObject.GetComponent<BasicCreep> ();
			if (creep != null) {
				if (Vector3.Distance (tower.transform.position, creep.transform.position) < tower.fireRange) {
					if (creep.health > highestHealth && (!tower.bulletType.announcesDamage || !creep.isDieing ())) {
						highestHealth = creep.health;
						highestHealthCreep = creep;
					}
				} 
			}
		}
		return highestHealthCreep;
	}
}

