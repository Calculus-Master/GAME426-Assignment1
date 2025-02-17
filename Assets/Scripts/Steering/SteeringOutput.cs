using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Definition of dynamic steering object
public class SteeringOutput
{
    // acceleration of character
    public Vector3 linear;
    public float angular;

    // class constructor
    public SteeringOutput()
    {
        linear = Vector3.zero;
    }
}
