using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Steering/Pursue")]
public class Pursue : Seek
{
    public float maxPredictionTime = 1.5f; // Max time to predict hero's movement

    public override SteeringOutput getSteering(Kinematic character, Kinematic target, RoomManager manager)
    {
        SteeringOutput result = new SteeringOutput();

        if (target == null) return result; // No target, no movement

        // Calculate direction and distance to hero
        Vector3 direction = target.position - character.position;
        float distance = direction.magnitude;
        float speed = character.velocity.magnitude;

        // Predict future position of the hero
        float prediction;
        if (speed <= distance / maxPredictionTime)
            prediction = maxPredictionTime;
        else
            prediction = distance / speed;

        Vector3 predictedTarget = target.position + target.velocity * prediction;

        // Use Seek to move towards predicted position
        result.linear = (predictedTarget - character.position).normalized * maxAcceleration;

        return result;
    }
}
