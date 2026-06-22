using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public LevelButton[] levelButtons;

    [System.Serializable]
    public class LevelButton
    {
        public Button button;
        public string levelKey;
        public string sceneName;
        public Image[] starImages;
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
            foreach (Image starImage in levelButton.starImages)
            {
                starImage.enabled = false;
            }

            int isComplete = PlayerPrefs.GetInt(levelButton.levelKey, 0);
            int starCount = PlayerPrefs.GetInt(
                "Stars" + levelButton.levelKey.Substring("isComplete".Length),
                0
            );

            if (levelButton.levelKey == "isComplete1" && isComplete == 0)
            {
                levelButton.button.onClick.AddListener(() =>
                {
                    LoadScene(levelButton.sceneName);
                });

                levelButton.button.interactable = true;
            }
            else if (isComplete == 1)
            {
                levelButton.button.onClick.AddListener(() =>
                {
                    LoadScene(levelButton.sceneName);
                });

                for (int i = 0; i < levelButton.starImages.Length; i++)
                {
                    if (i < starCount)
                    {
                        levelButton.starImages[i].enabled = true;
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
                Debug.Log("Mise à jour des étoiles pour le niveau : " +
                          level + " avec " + starCount + " étoiles");

                foreach (Image starImage in levelButton.starImages)
                {
                    starImage.enabled = false;
                }

                for (int i = 0; i < starCount && i < levelButton.starImages.Length; i++)
                {
                    levelButton.starImages[i].enabled = true;
                }
            }
        }
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1;

        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogWarning("Nom de scène vide !");
            return;
        }

        if (Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.Log("Chargement de la scène : " + sceneName);
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning(
                "La scène '" + sceneName +
                "' n'existe pas ou n'est pas ajoutée aux Build Profiles."
            );
        }
    }
}