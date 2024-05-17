using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GorrilaController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public bool InsideWaterBounds = true;
    public float detectionRadius = 10.0f;
    public float islandRadius = 15f;
    public NavMeshAgent agent;
    public float wanderRange = 10f;
    public Transform centrePoint;
    private Transform playerTransform;
    private Transform islandCenter;
    private PlayerController playerController;


    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        playerController = FindObjectOfType<PlayerController>();
        islandCenter = GameObject.FindGameObjectWithTag("IslandCenter").transform;
    }

    void Update()
    {
        float distanceToCenter = Vector3.Distance(transform.position, islandCenter.position);
        if (!InsideWaterBounds)
        {
            if (IsPlayerInRange() && distanceToCenter <= islandRadius)
            {
                ChasePlayer();
            }
            else
            {
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                {
                    
                   SetRandomDestination();
                   
                }
            }
        }
    }

    void ChasePlayer()
    {
        if (playerTransform != null && !playerController.isClimbing)
        {
            agent.SetDestination(playerTransform.position);
        }
    }

    bool IsPlayerInRange()
    {
        if (playerTransform != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
            return distanceToPlayer <= detectionRadius;
        }
        return false;
    }

    void SetRandomDestination()
    {
        Vector3 randomPoint = centrePoint.position + Random.insideUnitSphere * wanderRange;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, wanderRange, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}
