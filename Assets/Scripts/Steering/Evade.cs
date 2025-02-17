using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Steering/Evade")]
public class Evade : Flee
{
    public float maxPredictionTime = 1.5f; // Max time to predict hero's movement

    public override SteeringOutput getSteering(Kinematic character, Kinematic target, RoomManager manager)
    {
        SteeringOutput result = new SteeringOutput();

        if (target == null) return result; // No target, no movement

        // Calculate direction and distance from hero
        Vector3 direction = character.position - target.position;
        float distance = direction.magnitude;
        float speed = character.velocity.magnitude;

        // Predict future position of the hero
        float prediction;
        if (speed <= distance / maxPredictionTime)
            prediction = maxPredictionTime;
        else
            prediction = distance / speed;

        Vector3 predictedTarget = target.position + target.velocity * prediction;

        // Use Flee to move away from the predicted position
        result.linear = (character.position - predictedTarget).normalized * maxAcceleration;

        return result;
    }
}
