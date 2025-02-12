using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Definition of Kinematic object
public class Kinematic
{
    // limit on velocity
    public float maxSpeed;

    // 3D pose of character
    public Vector3 position;
    public float orientation;

    // velocity of character
    public Vector3 velocity;
    public float rotation;

    // scene object to which
    // this refers (if any)
    private GameObject targetObj;

    // class constructor
    public Kinematic(GameObject targetObj)
    {
        SetTarget(targetObj);

        // assume initial velocities (linear and angular) are zero
        velocity = Vector3.zero;
        rotation = 0.0f;
    }
    public void SetTarget(GameObject targetObj)
    {
        this.targetObj = targetObj;

        if (targetObj != null)
        {
            position = targetObj.transform.position;
            orientation = targetObj.transform.rotation.eulerAngles.y * Mathf.Deg2Rad;
        }
        else
        {
            position = Vector3.zero;
            orientation = 0.0f;
        }
    }

    // Note: This is a simple, non-force based implementation that follows
    // the example pseudocode from lecture as closely as possible. A better
    // implementation would use forces so it does not conflict with Unity's
    // physics system.
    public void EulerIntegration(SteeringOutput steering, float time)
    {
        // update the positoin and orientation
        position += velocity * time;
        orientation += rotation * time;

        // and the velocity and rotation.
        if (steering != null)
        {
            velocity += steering.linear * time;
            rotation += steering.angular * time;
        }

        // check for speeding and clip
        if (velocity.magnitude > maxSpeed)
        {
            velocity.Normalize();
            velocity *= maxSpeed;
        }
    }
}