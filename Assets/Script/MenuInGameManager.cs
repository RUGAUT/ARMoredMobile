using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInGameManager : MonoBehaviour
{
    public GameObject pausePanel; // Référence à ton UI de pause

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }

    public void RestartGame()
    {
        Time.timeScale = 1;

        foreach (var obj in GameObject.FindObjectsOfType<GameObject>())
        {
            if (obj.CompareTag("DontDestroy"))
            {
                Destroy(obj);
            }
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        Debug.Log("Jeu en pause.");
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        if (pausePanel != null)
            pausePanel.SetActive(false);
        Debug.Log("Jeu repris.");
    }

    public void QuitGame()
    {
        Debug.Log("Quitter le jeu...");
        Application.Quit();
    }
}
