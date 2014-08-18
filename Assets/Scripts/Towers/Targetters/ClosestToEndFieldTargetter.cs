using System;
using UnityEngine;
public class ClosestToEndFieldTargetter : AbstractTargetter
{
	public GameController gc;

	public override BasicCreep chooseCreep (BasicTower tower)
	{
		if (gc == null) {
			gc = GameController.getInstance ();
		}

		GameObject[] creeps = GameObject.FindGameObjectsWithTag ("Creep");
		double minDistToEndField = double.MaxValue;
		BasicCreep closestCreep = null;
		foreach (GameObject gameObject in creeps) {
			BasicCreep creep = gameObject.GetComponent<BasicCreep> ();
			if (creep != null) {
				if (Vector3.Distance (tower.transform.position, creep.transform.position) < tower.fireRange) {
					if (Vector3.Distance (gc.board.endField.transform.position, creep.transform.position) < minDistToEndField && (!tower.bulletType.announcesDamage || !creep.isDieing ())) {
						minDistToEndField = Vector3.Distance (gc.board.endField.transform.position, creep.transform.position);
						closestCreep = creep;
					}
				} 
			}
		}
		return closestCreep;
	}
}

