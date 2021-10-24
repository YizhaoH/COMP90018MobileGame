using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Cohesion")]
public class CohensionBehaviour : FilteredFlockBehaviour
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
        Vector3 cohensionMove = Vector3.zero;
        List<Transform> filteredContext = (filter==null)?context : filter.Filter(agent, context);
        if(filteredContext.Count ==0) return Vector3.zero;

        foreach (Transform item in filteredContext)
        {
            cohensionMove += item.position;
        }
        cohensionMove /= filteredContext.Count; // take average

        //create offset from agent position
        cohensionMove -= agent.transform.position;
        cohensionMove = Vector3.SmoothDamp(agent.transform.forward, cohensionMove, ref currVelocity, agentSmoothTime);
        return cohensionMove;
    }



}
