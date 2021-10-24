using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Avoidance")]
public class AvoidanceBehaviour : FlockBehaviour
{
    Vector3 currVelocity;
    public float agentSmoothTime = 0.5f;

    //find the centre of all neighbors and try to move there
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        if(agent.isDead) return Vector3.zero;
        
        //if no neighbors
        if(context.Count ==0) return Vector3.zero;

        //add all points together and average
        Vector3 avoidanceMove = Vector3.zero;
        int nAvoid = 0;
        foreach (Transform item in context)
        {
            Vector3 dis = item.position - agent.transform.position;
            if (dis.sqrMagnitude < flock.SquareAvoidanceRadius)
            {
                nAvoid ++;
                avoidanceMove += agent.transform.position - item.position;
            }
        }
        if((900-agent.transform.position.y) <100)
        {
            avoidanceMove += Vector3.down;
        }
        if (nAvoid > 0)
            avoidanceMove /= nAvoid;
        //avoidanceMove = Vector3.SmoothDamp(avoidanceMove.normalized, avoidanceMove, ref currVelocity, agentSmoothTime);
        return avoidanceMove;
    }
}
