using UnityEngine;
using UnityEngine.SceneManagement; // N'oublie pas d'ajouter cette ligne

public class GameController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Exemple de chargement de scène dès le début (par nom de scène)
        //LoadScene("NomDeTaScene");  // Remplace "NomDeTaScene" par le nom de ta scène
    }

    // Update is called once per frame
    void Update()
    {
        // Exemple de changement de scène quand une touche est pressée (par exemple, la touche 'P')
        if (Input.GetKeyDown(KeyCode.P))
        {
            //LoadScene("NomDeTaScene");  // Remplace "NomDeTaScene" par le nom de ta scène
        }
    }

    // Fonction pour charger une scène par son nom
    void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
