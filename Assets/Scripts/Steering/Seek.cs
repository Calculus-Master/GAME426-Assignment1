using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Steering/Seek")]
public class Seek : SteeringBehavior
{
    public float maxAcceleration = 0.5f;

    protected Kinematic target;
    public override SteeringOutput getSteering(Kinematic character, Kinematic target, RoomManager manager)
    {
        SteeringOutput result = new SteeringOutput();

        if (target == null)
        {
            Debug.LogError($"[Seek] {character.targetObj.name}: Target is NULL!");
            return result;
        }

        result.linear = target.position - character.position;
        result.linear = result.linear.normalized * maxAcceleration;

        Debug.Log($"[Seek] {character.targetObj.name} moving toward {target.targetObj.name} with acceleration {maxAcceleration}.");

        return result;
    }

}
