using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using static LevelManager;

public class FruitFusionGameManager : MonoBehaviour
{
    public Text timerText;
    public Text currentScoreText;
    public List<Text> fusionCountTexts; // Liste pour les diff�rents Text UI
    public GameObject winUI;
    public GameObject gameOverUI;
    public Transform topBoundary;
    public LayerMask fruitLayer;
    public FruitPlacer fruitPlacer;
    public Collider2D touchCollider;
    public float moveDistance = 3f;
    public float returnDelay = 3f;
    public Collider2D topBoundaryCollider;

    public FusionSpawnManager fusionSpawnManager;

    public List<GameObject> targetFruitPrefabs; // Liste des prefabs de fruits cibles
    public List<int> requiredFusions; // Liste des fusions n�cessaires par fruit

    public List<ParticleSystem> fusionVFXPrefabs; // Liste des VFX � jouer pour chaque fruit fusionn�
    public List<Transform> fusionVFXSpawnPoints; // Liste des points o� les VFX vont appara�tre

    private Dictionary<string, int> fusionCounts = new Dictionary<string, int>(); // Dictionnaire pour les fusions par type
    private HashSet<GameObject> fusedFruits = new HashSet<GameObject>(); // Garde une trace des fusions d�j� compt�es

    private int currentScore = 0;
    public float timer = 60f;
    private Vector3 initialTopBoundaryPosition;
    private bool isTopBoundaryMoving = false;
    private bool isGameOver = false;

    public StarRatingManager starRatingManager; // Gestionnaire des �toiles
    public int level;

    public bool canContinueWin = false;
    private bool isGameFinish = false;
    void Start()
    {
        initialTopBoundaryPosition = topBoundary.position;
        InitializeFusionCounts();
        UpdateScoreUI();
        UpdateFusionCountUI();

        Time.timeScale = 1;
    }

    void Update()
    {
        if (isGameOver || isGameFinish) return;
        timer -= Time.deltaTime;
        timerText.text = "Temps : " + Mathf.Ceil(timer).ToString() + "s";

        if (timer <= 0)
        {
            GameOver();
            return;
        }

        // V�rifier si le topBoundary est touch� par un fruit
        List<Collider2D> fruits = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(fruitLayer);
        touchCollider.Overlap(filter, fruits);

        if (fruits.Count > 0 && !isTopBoundaryMoving)
        {
            StartCoroutine(MoveTopBoundary(Vector3.right)); // D�placer vers la droite
        }

        // Mettre � jour les �toiles
        //starRatingManager.UpdateStarRating(timer);

        // V�rifier si un fruit touche le topBoundary pour Game Over
        List<Collider2D> gameOverFruits = new List<Collider2D>();
        topBoundaryCollider.Overlap(filter, gameOverFruits);

        if (gameOverFruits.Count > 0)
        {
            GameOver();
        }

        if (isGameOver) return; // Arr�te la logique si le jeu est en pause
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & fruitLayer) != 0 && !fusedFruits.Contains(other.gameObject))
        {
            Debug.Log("Fruit entr� dans le trigger");

            string fruitTag = other.gameObject.tag;

            if (fusionCounts.ContainsKey(fruitTag))
            {
                // V�rifier si la fusion pour ce fruit a d�j� atteint le maximum requis
                int index = targetFruitPrefabs.FindIndex(fruit => fruit.tag == fruitTag);
                if (index != -1 && fusionCounts[fruitTag] < requiredFusions[index])
                {
                    fusionCounts[fruitTag]++;
                    fusedFruits.Add(other.gameObject);
                    UpdateFusionCountUI();

                    // **Jouer la VFX correspondante ici, quand la fusion atteint la quantit� requise**
                    if (fusionCounts[fruitTag] == requiredFusions[index])
                    {
                        PlayFusionVFX(index);
                    }

                    // V�rifier la condition de victoire globale
                    if (CheckAllFusionsComplete())
                    {
                        StartCoroutine(Win());
                    }
                }
            }
        }
    }

    private IEnumerator MoveTopBoundary(Vector3 direction)
    {
        isTopBoundaryMoving = true;

        Vector3 targetPosition = topBoundary.position + direction * moveDistance;

        float elapsedTime = 0f;
        float moveDuration = 0.5f;
        Vector3 startPosition = topBoundary.position;

        while (elapsedTime < moveDuration)
        {
            topBoundary.position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        topBoundary.position = targetPosition;

        yield return new WaitForSeconds(returnDelay);

        topBoundary.position = initialTopBoundaryPosition;
        isTopBoundaryMoving = false;
    }

    private void InitializeFusionCounts()
    {
        foreach (var targetFruit in targetFruitPrefabs)
        {
            fusionCounts[targetFruit.tag] = 0;
        }
    }

    void UpdateScoreUI()
    {
        currentScoreText.text = "Score actuel : " + currentScore.ToString();
    }

    void UpdateFusionCountUI()
    {
        for (int i = 0; i < fusionCountTexts.Count; i++)
        {
            if (i < targetFruitPrefabs.Count)
            {
                string fruitTag = targetFruitPrefabs[i].tag;
                fusionCountTexts[i].text = $"{fusionCounts[fruitTag]}/{requiredFusions[i]}";
            }
        }
    }

    bool CheckAllFusionsComplete()
    {
        for (int i = 0; i < targetFruitPrefabs.Count; i++)
        {
            string fruitTag = targetFruitPrefabs[i].tag;
            if (fusionCounts[fruitTag] < requiredFusions[i])
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator Win()
    {
        isGameFinish = true;
        // Appeler les effets de victoire avant de continuer
        GetComponent<VictoryDefeatEffectsManager>().PlayVictoryEffects();
        winUI.SetActive(true);
        yield return new WaitUntil( () => canContinueWin == true);
        // Afficher l'interface de victoire

        // D�terminer le nombre d'�toiles en fonction du temps restant
        int stars = starRatingManager.CalculateStars(timer);
        starRatingManager.UpdateStarRating(timer);
        // R�cup�rer les �toiles actuellement stock�es pour ce niveau
        int currentStars = PlayerPrefs.GetInt("Stars" + level, 0);

        // Comparer les �toiles actuelles avec celles calcul�es et ne garder que le maximum
        if (stars > currentStars)
        {
            // Mettre � jour les �toiles si le nouveau nombre est sup�rieur
            PlayerPrefs.SetInt("Stars" + level, stars);
        }

        // Enregistrer le niveau comme compl�t�
        PlayerPrefs.SetInt("LevelComplete", level);
        PlayerPrefs.SetInt("isComplete" + level, 1);

        // D�bloquer le niveau suivant
        //int nextLevel = level + 1;
        //if (nextLevel <= LevelManager.Instance.levelButtons.Length)
        //{
        //    PlayerPrefs.SetInt("isComplete" + nextLevel, 1);
        //}

        // Appeler la mise � jour de l'affichage des �toiles dans LevelManager
        LevelManager.Instance.UpdateLevelStars(level, Mathf.Max(stars, currentStars));

        
    }

    public void RestartGame()
    {
        Time.timeScale = 1;
        timer = 9999999f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        timer = 9999999f;
    }

    public void GameOver()
    {
        // Appeler les effets de d�faite avant de continuer
        GetComponent<VictoryDefeatEffectsManager>().PlayDefeatEffects();

        if (isGameOver) return; // Emp�che d'appeler plusieurs fois
        isGameOver = true;

        gameOverUI.SetActive(true);
        Time.timeScale = 0;
        // Arr�ter toutes les interactions sans modifier Time.timeScale

        
    }

    public void DisplayVfx(GameObject obj)
    {
        fusionSpawnManager.RegisterFusion(obj);
    }

    // M�thode pour jouer le VFX de fusion
    private void PlayFusionVFX(int index)
    {
        if (index >= 0 && index < fusionVFXPrefabs.Count && index < fusionVFXSpawnPoints.Count)
        {
            // Instancier et jouer la VFX � la position du point de fusion
            Transform spawnPoint = fusionVFXSpawnPoints[index];
            ParticleSystem fusionVFX = Instantiate(fusionVFXPrefabs[index], spawnPoint.position, Quaternion.identity);
            fusionVFX.Play();

            // D�truire la VFX apr�s sa dur�e de vie
            Destroy(fusionVFX.gameObject, fusionVFX.main.duration);
        }
        else
        {
            Debug.LogWarning("Aucun VFX n'est d�fini pour l'index " + index);
        }
    }
}



























