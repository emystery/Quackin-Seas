using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

    void Update()
    {      
        if (GameManager.self.state != GameState.MainMenu)
        {
            pauseMenuUI.SetActive(GameManager.self.state == GameState.Paused);
           
        }
    }

    public void LoadScene(string sceneName)
    {
        GameManager.self.LoadScene(sceneName);
    }

    public void Resume()
    {
        GameManager.self.Resume();
    }

    public void Quit()
    {
        GameManager.self.QuitGame();
    }

}
