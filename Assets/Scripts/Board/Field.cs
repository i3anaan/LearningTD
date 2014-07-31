using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Field : MonoBehaviour
{
	public bool showLocalCost;
	public BasicTower tower;
	public Field nextField;
	public Board board;
	public TextMesh debug;
	public bool routable;

	public int costToReach = int.MaxValue;
	public int cost = 1;



	public virtual void FixedUpdate ()
	{
		if (routable && showLocalCost) {
			debug.text = "" + cost;
		}

	}

	public virtual void OnMouseDrag ()
	{
		if (tower == null) {
			if (GameController.getInstance ().towerSelected != null) {
				tower = Instantiate (GameController.getInstance ().towerSelected, this.transform.position, Quaternion.identity) as BasicTower;
				tower.fieldPlacedOn = this;
				tower.enabled = true;		
			}
		}
	}

	public List<Field> getNeighbours ()
	{
		List<Field> result = new List<Field> ();
		Vector3 pos = this.transform.position;
		for (int o=-1; o<2; o=o+2) {
			Field neighbour = board.getField ((int)(pos.x + o), (int)(pos.y));
			if (neighbour != null) {
				result.Add (neighbour);
			}
			neighbour = board.getField ((int)(pos.x), (int)(pos.y + o));
			if (neighbour != null) {
				result.Add (neighbour);
			}
		}
		return result;
	}

	public List<Field> getRoutableNeighbours ()
	{
		List<Field> result = new List<Field> ();
		foreach (Field f in getNeighbours ()) {
			if (f.routable) {
				result.Add (f);
				//TODO optimize
			}
		}
		//print ("In: "+getRoutableNeighbours)
		return result;
	}

	public int getRoutingScore ()
	{
		int towerCost = ((tower == null) ? 0 : tower.getCost ());
		return cost + towerCost;
	}
}