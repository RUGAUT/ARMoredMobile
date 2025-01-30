using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    // Références aux panels
    public GameObject mainMenuPanel; // Panel du menu principal
    public GameObject panel1;       // Premier panel supplémentaire
    public GameObject panel2;       // Deuxième panel supplémentaire

    // Références aux boutons
    public Button button1;          // Bouton pour afficher le panel1
    public Button button2;          // Bouton pour afficher le panel2
    public Button backButton1;      // Bouton pour revenir au menu depuis panel1
    public Button backButton2;      // Bouton pour revenir au menu depuis panel2

    void Start()
    {
        // Désactiver les panels supplémentaires au démarrage
        panel1.SetActive(false);
        panel2.SetActive(false);

        // Ajouter des écouteurs aux boutons
        button1.onClick.AddListener(() => ShowPanel(panel1));
        button2.onClick.AddListener(() => ShowPanel(panel2));
        backButton1.onClick.AddListener(ShowMainMenu);
        backButton2.onClick.AddListener(ShowMainMenu);
    }

    // Affiche un panel spécifique et cache le menu principal
    void ShowPanel(GameObject panelToShow)
    {
        mainMenuPanel.SetActive(false); // Cacher le menu principal
        panelToShow.SetActive(true);    // Afficher le panel spécifié
    }

    // Réaffiche le menu principal et cache tous les autres panels
    void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true); // Afficher le menu principal
        panel1.SetActive(false);       // Cacher panel1
        panel2.SetActive(false);       // Cacher panel2
    }
}