using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    // R�f�rences aux panels
    public GameObject mainMenuPanel; // Panel du menu principal
    public GameObject panel1;       // Premier panel suppl�mentaire
    public GameObject panel2;       // Deuxi�me panel suppl�mentaire

    // R�f�rences aux boutons
    public Button button1;          // Bouton pour afficher le panel1
    public Button button2;          // Bouton pour afficher le panel2
    public Button backButton1;      // Bouton pour revenir au menu depuis panel1
    public Button backButton2;      // Bouton pour revenir au menu depuis panel2

    void Start()
    {
        // D�sactiver les panels suppl�mentaires au d�marrage
        panel1.SetActive(false);
        panel2.SetActive(false);

        // Ajouter des �couteurs aux boutons
        button1.onClick.AddListener(() => ShowPanel(panel1));
        button2.onClick.AddListener(() => ShowPanel(panel2));
        backButton1.onClick.AddListener(ShowMainMenu);
        backButton2.onClick.AddListener(ShowMainMenu);
    }

    // Affiche un panel sp�cifique et cache le menu principal
    void ShowPanel(GameObject panelToShow)
    {
        mainMenuPanel.SetActive(false); // Cacher le menu principal
        panelToShow.SetActive(true);    // Afficher le panel sp�cifi�
    }

    // R�affiche le menu principal et cache tous les autres panels
    void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true); // Afficher le menu principal
        panel1.SetActive(false);       // Cacher panel1
        panel2.SetActive(false);       // Cacher panel2
    }
}