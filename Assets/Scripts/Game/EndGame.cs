using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Interact()
    {
        GameManager.self.gameEnded = true;
        GameManager.self.state = GameState.EndScreen;
    }
}
