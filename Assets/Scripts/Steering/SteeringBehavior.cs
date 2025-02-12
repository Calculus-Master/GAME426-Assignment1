using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SteeringBehavior
{
    // maximum linear speed of the character
    public float maxSpeed = 1.25f;

    // the game object that we are targeting
    public GameObject targetCharacter;

    // animation controller of this character
    protected Animator animController;

    // kinematics for this character
    protected Kinematic character;

    public abstract SteeringOutput getSteering();
}
