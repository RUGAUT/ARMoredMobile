using UnityEngine;
using UnityEngine.SceneManagement; // N'oublie pas d'ajouter cette ligne

public class GameController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Exemple de chargement de sc�ne d�s le d�but (par nom de sc�ne)
        //LoadScene("NomDeTaScene");  // Remplace "NomDeTaScene" par le nom de ta sc�ne
    }

    // Update is called once per frame
    void Update()
    {
        // Exemple de changement de sc�ne quand une touche est press�e (par exemple, la touche 'P')
        if (Input.GetKeyDown(KeyCode.P))
        {
            //LoadScene("NomDeTaScene");  // Remplace "NomDeTaScene" par le nom de ta sc�ne
        }
    }

    // Fonction pour charger une sc�ne par son nom
    void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
