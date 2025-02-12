using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoidance : Seek
{
    public float avoidDistance;

    public float lookahead;

    public int obstacleLayer;
    public override SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();
        Ray ray = new Ray(character.position, character.velocity.normalized);
        bool collisionDetected = Physics.Raycast(character.position, character.velocity.normalized, out RaycastHit hit, lookahead, 1 << obstacleLayer);
        if (!collisionDetected)
        {
            return result;
        }

        target.position = hit.point + hit.normal * avoidDistance;
        return base.getSteering();
    }
}
