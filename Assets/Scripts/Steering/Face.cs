using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : Align
{
    public Kinematic target;

    public override SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();

        Vector3 direction = target.position - character.position;

        if (Mathf.Approximately(direction.magnitude, 0))
        {
            return result;
        }

        base.target = target;
        base.target.orientation = Mathf.Atan2(direction.x, direction.z);
        return base.getSteering();
    }
}

