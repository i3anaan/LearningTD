using UnityEngine;
using System.Collections;
using System;


/**
 * Abstract wave class.
 * 
 * Have one 'example' of this under a WaveSpawner.
 * Each time this wave gets spawned, it gets cloned.
 * calling spawnCreeps() on the wave starts the spawning.
 * 
 * Do NOT put the clones under itself, this will recursively creat more clones.
 * Put clones under a different GameObject 'Instantiated Waves'.
 * Clones get put under the first child of WaveSpawner, this should be an empty gameobject.
 * */
public class AbstractWave : MonoBehaviour
{
	public Vector3 startPosition;
	public Field targetField;

	public BasicCreep[] creepTypes;
	public int[] amountPerType;
	public double difficulty;
	public int creepStupidity;
	public Board board;

	protected WaveSpawner waveSpawner;
	protected AbstractWave waveIteration;
	private int iterationNumber; 		//How many time this wave has been spawned (for endless)

	//Should call WaveSpawner.WaveDone() when this wave has finished spawning;
	public virtual void spawnCreeps ()
	{
		print ("Spawning wave: " + this.gameObject.name);
		waveSpawner = this.gameObject.transform.parent.transform.parent.gameObject.GetComponent<WaveSpawner> ();
		if (waveSpawner == null) {
			Debug.LogError ("Incorrect usage of AbstractWave. See code at the print statement for details");
			/* Hierarchy should be this:
			 *	WaveSpawner
			 *		Instantiated Waves
			 *			Wave 1
			 *			Wave 2
			 *			Wave X
			 * 		AbstractWave
			 * 
			 * Call makeNewIteration() on the AbstractWave.
			 * Call spawnCreeps() on the AbstractWave Instantces, creaded under Instantiated Waves.			 * 
			 * */
		}
	}

	public AbstractWave makeNewIteration ()
	{
		iterationNumber++;
		waveIteration = Instantiate (this, Vector3.zero, Quaternion.identity) as AbstractWave;
		waveIteration.transform.parent = this.transform.parent.transform.GetChild (0);
		waveIteration.name = "[" + (this.transform.parent.transform.GetChild (0).childCount - 1) + "]  " + this.gameObject.name + "\t(Iteration " + iterationNumber + ")";

		return waveIteration;
	}

	public virtual void waveDone ()
	{
		waveSpawner.waveSpawningDone ();
		this.enabled = false;
	}
}
