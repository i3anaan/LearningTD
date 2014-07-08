using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
		


		public int increaseWaveTimer;
		public int spawnTime;
		public Creep creep;
		private int ticks;
		public int spawnCount;
		public Board board;

		// Use this for initialization
		void FixedUpdate ()
		{
				ticks++;
				if (ticks % ((int)(spawnTime / Time.fixedDeltaTime)) == 0) {
						//Spawn units;
						for (int i=0; i<spawnCount; i++) {
								Creep newCreep = Instantiate (creep, new Vector3 (Random.value - 1f, Random.value - 1f, 0f), Quaternion.identity) as Creep;
								newCreep.setDestination (new Vector3 (0f, 0f, 0f));
								newCreep.transform.parent = this.transform;
								newCreep.setBoard (board);
						}
				}
				if (ticks % ((int)(increaseWaveTimer / Time.fixedDeltaTime)) == 0) {
						//Update wave size;
						spawnCount++;
				}
		}
}
