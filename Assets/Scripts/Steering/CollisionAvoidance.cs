using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Steering/CollisionAvoidance")]
public class CollisionAvoidance : Evade
{
    public float maxAcceleration;
    [FormerlySerializedAs("radius")] public float RepulseThreshold;

    public override SteeringOutput getSteering(Kinematic character, Kinematic t, RoomManager manager)
    {
        float shortestTime = Mathf.Infinity;
        Kinematic firstTarget = null;
        float firstMinSeparation = Mathf.Infinity;
        float firstDistance = Mathf.Infinity;
        Vector3 firstRelativePos = Vector3.zero;
        Vector3 firstRelativeVel = Vector3.zero;

        Vector3 relativePos;
        List<Kinematic> targets = manager.GetEnemies();
        
        foreach (var target in targets)
        {
            if (target == character)
            {
                continue;
            }
            //TODO: calculate intersection instead because something wrong
            
            relativePos = target.position - character.position;
            // var relativeVel = target.velocity - character.velocity;
            var relativeVel = character.velocity - target.velocity;
            var relativeSpeed = relativeVel.magnitude;
            var timeToCollision = Vector3.Dot(relativePos, relativeVel) / (relativeSpeed * relativeSpeed);
            var distance = relativePos.magnitude;
            var minSeparation = distance - relativeSpeed * timeToCollision;
            // Debug.Log($"{target.targetObj.name}. pos: {target.position}. vel: {target.velocity}. Dot:{Vector3.Dot(relativePos, relativeVel)}. relativeVel: {relativeVel}. relativePos: {relativePos}. time: {timeToCollision}. minSeparation: {minSeparation}. 2*rt: {2*RepulseThreshold}. shortestTime: {shortestTime}");

            if (minSeparation > 2 * RepulseThreshold)
            {
                continue;
            }
            
            if (timeToCollision > 0 && timeToCollision < shortestTime)
            {

                shortestTime = timeToCollision;
                firstTarget = target;
                firstMinSeparation = minSeparation;
                firstDistance = distance;
                firstRelativePos = relativePos;
                firstRelativeVel = relativeVel;
            }
        }
        
        
        if (firstTarget == null)
        {
            return new SteeringOutput();
        }

        if (firstMinSeparation <= 0 || firstDistance < 2 * RepulseThreshold)
        {
            relativePos = firstTarget.position - character.position;
        }
        else
        {
            relativePos = firstRelativePos + firstRelativeVel * shortestTime;
        }

        relativePos = -relativePos.normalized;

        SteeringOutput result = new SteeringOutput();
        result.linear = maxAcceleration * relativePos;
        result.angular = 0f;
        return result;
    }
}
