using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Flock/Behaviour/Stay in Radius")]
public class StayInRadiusBehaviour : FlockBehaviour
{
    public Vector3 center;
    public float radius = 750;

    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {

        if(agent.isDead) return Vector3.zero;
        
        Vector3 centerOffset = center - agent.transform.position;

        float t = centerOffset.magnitude / radius;

        if( t<0.9f)
        {
            return Vector3.zero;
        }
        return centerOffset * t * t;
    }
}
