using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
	public BasicTower towerSelected;

	private static GameController instance;

	public int increaseWaveTimer;
	public int spawnTime;
	public BasicCreep creep;
	private int ticks;
	public int spawnCount;
	public Board board;
	public int creepStupidity;
	public int creepTotal = 0;

	public virtual void Awake ()
	{
		instance = this;
	}	

	public static GameController getInstance ()
	{
		if (instance == null) {
			Debug.LogError ("GAMECONTROLLER NOT YET INSTANTIATED");
		}
		return instance;
	}

	// Use this for initialization
	public virtual void FixedUpdate ()
	{
		ticks++;
		if (ticks % ((int)(spawnTime / Time.fixedDeltaTime)) == 0) {
			//Spawn units;
			for (int i=0; i<spawnCount; i++) {
				BasicCreep newCreep = Instantiate (creep, new Vector3 (Random.value - 1f, Random.value - 1f, 0f), Quaternion.identity) as BasicCreep;
				creepTotal++;
				newCreep.setBoard (board);
				newCreep.setDestination (new Vector3 (0f, 0f, 0f));
				newCreep.transform.parent = this.transform;
				newCreep.stupidity = creepStupidity;
			}
		}
		if (ticks % ((int)(increaseWaveTimer / Time.fixedDeltaTime)) == 0) {
			//Update wave size;
			spawnCount++;
		}
	}
}
