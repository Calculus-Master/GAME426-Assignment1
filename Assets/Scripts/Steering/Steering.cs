using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Steering : MonoBehaviour
{
    // maximum linear speed of the character
    public float maxSpeed = 1.25f;

    // the game object that we are targeting
    public GameObject targetCharacter;

    public SteeringBehavior myBehavior;
    
    // animation controller of this character
    protected Animator animController;

    // kinematics for this character
    protected Kinematic character;
    protected Kinematic target;

    private void Awake()
    {
        character = new Kinematic(gameObject);
        character.maxSpeed = maxSpeed;
        // myBehavior.character = character;
        target = new Kinematic(targetCharacter);
    }

    private void FixedUpdate()
    {
        // It is best practice to do "physics" calculations
        // at a fixed and standardized framerate (e.g. 50 Hz)
        transform.position = character.position;
        transform.rotation = Quaternion.Euler(0.0f, character.orientation * Mathf.Rad2Deg, 0.0f);

        SteeringOutput result = myBehavior.getSteering(character, target);
        character.EulerIntegration(result, Time.deltaTime);
    }

    private void LateUpdate()
    {
        target.SetTarget(targetCharacter);
    }
}
