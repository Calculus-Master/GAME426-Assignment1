using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Steering/Flee")]
public class Flee : SteeringBehavior
{
    public float maxAcceleration = 0.5f; // Max acceleration for fleeing behavior

    public override SteeringOutput getSteering(Kinematic character, Kinematic target, RoomManager manager)
    {
        SteeringOutput result = new SteeringOutput();

        if (target == null)
        {
            Debug.LogError($"[Flee] {character.targetObj.name}: Target is NULL!");
            return result;
        }

        result.linear = character.position - target.position;
        result.linear = result.linear.normalized * maxAcceleration;

        Debug.Log($"[Flee] {character.targetObj.name} fleeing from {target.targetObj.name} with acceleration {maxAcceleration}.");

        return result;
    }

}
