using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Attack")]
public class AttackBehaviour : FlockBehaviour
{

    Vector3 currVelocity;
    public float agentSmoothTime = 0.3f;
    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        if(agent.isDead) return Vector3.zero;

        //add all points together and average
        Vector3 attackMove = Vector3.zero;
        float dis = agent.DistanceToTarget(flock.target);
        float angle = agent.AngleWithPlayer(flock.target);
        if( dis>=flock.playerStopDetectionRadius && 
            dis<flock.playerDetectionRadius &&
            angle<=flock.playerDetectionDegree )
        {
            attackMove = flock.target.transform.position - agent.transform.position;
            //Debug.DrawRay(agent.transform.position, attackMove, Color.green, 0.5f);
        }

        //if no neighbors and no player
        if (context.Count == 0 && attackMove == Vector3.zero)
        {
            if(agent.gun != null)
                agent.gun.attack = false;
            else
                agent.missleGun.attack = false;
            return Vector3.zero;
        }

        if(Random.Range(0,5000) < 30)
        {
            attackMove = Vector3.zero;
        }

        if( attackMove != Vector3.zero)
        {
            //attackMove = Vector3.SmoothDamp(agent.transform.forward, attackMove, ref currVelocity, agentSmoothTime);
            if(agent.gun != null && angle<10)
                agent.gun.attack = true;
            if(agent.missleGun != null && angle<35)
                agent.missleGun.attack = true;
            
        }
            
        return attackMove;
    }
}
