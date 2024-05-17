using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    public Image deathScreen;
    // Start is called before the first frame update
    void Start()
    {
        deathScreen.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        deathScreen.enabled = GameManager.self.state == GameState.Dead;
    }
}
