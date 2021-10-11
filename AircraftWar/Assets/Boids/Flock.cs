using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{

    public FlockAgent agentPrefab;
    List<FlockAgent> agents = new List<FlockAgent>();
    public FlockBehaviour behaviour;

    [Range(10,50)]public int startingCount = 30;
    public float AgentDensity = 0.1f;

    [Range(1f, 100f)]public float driveFactor = 10f;
    [Range(1f, 100f)]public float maxSpeed = 10f;
    [Range(1f, 50f)]public float neighborRadius = 5f;
    [Range(0f, 1f)]public float avoidanceRadiusMultiplier = 0.5f;
    [Range(10f, 100f)] public float playerDetectionRadius = 50f;
    [Range(0f, 180f)] public float playerDetectionDegree = 90f;
    float squareMaxSpeed;
    float squareNeighborRadius;
    float squareAvoidanceRadius;
    public float SquareAvoidanceRadius{ get{ return squareAvoidanceRadius; } }
    float sqaurePlayerDetectionRadius;
    public float SqaurePlayerDetectionRadius{ get { return sqaurePlayerDetectionRadius; } }

    // Start is called before the first frame update
    void Start()
    {
        squareMaxSpeed = maxSpeed * maxSpeed;
        squareNeighborRadius = neighborRadius * neighborRadius;
        squareAvoidanceRadius = squareNeighborRadius * avoidanceRadiusMultiplier * avoidanceRadiusMultiplier;
        sqaurePlayerDetectionRadius = playerDetectionRadius * playerDetectionRadius;


        for(int i=0; i<startingCount; i++)
        {
            Vector3 spawnPos = Random.insideUnitSphere * startingCount * AgentDensity + this.transform.position;
            if (spawnPos.y<=0) spawnPos.y = 300;
            
            // Bit shift the index of the layer (8) to get a bit mask
            int layerMask = 1 << 8;

            // This would cast rays only against colliders in layer 8.
            // But instead we want to collide against everything except layer 8. The ~ operator does this, it inverts a bitmask.
            layerMask = ~layerMask;

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            int maxDistance = 500;
            if (Physics.Raycast(spawnPos, transform.TransformDirection(Vector3.up), out hit, maxDistance, layerMask))
            {
                Debug.DrawRay(spawnPos, transform.TransformDirection(Vector3.up) * hit.distance, Color.yellow, 30, false);
                Debug.Log("Did Hit");
                if( hit.collider.CompareTag("Terrain"))
                {
                    float disToTerrain = hit.distance;
                    spawnPos.y += disToTerrain + 50f;
                }
            }
            

            FlockAgent newAgent = Instantiate(
                agentPrefab,
                spawnPos,
                //Quaternion.Euler(Vector3.forward * Random.Range(-90f,90f)),
                Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)),
                transform
            );
            newAgent.name = "Agent "+i;
            newAgent.Initialize(this);
            agents.Add(newAgent);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (FlockAgent agent in agents)
        {
            List<Transform> context = GetNearbyObject(agent);

            Vector3 move = behaviour.CalculateMove(agent, context, this);
            move *= driveFactor;
            if (move.sqrMagnitude > squareMaxSpeed)
            {
                move = move.normalized * maxSpeed;
            }
            agent.Move(move);

        }
    }

    List<Transform> GetNearbyObject(FlockAgent agent)
    {
        List<Transform> context = new List<Transform>();

        Collider[] contextColliders = Physics.OverlapSphere(agent.transform.position, neighborRadius);
        foreach(Collider c in contextColliders)
        {
            if (c != agent.AgentCollider) context.Add(c.transform);
        }

        return context;
    }
}
