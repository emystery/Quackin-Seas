using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float mainCameraSensitivity;

    private float xRotation;
    private float yRotation;

    public Transform playerOrientation;

    [SerializeField] private PlayerController playerController;

    public GameObject ship;
    public Transform frontOfTheShip;
    private Camera mainCamera;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        mainCamera = Camera.main;
    }

    void Update()
    {

        if (GameManager.self.state == GameState.Paused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            if (!playerController.onHelm)
            {
                float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * mainCameraSensitivity;
                float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * mainCameraSensitivity;

                yRotation += mouseX;
                xRotation -= mouseY;
                xRotation = Mathf.Clamp(xRotation, -90, 90);

                transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

                if (playerController.movementState == MovementState.Swimming)
                {
                    playerOrientation.rotation = Quaternion.Euler(xRotation, yRotation, 0);
                }
                else
                {
                    playerOrientation.rotation = Quaternion.Euler(0, yRotation, 0);
                }
            }

            else
            {
                Vector3 shipForward = (frontOfTheShip.position - transform.position).normalized;

                shipForward.y = 0f;
                Quaternion mainCameraRotation = Quaternion.LookRotation(shipForward);

                mainCamera.transform.rotation = Quaternion.Slerp(mainCamera.transform.rotation, mainCameraRotation, Time.deltaTime * 5);
                playerOrientation.rotation = Quaternion.Slerp(mainCamera.transform.rotation, mainCameraRotation, Time.deltaTime * 5);

                yRotation = playerOrientation.rotation.eulerAngles.y;
                xRotation = 0;
            }
        }
    }
}  
