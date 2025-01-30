using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public LevelButton[] levelButtons; // Liste des boutons avec leurs clés et scènes

    [System.Serializable]
    public class LevelButton
    {
        public Button button;       // Le bouton pour le niveau
        public string levelKey;     // La clé PlayerPrefs (ex: "isComplete1")
        public string sceneName;    // Le nom de la scène à charger
        public Image[] starImages;  // Les images des étoiles associées au bouton
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
       
        levelButtons = LevelButtonsUI.Instance.levelButtons;
        int actualLevel = PlayerPrefs.GetInt("LevelComplete");
        int nextLevel = actualLevel + 1;
        if (nextLevel <= levelButtons.Length)
        {
            PlayerPrefs.SetInt("isComplete" + nextLevel, 1);
        }
        if (levelButtons == null)
            return;
        foreach (LevelButton levelButton in levelButtons)
        {
            // Désactiver toutes les étoiles au début
            foreach (Image starImage in levelButton.starImages)
            {
                starImage.enabled = false;
            }

            int isComplete = PlayerPrefs.GetInt(levelButton.levelKey, 0);
            int starCount = PlayerPrefs.GetInt("Stars" + levelButton.levelKey.Substring("isComplete".Length), 0);

            // Vérification spéciale pour le niveau 1
            if (levelButton.levelKey == "isComplete1" && isComplete == 0)
            {
                // Le niveau 1 est débloqué mais sans étoiles de base
                levelButton.button.onClick.AddListener(() =>
                {
                    SceneManager.LoadScene(levelButton.sceneName);
                });
                levelButton.button.interactable = true; // Le niveau 1 est jouable
            }
            else if (isComplete == 1)
            {
                levelButton.button.onClick.AddListener(() =>
                {
                    SceneManager.LoadScene(levelButton.sceneName);
                });

                // Activer uniquement les étoiles correspondant au nombre enregistré
                for (int i = 0; i < levelButton.starImages.Length; i++)
                {
                    if (i < starCount)
                    {
                        levelButton.starImages[i].enabled = true; // Activer les étoiles gagnées
                    }
                }
            }
            else
            {
                levelButton.button.interactable = false;
                levelButton.button.onClick.AddListener(() =>
                {
                    Debug.Log("Niveau non complété : " + levelButton.levelKey);
                });
            }
        }
    }


    public void UpdateLevelStars(int level, int starCount)
    {
        foreach (LevelButton levelButton in levelButtons)
        {
            if (levelButton.levelKey == "isComplete" + level)
            {
                Debug.Log("Mise à jour des étoiles pour le niveau : " + level + " avec " + starCount + " étoiles");

                // Désactiver toutes les étoiles avant la mise à jour
                foreach (Image starImage in levelButton.starImages)
                {
                    starImage.enabled = false;
                }

                // Activer uniquement les étoiles correspondant au nombre enregistré
                for (int i = 0; i < starCount; i++)
                {
                    levelButton.starImages[i].enabled = true;
                }
            }
        }
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }
}






