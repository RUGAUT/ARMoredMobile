using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Singleton pattern pour accès global au GameManager
    public static LevelManager Instance;

    public LevelButton[] levelButtons; // Liste des boutons avec leurs clés et scènes

    [System.Serializable]
    public class LevelButton
    {
        public Button button;       // Le bouton pour le niveau
        public string levelKey;     // La clé PlayerPrefs (ex: "isComplete1")
        public string sceneName;    // Le nom de la scène à charger
    }


    private void Awake()
    {
        // Assurer une seule instance du GameManager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // les etats de niveau
        if (!PlayerPrefs.HasKey("isComplete1"))
            PlayerPrefs.SetInt("isComplete1", 0); // 0 = false et 1 = true on changera les valeur en fin de niveau dans gameManager
        if (!PlayerPrefs.HasKey("isComplete2"))
            PlayerPrefs.SetInt("isComplete2", 0); 
        if (!PlayerPrefs.HasKey("isComplete3"))
            PlayerPrefs.SetInt("isComplete3", 0); 


        // LEs boutons de niveaux
        foreach (LevelButton levelButton in levelButtons)
        {
            if (PlayerPrefs.GetInt(levelButton.levelKey, 0) == 1)
            {
                // Si le niveau est complété, ajouter l'action pour lancer le nv suivant 
                levelButton.button.onClick.AddListener(() =>
                {
                    SceneManager.LoadScene(levelButton.sceneName);
                });
            }
            else
            {
                // Sinon, désactiver le bouton ou ajouter un message
                levelButton.button.interactable = false;
                levelButton.button.onClick.AddListener(() =>
                {
                    Debug.Log("Niveau non complété : " + levelButton.levelKey);
                });
            }
        }

    }

    // Fonction pour charger une scène par son nom
     public void LoadScene(string sceneName)
    {
        Time.timeScale = 1 ;
        SceneManager.LoadScene(sceneName);
    }
}

