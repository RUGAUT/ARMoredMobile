using UnityEngine;
using UnityEngine.UI;

public class MuteManager : MonoBehaviour
{
    public Button muteButton;  // Bouton pour couper le son
    public Button unmuteButton; // Bouton pour r�activer le son

    void Start()
    {
        // V�rifier l'�tat du son au d�marrage
        if (AudioListener.volume == 0)
        {
            muteButton.gameObject.SetActive(false);
            unmuteButton.gameObject.SetActive(true);
        }
        else
        {
            muteButton.gameObject.SetActive(true);
            unmuteButton.gameObject.SetActive(false);
        }

        // Ajouter les �v�nements aux boutons
        muteButton.onClick.AddListener(MuteSound);
        unmuteButton.onClick.AddListener(UnmuteSound);
    }

    void MuteSound()
    {
        AudioListener.volume = 0;
        muteButton.gameObject.SetActive(false);
        unmuteButton.gameObject.SetActive(true);
    }

    void UnmuteSound()
    {
        AudioListener.volume = 1;
        muteButton.gameObject.SetActive(true);
        unmuteButton.gameObject.SetActive(false);
    }
}

