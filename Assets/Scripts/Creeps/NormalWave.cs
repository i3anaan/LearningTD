using UnityEngine;
using System.Collections;

public class NormalWave : AbstractWave
{
	public int framesInBetween;
	private int cooldown;
	private bool waveStart = false;
	private int totalCreepsToSpawn;
	private int creepsSpawned;

	public bool spawning = false;




	public override void spawnCreeps ()
	{
		base.spawnCreeps ();
		totalCreepsToSpawn = 0;
		creepsSpawned = 0;
		foreach (int i in amountPerType) {
			totalCreepsToSpawn = totalCreepsToSpawn + i;
		}
		spawning = true;
	}


	public void FixedUpdate ()
	{
		if (spawning) {
			if (cooldown > 0) {
				cooldown--;
			} else {
				spawnRandomCreep ();
				cooldown = framesInBetween;
			}
		}
	}

	private void spawnRandomCreep ()
	{
		int random = (int)(Random.value * (totalCreepsToSpawn - creepsSpawned));
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
		creepsSpawned++;
		BasicCreep newCreep = Instantiate (creepTypes [index], startPosition, Quaternion.identity) as BasicCreep;
		newCreep.setBoard (board);
		newCreep.setDestination (targetField);
		newCreep.transform.parent = this.transform;
		newCreep.stupidity = creepStupidity;
		newCreep.health = (int)(newCreep.health * difficulty);
		newCreep.speed = newCreep.speed;

		if (creepsSpawned >= totalCreepsToSpawn) {
			waveDone ();
		}
	}
}
