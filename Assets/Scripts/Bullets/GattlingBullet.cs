using UnityEngine;
using System.Collections;

public class GattlingBullet : StraightMovingBullet
{

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        //print ("Trigger collision!");
        //print ("OnTriggernEnter2D with: " + other);
        BasicCreep creepHit = other.gameObject.GetComponent<BasicCreep>();
        if (creepHit != null)
        {
            creepHit.hit(this, damage, false);
            GameObject.Destroy(this.gameObject);
        }

    }

    public override void target(BasicCreep creep)
    {
        base.target(creep);
        this.direction = Quaternion.Euler(0, 0, Random.Range(-1.0f, 1.0f) * accuracyPenalty) * direction;
    }
}
