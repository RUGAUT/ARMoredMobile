using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInGameManager : MonoBehaviour
{
    // Méthode pour charger la scène du menu principal
    public void GoToMainMenu()
    {
        // S'assurer que le jeu est à la vitesse normale
        Time.timeScale = 1;

        // Charger la scène du menu principal
        SceneManager.LoadScene("MainMenu");
    }

    // Méthode pour redémarrer le jeu (recommencer la scène actuelle)
    public void RestartGame()
    {
        // Réinitialiser le temps au cas où le jeu était en pause
        Time.timeScale = 1;

        // Réinitialiser manuellement tous les objets si nécessaire
        foreach (var obj in GameObject.FindObjectsOfType<GameObject>())
        {
            if (obj.CompareTag("DontDestroy"))
            {
                Destroy(obj);
            }
        }

        // Recharger la scène actuelle
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Méthode pour mettre en pause le jeu
    public void PauseGame()
    {
        Time.timeScale = 0; // Met le temps en pause
        Debug.Log("Jeu en pause.");
    }

    // Méthode pour reprendre le jeu après une pause
    public void ResumeGame()
    {
        Time.timeScale = 1; // Reprend le temps normal
        Debug.Log("Jeu repris.");
    }

    // Méthode pour quitter le jeu
    public void QuitGame()
    {
        Debug.Log("Quitter le jeu...");
        Application.Quit(); // Quitte l'application
    }
}


