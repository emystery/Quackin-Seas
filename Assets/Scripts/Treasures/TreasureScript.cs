using UnityEngine;

public class TreasureScript : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt;


    private AchievementsBook achievementsBook;
    public Treasure treasure;

    public string InteractionPrompt => prompt;

    public void Start()
    {
        achievementsBook = FindObjectOfType<AchievementsBook>();
        if (achievementsBook == null)
        {
            Debug.LogError("AchievementsBook component not found in scene.");
        }
    }

    private void Update()
    {
        if (GameManager.self.unlockTreasures == true && gameObject.activeSelf)
        {
            Interact();
        }
    }

    public void Interact()
    {
        achievementsBook.AddTreasure(treasure);
        gameObject.SetActive(false);
        GameManager.self.treasuresLeft --;
    }
}

[System.Serializable]
public class Treasure
{
    public string name;
    public Sprite image;
    public int index;
}
