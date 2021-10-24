using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Flock/Behaviour/Composite")]
public class CompositeBehaviour : FlockBehaviour
{

    public FlockBehaviour[] behaviours;
    public float[] weights;


    public override Vector3 CalculateMove(FlockAgent agent, List<Transform> context, Flock flock)
    {
        if(agent.isDead) return Vector3.zero;
        
        if (weights.Length != behaviours.Length)
        {
            Debug.LogError("Data mismatch in "+ name ,this);
        }

        //set up move
        Vector3 move = Vector3.zero;

        //iter through behaviours
        for(int i=0; i<behaviours.Length; i++)
        {
            Vector3 partialMove = behaviours[i].CalculateMove(agent, context, flock) * weights[i];

            if(partialMove != Vector3.zero)
            {
                //if exceed the weight, then normalize
                if(partialMove.sqrMagnitude > weights[i]*weights[i])
                {
                    partialMove.Normalize();
                    partialMove *= weights[i];
                }

                move += partialMove;
            }
        }

        return move;
    }
}
