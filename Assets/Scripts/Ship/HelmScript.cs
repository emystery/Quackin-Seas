using UnityEngine;

public class HelmScript : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject ship;

    public string InteractionPrompt => prompt;


    public void Interact()
    {

        if (!player.GetComponent<PlayerController>().onHelm)
        {
            Debug.Log("Grabbing onto Helm");
            player.GetComponent<PlayerController>().onShip = true;

            Vector3 helmSpotPos =
                new Vector3(
                    ship.GetComponent<ShipController>().helmSpot.transform.position.x,
                    ship.GetComponent<ShipController>().helmSpot.transform.position.y + player.GetComponent<Transform>().lossyScale.y/2,
                    ship.GetComponent<ShipController>().helmSpot.transform.position.z);

            player.GetComponent<Rigidbody>().velocity = Vector3.zero;
            player.transform.position = helmSpotPos;
            player.GetComponent<PlayerController>().onHelm = true;
            player.transform.parent = ship.GetComponent<ShipController>().helmSpot.transform;


        }
        else
        {
            Debug.Log("Letting go of Helm");

            player.GetComponent<PlayerController>().onHelm = false;
           

        }

    }
}
