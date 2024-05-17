using UnityEngine;

public class Rotation : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    [SerializeField] private float rotationSpeed = 100.0f;

    private Animator helmAnimator;

    private Quaternion targetRotation;
    
    void Start()
    {
        targetRotation = transform.rotation;
        helmAnimator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.onHelm)
        {
            helmAnimator.SetInteger("Turning", 0);
            if (Input.GetKey(KeyCode.A))
            {
                targetRotation *= Quaternion.Euler(0, -rotationSpeed * Time.deltaTime, 0);
                helmAnimator.SetInteger("Turning", -1);
            }

            if (Input.GetKey(KeyCode.D))
            {
                targetRotation *= Quaternion.Euler(0, rotationSpeed * Time.deltaTime, 0);
                helmAnimator.SetInteger("Turning", 1);
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5);
        }
    }
}
