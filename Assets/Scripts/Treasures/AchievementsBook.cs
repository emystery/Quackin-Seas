using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementsBook : MonoBehaviour
{
    public List<Treasure> loggedTreasures;
    public TreasureElement[] treasureElements;

    bool menuOpen = false;

    void Start()
    {
        loggedTreasures = new List<Treasure>(); 
        treasureElements = GetComponentsInChildren<TreasureElement>(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            menuOpen = !menuOpen;
        }
    }

    public void AddTreasure(Treasure treasureToAdd)
    {
        if (loggedTreasures.Contains(treasureToAdd))
        {
            return;
        }
        loggedTreasures.Add(treasureToAdd);
        UpdateTreasures(treasureToAdd);
    }

    void UpdateTreasures(Treasure addedTreasure)
    {

        foreach(TreasureElement element in treasureElements)
        {
            if(element.index == addedTreasure.index)
            {
                element.LogTreasure(addedTreasure);
                return;
            }
        }
    }
}
