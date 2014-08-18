using System;
using UnityEngine;
public class LowestHealthTargetter : AbstractTargetter
{
	//pure
	public override BasicCreep chooseCreep (BasicTower tower)
	{
		GameObject[] creeps = GameObject.FindGameObjectsWithTag ("Creep");
		double lowestHealth = double.MaxValue;
		BasicCreep lowestHealthCreep = null;
		foreach (GameObject gameObject in creeps) {
			BasicCreep creep = gameObject.GetComponent<BasicCreep> ();
			if (creep != null) {
				if (Vector3.Distance (tower.transform.position, creep.transform.position) < tower.fireRange) {
					if (creep.health < lowestHealth && creep.health > 0 && (!tower.bulletType.announcesDamage || !creep.isDieing ())) {
						lowestHealth = creep.health;
						lowestHealthCreep = creep;
					}
				} 
			}
		}
		return lowestHealthCreep;
	}
}

