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
}
