using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : SteeringBehavior
{
    public float maxAcceleration = 0.5f;

    // the target's kinematic properties
    protected Kinematic target;
    public override SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();

        result.linear = target.position - character.position;
        result.linear = result.linear.normalized * maxAcceleration;
        return result;
    }
}
