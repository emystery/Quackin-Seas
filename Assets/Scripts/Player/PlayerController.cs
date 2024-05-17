using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum MovementState
{
    Walking,
    Swimming,
    Wading,
    Air,
    Climbing,
    Sailing,
    Flying
}

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform orientation;
    [SerializeField] private GameObject ship;
    [SerializeField] private CapsuleCollider playerCollider;
    [SerializeField] private GameObject waterCollider;
    [SerializeField] public GameObject underWaterShader;
    [SerializeField] public GameObject[] waterGrid;
    [SerializeField] private Camera mainCamera;

    [Space(20)]
    [Header("Movement Values")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float sprintMultiplier;
    [SerializeField] private float swimSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float groundDrag;
    [SerializeField] private float playerHeight;
    [SerializeField] private float gravityScale;
    [SerializeField] private float currentGravity;

    [Space(20)]
    [Header("Layer Masks")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask waterLayer;
    [SerializeField] public LayerMask shipMask;

    [Space(20)]
    [Header("Checks")]
    public MovementState movementState;

    public bool onShip = false;

    bool canJump = true;
    [SerializeField] private bool grounded;
    [SerializeField] private bool inWater;
    [SerializeField] private bool ascending;
    [SerializeField] private bool sprinting;
    [SerializeField] private bool onSlope;
    [SerializeField] public bool isClimbing;
    [SerializeField] private bool belowDeck;
    [SerializeField] private bool immortal;

    [SerializeField] public bool onHelm;

    private bool exitingSlope;

    private float horizontalInput;
    private float verticalInput;
    private RaycastHit slopeHit;

    [Space(20)]
    [Header("Special Input Keys")]
    [SerializeField] public KeyCode jumpKey;
    [SerializeField] public KeyCode sprintKey;

    private Vector3 moveDirection;

    [HideInInspector] public Rigidbody rb;


    void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        rb.useGravity = false;
    }

    void FixedUpdate()
    {
        MovePlayer();
    }

    void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayer);
        onSlope = OnSlope();
        sprinting = Input.GetKey(sprintKey);
        ascending = Input.GetKey(jumpKey);

        currentGravity = onSlope ? 0 : gravityScale;

        if (onShip && !onHelm)
        {
            inWater = false;
            transform.parent = ship.transform;
        }
        else
        {
            if (!onHelm)
            {
                transform.parent = null;
            }
        }
        if (movementState != MovementState.Flying)
        {
            if (inWater)
            {
                if (grounded)
                {
                    movementState = MovementState.Wading;
                }
                else
                {
                    movementState = MovementState.Swimming;
                }
            }
            else if (grounded)
            {
                movementState = MovementState.Walking;
            }
            else
            {
                movementState = MovementState.Air;
            }
            if (isClimbing)
            {
                movementState = MovementState.Climbing;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    isClimbing = false;
                }
            }
            if (onHelm)
            {
                movementState = MovementState.Sailing;

            }
        }

        if (!underWaterShader.activeSelf && inWater)
        {
            underWaterShader.SetActive(true);
        }
        else if (underWaterShader.activeSelf && !inWater)
        {
            underWaterShader.SetActive(false);
        }

        if (onShip)
        {
            waterCollider.SetActive(false);
            if (belowDeck)
            {
                for (int i = 0; i < waterGrid.Length; i++)
                {
                    waterGrid[i].GetComponent<MeshRenderer>().enabled = false;
                }
            }
            else
            {
                for (int i = 0; i < waterGrid.Length; i++)
                {
                    waterGrid[i].GetComponent<MeshRenderer>().enabled = true;
                }
            }
        }
        else
        {
            waterCollider.SetActive(true);

        }
        Cheats();
        PlayerInput();
        SpeedControl();
    }

    private void Cheats()
    {
        if (Input.GetKey(KeyCode.RightShift) && Input.GetKeyDown(KeyCode.Alpha0)) 
        {
            GameManager.self.unlockTreasures = true;           
        }

        if (Input.GetKey(KeyCode.RightShift) && Input.GetKeyDown(KeyCode.Alpha9))
        {
            if (!immortal)
            {
                immortal = true;
            }
            else
            {
                immortal = false;
            }
        }

        if (Input.GetKey(KeyCode.RightShift) && Input.GetKeyDown(KeyCode.Alpha8))
        {
            movementState = MovementState.Flying;
        }
    }
    private void MovePlayer()
    {
        float movementMultiplier = 1;
        if (sprinting)
        {
            movementMultiplier = sprintMultiplier;
        }

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        Vector3 gravity = currentGravity * Vector3.up;

        if (onSlope)
        {
            moveDirection = GetSlopeMoveDirection();
            //if (rb.velocity.y > 0)
            //{
            //    rb.AddForce(Vector3.down * 80, ForceMode.Force);
            //}
        }

        switch (movementState)
        {
            case MovementState.Walking:
                rb.drag = groundDrag;
                rb.AddForce(moveDirection * moveSpeed * movementMultiplier * 10, ForceMode.Force);
                break;

            case MovementState.Swimming:
                if (ascending)
                {
                    Vector3 up = swimSpeed * 10 * Vector3.up;
                    rb.AddForce(up, ForceMode.Acceleration);
                }
                gravity *= 0.25f;
                rb.drag = groundDrag * 2;
                rb.AddForce(moveDirection * swimSpeed * movementMultiplier * 0.8f * 10, ForceMode.Force);
                break;

            case MovementState.Wading:
                if (ascending)
                {
                    Vector3 up = swimSpeed * Vector3.up;
                    rb.AddForce(up, ForceMode.Acceleration);
                }
                gravity *= 0.75f;
                rb.drag = groundDrag * 2.5f;
                rb.AddForce(moveDirection * moveSpeed * movementMultiplier * 0.5f * 10, ForceMode.Force);
                break;

            case MovementState.Air:
                rb.drag = 0;
                rb.AddForce(moveDirection * moveSpeed * 10 * .4f, ForceMode.Force);
                break;

            case MovementState.Climbing:
                gravity *= 0;

                float climbSpeed = 500f;
                verticalInput = Input.GetAxisRaw("Vertical");
                Vector3 climbMovement = orientation.up * verticalInput * climbSpeed;
                rb.AddForce(climbMovement * Time.fixedDeltaTime);
                break;
            case MovementState.Sailing:

                break;

            case MovementState.Flying:
                gravity *= 0;
                moveDirection = mainCamera.transform.forward * verticalInput + mainCamera.transform.right * horizontalInput;
                if (ascending)
                {
                    Vector3 up = swimSpeed * 10 * Vector3.up;
                    rb.AddForce(up, ForceMode.Acceleration);
                }
                rb.drag = groundDrag;
                rb.AddForce(moveDirection * swimSpeed * movementMultiplier * 50, ForceMode.Force);


                break;
        }

        rb.AddForce(gravity, ForceMode.Acceleration);
    }

    void PlayerInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && canJump && movementState != MovementState.Sailing)
        {
            Jump();
        }
    }

    void SpeedControl()
    {
        float movementMultiplier = 1;
        if (sprinting)
        {
            movementMultiplier = sprintMultiplier;
        }

        if (onSlope && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed * movementMultiplier)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed * movementMultiplier;
            }
        }
        else
        {
            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            if (flatVelocity.magnitude > moveSpeed * movementMultiplier)
            {
                Vector3 limitedVel = flatVelocity.normalized * moveSpeed * movementMultiplier;

                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    void Jump()
    {
        if (!grounded && !inWater)
            return;

        else if (grounded)
        {
            grounded = false;
            canJump = false;
            exitingSlope = true;

            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

            Invoke(nameof(ResetJump), 0.5f);
        }
    }

    void ResetJump()
    {
        canJump = true;
        exitingSlope = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (shipMask.Contains(collision.gameObject.layer))
        {
            onShip = true;
        }
        else
        {
            onShip = false;

            if (collision.gameObject.CompareTag("Shark") && !immortal)
            {
                Debug.Log("You are dead!");
                GameManager.self.state = GameState.Dead;
                Time.timeScale = 0f;
            }

            if (collision.gameObject.CompareTag("Gorrila") && !immortal)
            {
                Debug.Log("You are dead!");
                GameManager.self.state = GameState.Dead;
                Time.timeScale = 0f;
            }

            if (collision.gameObject.CompareTag("Crocodile") && !immortal)
            {
                Debug.Log("You are dead!");
                GameManager.self.state = GameState.Dead;
                Time.timeScale = 0f;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (shipMask.Contains(collision.gameObject.layer))
        {
            onShip = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (waterLayer.Contains(other.gameObject.layer))
        {
            inWater = true;
        }

        if (other.gameObject.CompareTag("HideWater") && !belowDeck)
        {
            //disable water shader
            belowDeck = true;
        }
        else if (other.gameObject.CompareTag("HideWater") && belowDeck)
        {
            //enable water shader
            belowDeck = false;
        }


        //add conditions - raycast - interactions 
        if (other.gameObject.CompareTag("Ladder"))
        {
            isClimbing = true;
        }

        if (other.gameObject.CompareTag("ShipLadder"))
        {
            onShip = true;
            isClimbing = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (waterLayer.Contains(other.gameObject.layer))
        {
            inWater = false;
        }

        if (other.gameObject.CompareTag("Ladder"))
        {
            isClimbing = false;
        }

        if (other.gameObject.CompareTag("ShipLadder")) 
        {
            isClimbing = false;
        }
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f, groundLayer))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle > 10 && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    public void TeleportToRudder()
    {
        GameObject rudder = GameObject.FindGameObjectWithTag("Helm");
        if (rudder != null)
        {
            transform.position = rudder.transform.position;
            GameManager.self.state = GameState.Playing;
            Time.timeScale = 1.0f;
        }
        else
        {
            Debug.LogError("Rudder not found!");
        }
    }
}

public static class UnityExtensions // please ignore
{

    /// <summary>
    /// Extension method to check if a layer is in a layermask
    /// </summary>
    /// <param name="mask"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static bool Contains(this LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }
}
