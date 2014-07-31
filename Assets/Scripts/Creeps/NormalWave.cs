using UnityEngine;
using System.Collections;

public class BasicWave : AbstractWave
{
	public int framesInBetween;
	private int cooldown;
	private bool waveStart = false;
	private int totalCreeps;


	public void Start ()
	{
		totalCreeps = 0;
		foreach (int i in amountPerType) {
			totalCreeps = totalCreeps + i;
		}
	}


	public void FixedUpdate ()
	{
		if (cooldown > 0) {
			cooldown--;
		} else {
			spawnRandomCreep ();
			cooldown = framesInBetween;
		}
	}

	private void spawnRandomCreep ()
	{
		int random = (int)(Random.value * totalCreeps);
		int index = 0;
		foreach (int i in amountPerType) {
			if (i < random) {
				random = random - i;
				index++;
			} else {
				break;
			}
		}

		amountPerType [index]--;
		totalCreeps--;
		BasicCreep newCreep = Instantiate (creepTypes [index], startPosition, Quaternion.identity) as BasicCreep;
		newCreep.setBoard (board);
		newCreep.setDestination (targetPosition);
		newCreep.transform.parent = this.transform;
		newCreep.stupidity = creepStupidity;
		newCreep.health = newCreep.health * difficulty;
		newCreep.speed = newCreep.speed * ((difficulty - 1) * 1.3f + 1);

		if (totalCreeps <= 0) {
			GameController.getInstance ().waveSpawningDone ();
		}
	}
}
