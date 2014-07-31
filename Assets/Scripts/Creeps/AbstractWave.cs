using UnityEngine;
using System.Collections;

public class AbstractWave : MonoBehaviour
{
	public Vector3 startPosition;
	public Vector3 targetPosition;

	public BasicCreep[] creepTypes;
	public int[] amountPerType;
	public int difficulty;
	public int creepStupidity;
	public Board board;

	//Should call GameController.WaveDone() when this wave has finished spawning;
}
