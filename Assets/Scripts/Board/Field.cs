using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Field : MonoBehaviour
{
	public int showLocalCost;
    public PassiveTower tower;
	public Field[] nextField;
	public Board board;
	public TextMesh debug;
	public bool routable;

	public int[] costToReach;
	public int[] cost;
    private GameController gc;


	public virtual void Awake ()
	{
		costToReach = new int[Board.ROUTING_TYPE_COUNT];
		cost = new int[Board.ROUTING_TYPE_COUNT];
		nextField = new Field[Board.ROUTING_TYPE_COUNT];
		for (int i=0; i<Board.ROUTING_TYPE_COUNT; i++) {
			costToReach [i] = int.MaxValue;
			cost [i] = 1;
		}
	}

    public virtual void Start()
    {
        gc = GameController.getInstance();
    }


	public virtual void FixedUpdate ()
	{
		if (routable && showLocalCost >= 0) {
			debug.text = "" + getRoutingScore (showLocalCost);
		}
	}


	public virtual void OnMouseDown ()
	{
		if (tower == null) {
			Vector3 mousePos = Input.mousePosition;
			Vector3 worldMousePos = Camera.main.ScreenToWorldPoint (mousePos);
			if (Mathf.Round (worldMousePos.x) == this.transform.position.x && Mathf.Round (worldMousePos.y) == this.transform.position.y && GameController.getInstance ().towerSelected != null) {
                placeTower(gc.towerSelected);
			}
		}
	}

    public virtual bool placeTower(PassiveTower towerToPlace)
    {
        if (gc.spendGold(towerToPlace.goldCost))
        {
            tower = Instantiate(towerToPlace, this.transform.position - (new Vector3(0f, 0f, -1f)), Quaternion.identity) as PassiveTower;
            tower.fieldPlacedOn = this;
            tower.enabled = true;
            tower.transform.parent = board.transform;
            return true;
        }
        else
        {
            return false;
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

	public int getRoutingScore (int pathingTypeIndex)
	{
		int towerCost = ((tower == null) ? 0 : tower.getCost ());
		return cost [pathingTypeIndex] + towerCost;
	}


	public bool isBlocking ()
	{
		return this.tower != null && tower.isBlocking;
	}


	public void damageTower (int damage)
	{
		if (tower != null) {
			this.tower.takeDamage (damage);
		}
	}
}