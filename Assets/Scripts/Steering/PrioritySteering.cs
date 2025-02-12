using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrioritySteering : SteeringBehavior
{
    public List<BlendedSteering> groups;
    
    public override SteeringOutput getSteering()
    {
        SteeringOutput steering = new SteeringOutput();
        foreach (var group in groups)
        {
            steering = group.getSteering();
            if (steering.linear.magnitude > Mathf.Epsilon || Mathf.Abs(steering.angular) > Mathf.Epsilon)
            {
                return steering;
            }
        }

        return steering;
    }
}
