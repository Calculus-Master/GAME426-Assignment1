using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Steering/ObstacleAvoidance")]
public class ObstacleAvoidance : Seek
{
    public float avoidDistance = 1.5f;

    public float lookahead = 1f;
    public float height = 0.5f;

    public int obstacleLayer;
    public int numWhiskers = 3;
    public float fov = 60;
    public override SteeringOutput getSteering(Kinematic character, Kinematic target)
    {
        //TODO: triple ray configuration for shorter whiskers
        SteeringOutput result = new SteeringOutput();
        // Debug.Log($"Collision detected with {hit.transform.gameObject.name} and {character.targetObj.name}.");

        Vector3 rayOrigin = character.position;
        rayOrigin.y += height;
        List<RaycastHit> collisions = new List<RaycastHit>(numWhiskers);
        // Debug.DrawRay(headPosition, Quaternion.Euler(0,-fov/2, 0) * transform.forward* maxDistance, Color.green);
        float rotationPerWhisker = fov / numWhiskers;
        float startRotation = -fov / 2;
        for (int i = 0; i < numWhiskers; i++)
        {
            float currentRotation = startRotation + rotationPerWhisker * i;
            Ray ray = new Ray(rayOrigin, Quaternion.Euler(0, currentRotation, 0) * character.velocity.normalized);
            bool collisionDetected = Physics.Raycast(ray, out RaycastHit hit, lookahead, 1 << obstacleLayer);
            Debug.DrawRay(ray.origin, ray.direction * lookahead, Color.red);

            if (!collisionDetected)
            {
                continue;
            }

            collisions.Add(hit);
        }

        if (collisions.Count == 0)
        {
            return result;
        }

        Vector3 collisionNormal = Vector3.zero;
        Vector3 avoidPoint = Vector3.zero;
        for (int i = 0; i < collisions.Count; i++)
        {
            avoidPoint += collisions[i].point;
            collisionNormal += collisions[i].normal;
        }

        collisionNormal /= collisions.Count;
        avoidPoint /= collisions.Count;
        avoidPoint.y = character.position.y;
        target.position =  avoidPoint + collisionNormal * avoidDistance;
        Vector3 debugPoint = avoidPoint;
        debugPoint.y = height + character.position.y;
        Debug.DrawRay(debugPoint, (target.position - avoidPoint).normalized * avoidDistance, Color.green);
        return base.getSteering(character, target);
    }
}