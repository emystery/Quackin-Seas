using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class MenuAchievements : MonoBehaviour
{
    public GameObject AchievementMenuUI;

    public Image AchievementMenu;

    void Start()
    {
        AchievementMenuUI.SetActive(false);
    }

    void Update()
    {
        AchievementMenuUI.SetActive(GameManager.self.state == GameState.LogBook);
        AchievementMenu.enabled = GameManager.self.state == GameState.LogBook;
    }
}