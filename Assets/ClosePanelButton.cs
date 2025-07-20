using UnityEngine;

public class ClosePanelButton : MonoBehaviour
{
    public GameObject panelToClose;

    public void ClosePanel()
    {
        if (panelToClose != null)
        {
            panelToClose.SetActive(false);
            // Reprend le jeu, le son et cache le Panel
            Time.timeScale = 1;
        }
    }
}
