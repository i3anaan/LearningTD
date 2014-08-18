using System;
using UnityEngine;
public class ClosestToTowerTargetter : AbstractTargetter
{

	//pure
	public override BasicCreep chooseCreep (BasicTower tower)
	{
		GameObject[] creeps = GameObject.FindGameObjectsWithTag ("Creep");
		double minDist = tower.fireRange;
		BasicCreep closest = null;
		foreach (GameObject gameObject in creeps) {
			BasicCreep creep = gameObject.GetComponent<BasicCreep> ();
			if (creep != null) {
				if (Vector3.Distance (tower.transform.position, creep.transform.position) < minDist) {
					if (!tower.bulletType.announcesDamage || !creep.isDieing ()) {
						closest = creep;
						minDist = Vector3.Distance (tower.transform.position, creep.transform.position);
					}
				} 
			}
		}

		return closest;
	}
}

