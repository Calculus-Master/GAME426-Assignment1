using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Steering/Face")]
public class Face : Align
{
    // public Kinematic target;

    public override SteeringOutput getSteering(Kinematic character, Kinematic target)
    {
        SteeringOutput result = new SteeringOutput();

        Vector3 direction = target.position - character.position;

        if (Mathf.Approximately(direction.magnitude, 0))
        {
            return result;
        }

        // base.target = target;
        target = new Kinematic(target.targetObj);
        target.orientation = Mathf.Atan2(direction.x, direction.z);
        return base.getSteering(character, target);
    }
}

