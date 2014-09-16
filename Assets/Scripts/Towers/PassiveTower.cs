using UnityEngine;
using System.Collections;

public class PassiveTower : MonoBehaviour{

    public new bool enabled = false;
    public int towerHealth;
    public int goldCost;
    public bool isBlocking = true;
    public Field fieldPlacedOn;

    public virtual double getDistance(BasicCreep creep)
    {
        return Vector3.Distance(this.transform.position, creep.transform.position);
    }

    public virtual int getCost()
    {
        return 50;
    }

    public virtual void takeDamage(int damage)
    {
        this.towerHealth = this.towerHealth - damage;
        if (towerHealth < 0)
        {
            die();
        }
    }

    public void die()
    {
        this.fieldPlacedOn.tower = null;
        Destroy(this.gameObject);
    }


    public void OnMouseDown()
    {
        print("Selected: " + this);
        //TODO afmaken
        //(GameObject.FindGameObjectWithTag<GameGUI> ("GameGUI")).selectionHighlight.transform.position = this.transform.position - new Vector3 (0, 0, 0.1f);
    }
}
