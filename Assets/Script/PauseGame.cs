using UnityEngine;
using UnityEngine.UI;  // Nécessaire pour travailler avec les boutons UI

public class PauseGame : MonoBehaviour
{
    // Référence au bouton
    public Button pauseButton;

    // Référence au Panel de pause
    public GameObject pausePanel;

    // Variable pour savoir si le jeu est en pause
    private bool isPaused = false;

    // Variable pour garder en mémoire le volume initial
    private float initialVolume;

    // Start is called before the first frame update
    void Start()
    {
        // Assure-toi que le bouton appelle la fonction TogglePause lorsqu'il est cliqué
        pauseButton.onClick.AddListener(TogglePause);

        // Sauvegarde du volume initial du son
        initialVolume = AudioListener.volume;

        // Cache le panel au début
        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    // Cette fonction permet de mettre le jeu en pause ou de le reprendre
    void TogglePause()
    {
        if (isPaused)
        {
            // Reprend le jeu, le son et cache le Panel
            Time.timeScale = 1;
            AudioListener.volume = initialVolume;  // Restaure le volume initial
            if (pausePanel != null)
            {
                pausePanel.SetActive(false);  // Cache le panel
            }
            isPaused = false;
        }
        else
        {
            // Met le jeu, le son en pause et affiche le Panel
            Time.timeScale = 0;
            AudioListener.volume = 0;  // Coupe le son
            if (pausePanel != null)
            {
                pausePanel.SetActive(true);  // Affiche le panel
            }
            isPaused = true;
        }
    }
}




