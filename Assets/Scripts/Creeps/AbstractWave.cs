using UnityEngine;
using System.Collections;
using System;

public class AbstractWave : MonoBehaviour
{
	public Vector3 startPosition;
	public Vector3 targetPosition;

	public BasicCreep[] creepTypes;
	public int[] amountPerType;
	public float difficulty;
	public int creepStupidity;
	public Board board;

	private int iterationNumber; 		//How many time this wave has been spawned (for endless)
	protected GameObject waveIteration;
	protected WaveSpawner waveSpawner;
	public int[] amountPerTypeOriginal;

	//Should call WaveSpawner.WaveDone() when this wave has finished spawning;
	public virtual void OnEnable ()
	{
		iterationNumber++;
		waveIteration = new GameObject ("Iteration: " + iterationNumber);
		waveIteration.transform.parent = this.transform;
		print ("Spawning wave: " + this.gameObject.name);
		amountPerTypeOriginal = new int[amountPerType.Length];
		Array.Copy (amountPerType, amountPerTypeOriginal, amountPerType.Length);
		waveSpawner = this.gameObject.transform.parent.gameObject.GetComponent<WaveSpawner> ();
	}



	public void resetCreepAmount ()
	{
		Array.Copy (amountPerTypeOriginal, amountPerType, amountPerTypeOriginal.Length);
	}

	public virtual void waveDone ()
	{
		waveSpawner.waveSpawningDone ();
		this.resetCreepAmount ();
		this.enabled = false;
	}
}
