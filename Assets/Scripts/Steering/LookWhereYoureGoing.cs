using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Steering/LookWhereYoureGoing")]
public class LookWhereYoureGoing : Align
{
   public override SteeringOutput getSteering(Kinematic character, Kinematic target)
   {
      Vector3 velocity = character.velocity;
      if (Mathf.Approximately(velocity.magnitude, 0))
      {
         return new SteeringOutput();
      }

      target = new Kinematic(target.targetObj);
      target.orientation = Mathf.Atan2(velocity.x, velocity.z);
      return base.getSteering(character, target);
   }
}
