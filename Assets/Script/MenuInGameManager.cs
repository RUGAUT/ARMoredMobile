using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInGameManager : MonoBehaviour
{
    // M�thode pour charger la sc�ne du menu principal
    public void GoToMainMenu()
    {
        // S'assurer que le jeu est � la vitesse normale
        Time.timeScale = 1;

        // Charger la sc�ne du menu principal
        SceneManager.LoadScene("MainMenu");
    }

    // M�thode pour red�marrer le jeu (recommencer la sc�ne actuelle)
    public void RestartGame()
    {
        // R�initialiser le temps au cas o� le jeu �tait en pause
        Time.timeScale = 1;

        // R�initialiser manuellement tous les objets si n�cessaire
        foreach (var obj in GameObject.FindObjectsOfType<GameObject>())
        {
            if (obj.CompareTag("DontDestroy"))
            {
                Destroy(obj);
            }
        }

        // Recharger la sc�ne actuelle
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // M�thode pour mettre en pause le jeu
    public void PauseGame()
    {
        Time.timeScale = 0; // Met le temps en pause
        Debug.Log("Jeu en pause.");
    }

    // M�thode pour reprendre le jeu apr�s une pause
    public void ResumeGame()
    {
        Time.timeScale = 1; // Reprend le temps normal
        Debug.Log("Jeu repris.");
    }

    // M�thode pour quitter le jeu
    public void QuitGame()
    {
        Debug.Log("Quitter le jeu...");
        Application.Quit(); // Quitte l'application
    }
}


