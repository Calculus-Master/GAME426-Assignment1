using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Steering/Wander")]
public class Wander : Face
{
    public float wanderOffset = 1f;

    public float wanderRadius = 1f;

    public float wanderRate = 1f;

    public float maxAcceleration = 1f;

    private float wanderOrientation;

    
    public override SteeringOutput getSteering(Kinematic character, Kinematic target, RoomManager manager)
    {
        target = new Kinematic(target.targetObj);
        wanderOrientation += UnityEngine.Random.Range(-1, 1) * wanderRate;
        float targetOrientation = wanderOrientation + character.orientation;
        target.position = character.position + wanderOffset * 
            new Vector3(Mathf.Sin(character.orientation), 0f, Mathf.Cos(character.orientation));

        target.position += wanderRadius * new Vector3(Mathf.Sin(targetOrientation), 0f, Mathf.Cos(targetOrientation));

        var result = base.getSteering(character, target, manager);

        result.linear = maxAcceleration * 
                        new Vector3(Mathf.Sin(character.orientation), 0f, Mathf.Cos(character.orientation));
        return result;
    }
}
