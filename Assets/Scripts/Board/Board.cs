using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Board : MonoBehaviour
{
	public Field fieldToPlace;
	public Field[,] board;

	public Field endField;
	public Field startField;

	// Use this for initialization
	void Start ()
	{
		board = new Field[10, 10];
		for (int x=0; x<10; x++) {
			for (int y=0; y<10; y++) {

				board [x, y] = Instantiate (fieldToPlace, new Vector3 (x, y, 0.1f), Quaternion.identity) as Field;
				board [x, y].transform.parent = this.transform;
				board [x, y].board = this;
				if (x <= 4 && y == 2) {
					board [x, y].routable = false;
				}
			}
		}
		startField = board [0, 0];
		endField = board [9, 9];
		updateRouting ();
	}

	public Field getField (int x, int y)
	{
		if (x < 0 || x > 9 || y < 0 || y > 9) {
			//Out of bounds;
			//print ("Requesting out of bounds Field: " + x + ", " + y);
			return null;
		}
		return board [x, y];
	}

	public void updateRouting ()
	{
		//Setup frontier.
		endField.costToReach = endField.getRoutingScore ();
		List<Field> finalized = new List<Field> ();
		List<Field> frontier = new List<Field> ();
		finalized.Add (endField);
		frontier.AddRange (endField.getRoutableNeighbours ());
		foreach (Field f in frontier) {
			f.nextField = endField;
			f.costToReach = endField.getRoutingScore ();
		}
				
		while (frontier.Count>0) {
			//print ("Frontier count: " + frontier.Count);
			//Finalize current best move
			Field nextShortest = getAndRemoveShortest (frontier);
			finalized.Add (nextShortest);
			List<Field> neighbours = nextShortest.getRoutableNeighbours ();

			//Update frontier with new routing information.
			foreach (Field n in neighbours) {
				if (!finalized.Contains (n)) {
					//Update route if better round found.
					if (!frontier.Contains (n)) {
						//New entry, reset costToReach, add to frontier.
						n.costToReach = nextShortest.costToReach + n.getRoutingScore ();
						n.nextField = nextShortest;
						frontier.Add (n);
					}
					if (nextShortest.costToReach + n.getRoutingScore () < n.costToReach) {
						n.costToReach = nextShortest.costToReach + n.getRoutingScore ();
						n.nextField = nextShortest;
					}
				} else {
					//Ignore, already processed;
				}
			}

		}
				
	}

	private Field getAndRemoveShortest (List<Field> frontier)
	{
		Field closest = null;
		int minCost = int.MaxValue;
		foreach (Field f in frontier) {
			int cost = f.nextField.costToReach + f.getRoutingScore ();
			if (cost < minCost) {
				minCost = cost;
				closest = f;
			}
		}
		frontier.Remove (closest);
		return closest;
	}

}
