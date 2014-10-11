using UnityEngine;
using System.Collections;

public class WaveSpawner : MonoBehaviour
{
	private Board board;
    private GameController gc;
	public int timeBetweenWaves;
	public int maxTimePerWave;	//-1 means there is not maxTimePerWave
	public bool endless;		//True = Repeat over child waves.
	public int wave;
	public double difficulty = 1;

	public Vector3 startPosition;
	public Vector3 targetPosition;

	private bool waveSpawningIsDone = false;
	private bool allWavesDone;
	private int timeBeforeNextWave;
	private int timeLeftForWave;

	public AbstractWave[] waves;

    public void Awake()
    {
        if (GameObject.Find("Instantiated Waves") == null)
        {
            GameObject instantiatedWavesParentObject = new GameObject("Instantiated Waves");
            instantiatedWavesParentObject.transform.parent = this.transform;
        }
    }

	public void Start ()
	{
		waves = this.gameObject.GetComponentsInChildren<AbstractWave> ();
        gc = GameController.getInstance();
        board = gc.board;
		waveSpawningDone ();
	}


	public void waveSpawningDone ()
	{
		waveSpawningIsDone = true;
		timeBeforeNextWave = timeBetweenWaves;
	}

	public void FixedUpdate ()
	{
		if (timeBeforeNextWave > 0) {
			timeBeforeNextWave--;
		} else if (waveSpawningIsDone && !allWavesDone) {
			spawnNextWave ();
		}

		if (timeLeftForWave > 0) {
			timeLeftForWave--;
		} else if (timeLeftForWave == 0 && !allWavesDone) {
			spawnNextWave ();
		}
	}


	public void spawnNextWave ()
	{
		timeLeftForWave = maxTimePerWave;
		if (wave > waves.Length) {
			difficulty = difficulty * 1.2;
		}
		if (endless) {
			activateWave (wave % waves.Length);
		} else {
			if (wave < waves.Length) {
				activateWave (wave);
			} else {
				allWavesDone = true;
			}
		}

		waveSpawningIsDone = false;
		timeBeforeNextWave = 0;
		wave++;
	}

	public void activateWave (int index)
	{
		waves [index].startPosition = startPosition;
		waves [index].targetField = board.getField (Mathf.RoundToInt (targetPosition.x), Mathf.RoundToInt (targetPosition.y));
		AbstractWave newWave = waves [index].makeNewIteration ();
		newWave.difficulty = difficulty;
		newWave.spawnCreeps ();
	}

}
