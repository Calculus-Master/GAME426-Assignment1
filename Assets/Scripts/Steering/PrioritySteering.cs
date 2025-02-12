using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Steering/PrioritySteering")]
public class PrioritySteering : SteeringBehavior
{
    public List<BlendedSteering> groups;
    
    public override SteeringOutput getSteering(Kinematic character, Kinematic target)
    {
        SteeringOutput steering = new SteeringOutput();
        foreach (var group in groups)
        {
            steering = group.getSteering(character, target);
            if (steering.linear.magnitude > Mathf.Epsilon || Mathf.Abs(steering.angular) > Mathf.Epsilon)
            {
                return steering;
            }
        }

        return steering;
    }
}
