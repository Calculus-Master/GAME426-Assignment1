using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Steering/PrioritySteering")]
public class PrioritySteering : SteeringBehavior
{
    //TODO: priority queue or something
    public List<SteeringBehavior> groups;
    
    public override SteeringOutput getSteering(Kinematic character, Kinematic target, RoomManager manager)
    {
        SteeringOutput steering = new SteeringOutput();
        foreach (var group in groups)
        {
            steering = group.getSteering(character, target, manager);
            if (steering.linear.magnitude > Mathf.Epsilon || Mathf.Abs(steering.angular) > Mathf.Epsilon)
            {
                return steering;
            }
        }

        return steering;
    }
}
