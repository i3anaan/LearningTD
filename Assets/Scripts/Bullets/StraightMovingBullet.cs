using UnityEngine;
using System.Collections;

public class StraightMovingBullet : AbstractBullet
{
	protected Vector3 direction;
	public int lifeTime;
	

	public virtual void FixedUpdate ()
	{
		this.transform.position = this.transform.position + direction;
		lifeTime--;
		if (lifeTime < 0) {	
			Destroy (this.gameObject);
		}
        rotate();
	}

    public virtual void rotate()
    {
        float zAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90;
        this.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, zAngle));
    }


    public override void target(BasicCreep creep)
    {
        float step = Time.fixedDeltaTime * speed;
        Vector3 dirNor = (creep.transform.position - this.transform.position).normalized;
        dirNor.z = 0;
        if (dirNor.magnitude == 0)
        {
            dirNor = new Vector3(Random.value * 2 - 1, Random.value * 2 - 1, 0).normalized;
        }
        this.direction = dirNor * step;
        rotate();
    }
}

