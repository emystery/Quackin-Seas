using UnityEngine;

public class ShipController : MonoBehaviour
{
    public enum ShipState
    {
        Anchored = 0,
        HalfSail = 1,
        FullSail = 2
    }

    public PlayerController playerController;

    public GameObject fullSail;
    public GameObject halfSail;
    public GameObject anchorDown;
    public GameObject shipModel;

    public Transform frontOfTheShip;

    public GameObject helmSpot;

    [SerializeField] ShipState currentState = ShipState.Anchored;
    private Rigidbody rb;

    [SerializeField] private float FullSailSpeed = 5f;
    [SerializeField] private float HalfSailSpeed = 2f;
    private float currentSpeed;

    private void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
    }

    void FixedUpdate()
    {
        transform.position = rb.transform.position;
        rb.transform.localPosition = Vector3.zero;
        Vector3 rotation = rb.transform.rotation.eulerAngles;
        rotation.y = shipModel.transform.rotation.eulerAngles.y;
        rb.transform.rotation = Quaternion.Euler(rotation);
        
        if (playerController.movementState != MovementState.Sailing)
        {
            rb.velocity = Vector3.zero;
            return;
        }
        Vector3 direction = (frontOfTheShip.position - transform.position).normalized;

        //transform.position += direction * currentSpeed * Time.deltaTime;
        rb.velocity = direction * currentSpeed * 5;

        switch (currentState)
        {
            case ShipState.Anchored:
                currentSpeed = 0;
                break;
            case ShipState.HalfSail:
                currentSpeed = HalfSailSpeed;
                break;
            case ShipState.FullSail:
                currentSpeed = FullSailSpeed;
                break;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            FullSail();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            HalfSail();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Anchoring();
        }

        //change to only happen when player on helm
    }

    public void FullSail()
    {
        currentState = ShipState.FullSail;
        fullSail.SetActive(true);
        halfSail.SetActive(false);
        anchorDown.SetActive(false);
    }

    public void HalfSail()
    {
        currentState = ShipState.HalfSail;
        fullSail.SetActive(false);
        halfSail.SetActive(true);
        anchorDown.SetActive(false);
    }

    public void Anchoring()
    {
        currentState = ShipState.Anchored;
        fullSail.SetActive(false);
        halfSail.SetActive(false);
        anchorDown.SetActive(true);
    }
}
