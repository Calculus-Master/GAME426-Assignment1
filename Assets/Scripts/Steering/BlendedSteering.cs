using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendedSteering : SteeringBehavior
{
    public class BehaviorAndWeight
    {
        public SteeringBehavior behavior;
        public float weight;
    }

    public List<BehaviorAndWeight> behaviors;
    public float maxAcceleration;
    public float maxRotation;

    public override SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();

        foreach (var behavior in behaviors)
        {
            var steeringOutput = behavior.behavior.getSteering();
            result.angular += behavior.weight * steeringOutput.angular;
            result.linear += behavior.weight * steeringOutput.linear;
        }

        if (Mathf.Abs(result.angular) > maxRotation)
        {
            result.angular /= Mathf.Abs(result.angular);
            result.angular *= maxRotation;
        }

        if (result.linear.magnitude > maxAcceleration)
        {
            result.linear = result.linear.normalized * maxAcceleration;
        }
        
        return result;
    }
}
