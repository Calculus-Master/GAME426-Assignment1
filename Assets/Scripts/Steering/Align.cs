using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Align : SteeringBehavior
{
    // max angular acceleration (in radians)
    public float maxAngularAcceleration = 2.0f;
    // max rate of rotation (in radians)
    public float maxRotation  = 2.0f;

    // The radius for arriving at the target (in radians)
    public float targetRadius = 0.05f;

    // The radius for beginning to slow down (in radians)
    public float slowRadius = 1.0f;

    // The time over which to achieve target speed.
    public float timeToTarget = 0.1f;

    // Overrides the Align.target data member
    protected Kinematic target;
    
    public override SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();
        var rotation = target.orientation - character.orientation;

        rotation = Mathf.Deg2Rad * Mathf.DeltaAngle(0.0f, rotation * Mathf.Rad2Deg);
        var rotationSize = Mathf.Abs(rotation);

        if (rotationSize < targetRadius)
        {
            return result;
        }

        float targetRotation = maxRotation;
        if (rotationSize <= slowRadius)
        {
            targetRotation = maxRotation * rotationSize / slowRadius;
        }

        targetRotation *= rotation / rotationSize;

        result.angular = targetRotation - character.rotation;
        result.angular /= timeToTarget;

        var angularAcceleration = Mathf.Abs(result.angular);
        if (angularAcceleration > maxAngularAcceleration)
        {
            result.angular /= angularAcceleration;
            result.angular *= maxAngularAcceleration;
        }

        return result;
    }
}
