using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class FinalIsland : MonoBehaviour
{
    public GameObject finalIsland;

    public GameObject compass;

    private bool initialized;

    // Start is called before the first frame update
    void Start()
    {
        finalIsland.SetActive(false);
        compass.SetActive(false);
        initialized = false;
        StartCoroutine(Initialize());
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.self.treasuresLeft == 0 && initialized)
        {
            finalIsland.SetActive(true);
            compass.SetActive(true);
            compass.transform.forward = finalIsland.transform.position - compass.transform.position;
        }
    }

    private IEnumerator Initialize()
    {
        yield return new WaitForSeconds(0.6f);

        initialized = true;
    }
}
