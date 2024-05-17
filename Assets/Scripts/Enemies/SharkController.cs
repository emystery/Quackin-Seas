using UnityEngine;
using UnityEngine.UI;

public class SharkController : MonoBehaviour
{
    public Transform[] waypoints;
    public float moveSpeed = 2f;

    private int currentWaypointIndex = 0;

    public bool IsChasingPlayer;
    public bool InsideWaterBounds;
    public bool InsideSafeSpot;
    public float detectionRadius = 10.0f;

    void Start()
    {
        transform.position = waypoints[currentWaypointIndex].position;
        InsideWaterBounds = true;
        InsideSafeSpot = false;
    }

    void Update()
    {
        ChasePlayer();

        if (!IsChasingPlayer)
        {
            MoveToWaypoint();
        }
    }

    void MoveToWaypoint()
    {
        if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) <= 0.1f)
        {
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
            InsideWaterBounds = true;
            InsideSafeSpot = false;
        }

        transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, moveSpeed * Time.deltaTime);

        RotateTowardsWaypoint();
    }

    void ChasePlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null && InsideWaterBounds && !InsideSafeSpot)
        {
            Transform playerTransform = player.transform;

            float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);

            if (distanceToPlayer <= detectionRadius)
            {
                Vector3 directionToPlayer = playerTransform.position - transform.position;
                directionToPlayer.y = 0f;

                IsChasingPlayer = true;

                moveSpeed = 5f;

                if (directionToPlayer != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
                }

                transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);
            }
            else
            {
                moveSpeed = 2f;
                IsChasingPlayer = false;
                MoveToWaypoint();
            }
        }
        else
        {
            moveSpeed = 2f;
            IsChasingPlayer = false;
            MoveToWaypoint();
        }
    }

    void OnCollisionEnter(Collision collision)
    {   
        if (collision.gameObject.CompareTag("SafeSpot"))
        {
            Debug.Log("SafeSpot");
            InsideSafeSpot = true;
            IsChasingPlayer = false;
            MoveToWaypoint();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("WaterTrigger"))
        {
            Debug.Log("Trigger");
            InsideWaterBounds = false;
            IsChasingPlayer = false;
            MoveToWaypoint();
        }
    }

    void RotateTowardsWaypoint()
    {
        Vector3 direction = waypoints[currentWaypointIndex].position - transform.position;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }
}

