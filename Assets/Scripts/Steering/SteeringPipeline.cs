using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteeringPipeline : SteeringBehavior
{
    public Targeter[] targeters;
    public Decomposer[] decomposers;
    public Constraint[] constraints;
    public Actuator actuator;

    public int constraintSteps;

    public SteeringBehavior deadlock;
    
    public override SteeringOutput getSteering(Kinematic character, Kinematic target, RoomManager manager)
    {
        Goal goal = new Goal();
        foreach (var targeter in targeters)
        {
            var targeterGoal = targeter.getGoal(character);
            goal.updateChannels(targeterGoal);
        }

        foreach (var decomposer in decomposers)
        {
            goal = decomposer.decompose(character, goal);
        }

        for (int i = 0; i < constraintSteps; i++)
        {
            var path = actuator.getPath(character, goal);
            bool valid = true;
            foreach (var constraint in constraints)
            {
                if (constraint.isViolated(path))
                {
                    goal = constraint.suggest(character, path, goal);
                    valid = false;
                    break;
                }
            }

            if (valid)
            {
                return actuator.output(character, path, goal);
            }
        }

        return deadlock.getSteering(character, target, manager);
    }
}
