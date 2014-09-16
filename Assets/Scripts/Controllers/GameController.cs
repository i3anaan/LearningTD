using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour
{
    public PassiveTower towerSelected;

	private static GameController instance;

	public int increaseWaveTimer;
	public int spawnTime;
	public BasicCreep creep;
	private int ticks;
	public int spawnCount;
	public Board board;
	public int creepStupidity;
	public int creepTotal = 0;

    public int currentGold;
    public int livesLeft;

    public void damageLives(int lives)
    {
        print("GC.damageLives(" + lives + ")");
        livesLeft = livesLeft - lives;
        if (livesLeft <= 0)
        {
            print("Game Over!");
            Application.LoadLevel(Application.loadedLevel);
        }
    }

    public bool spendGold(int goldSpend)
    {
        if (currentGold >= goldSpend)
        {
            currentGold = currentGold - goldSpend;
            return true;
        }
        else
        {
            return false;
        }
    }

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
