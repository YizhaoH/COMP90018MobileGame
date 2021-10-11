using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Attack")]
public class AttackBehaviour : FlockBehaviour
{
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        //if no neighbors
        if(context.Count ==0) return Vector3.zero;

        //add all points together and average
        Vector3 attackMove = Vector3.zero;
        foreach (Transform item in context)
        {
            if (item.tag != "Player") 
            {
                continue;
            }
            else
            {
                Vector3 dis = item.position - agent.transform.position;
                float angle = Mathf.Abs(Vector3.Angle(dis, agent.transform.forward));
                // player in radius && player in sight angle degree
                if (dis.sqrMagnitude < flock.SquareAvoidanceRadius && angle<flock.playerDetectionDegree)
                {
                    attackMove = dis;
                }
            }
        }
        return attackMove;
    }
}
