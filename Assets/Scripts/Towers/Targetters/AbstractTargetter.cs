using UnityEngine;
using System.Collections;

public abstract class AbstractTargetter : MonoBehaviour
{
	public abstract BasicCreep chooseCreep (BasicTower tower);
}
