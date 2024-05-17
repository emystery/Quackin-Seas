using TMPro;
using UnityEngine;
using UnityEngine.UI;

interface IInteractable
{
    public void Interact();
}

public class Interactor : MonoBehaviour
{

    [SerializeField] private Transform InteractorSource;
    [SerializeField] private float InteractRange;


    [SerializeField] private Image prompt;
    [SerializeField] private TextMeshProUGUI promptText;

    [SerializeField] private LayerMask interactPromptMask;
    [SerializeField] private PlayerController playerController;

    private void Start()
    {
        playerController = GetComponent<PlayerController>();
        prompt.enabled = false;
        promptText.enabled = false;

    }

    private void Update()
    {
        Ray r = new Ray(InteractorSource.position, InteractorSource.forward);
        Debug.DrawRay(InteractorSource.position, InteractorSource.forward, Color.green);
        if (Physics.Raycast(r, out RaycastHit hitInfo, InteractRange, interactPromptMask))
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (hitInfo.collider.gameObject.TryGetComponent(out IInteractable interactObj))
                {
                    interactObj.Interact();
                }                
            }

            if (!playerController.onHelm)
            {                
                prompt.enabled = true;
                promptText.enabled = true;
            }
            else
            {
                prompt.enabled = false;
                promptText.enabled = false;
            }
        }
        else
        {
            prompt.enabled = false;
            promptText.enabled = false;
        }
    }

}
