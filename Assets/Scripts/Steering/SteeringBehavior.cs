using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SteeringBehavior : ScriptableObject
{
    // maximum linear speed of the character
    public float maxSpeed = 1.25f;
    
    // // the game object that we are targeting
    // public GameObject targetCharacter;
    //
    // animation controller of this character
    protected Animator animController;
    
    //
    // // kinematics for this character
    // public Kinematic character;

    public abstract SteeringOutput getSteering(Kinematic character, Kinematic target);
    //TODO: get the list of targets from inside the behavior itself, i like this idea better
    
    // public abstract SteeringOutput getSteering(Kinematic character, Kinematic target, float maxSpeed);

    // public SteeringOutput getSteering(GameObject character, GameObject target, float maxSpeed)
    // {
    //     return getSteering(new Kinematic(character), new Kinematic(target), maxSpeed);
    // }
    //
    // public SteeringOutput getSteering(Kinematic character, GameObject target, float maxSpeed)
    // {
    //     return getSteering(character, new Kinematic(target), maxSpeed);
    // }
}
