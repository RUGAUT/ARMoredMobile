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
    public List<Text> fusionCountTexts; // Liste pour les différents Text UI
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
    public List<int> requiredFusions; // Liste des fusions nécessaires par fruit

    private Dictionary<string, int> fusionCounts = new Dictionary<string, int>(); // Dictionnaire pour les fusions par type
    private HashSet<GameObject> fusedFruits = new HashSet<GameObject>(); // Garde une trace des fusions déjà comptées

    private int currentScore = 0;
    public float timer = 60f;
    private Vector3 initialTopBoundaryPosition;
    private bool isTopBoundaryMoving = false;
    private bool isGameOver = false;

    public StarRatingManager starRatingManager; // Gestionnaire des étoiles
    public int level;

    void Start()
    {
        initialTopBoundaryPosition = topBoundary.position;
        InitializeFusionCounts();
        UpdateScoreUI();
        UpdateFusionCountUI();
    }

    void Update()
    {
        timer -= Time.deltaTime;
        timerText.text = "Temps restant : " + Mathf.Ceil(timer).ToString() + "s";

        if (timer <= 0)
        {
            GameOver();
            return;
        }

        // Vérifier si le topBoundary est touché par un fruit
        List<Collider2D> fruits = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(fruitLayer);
        touchCollider.Overlap(filter, fruits);

        if (fruits.Count > 0 && !isTopBoundaryMoving)
        {
            StartCoroutine(MoveTopBoundary(Vector3.right)); // Déplacer vers la droite
        }

        // Mettre à jour les étoiles
        starRatingManager.UpdateStarRating(timer);

        // Vérifier si un fruit touche le topBoundary pour Game Over
        List<Collider2D> gameOverFruits = new List<Collider2D>();
        topBoundaryCollider.Overlap(filter, gameOverFruits);

        if (gameOverFruits.Count > 0)
        {
            GameOver();
        }

        if (isGameOver) return; // Arrête la logique si le jeu est en pause
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & fruitLayer) != 0 && !fusedFruits.Contains(other.gameObject))
        {
            Debug.Log("Fruit entré dans le trigger");

            string fruitTag = other.gameObject.tag;

            if (fusionCounts.ContainsKey(fruitTag))
            {
                fusionCounts[fruitTag]++;
                fusedFruits.Add(other.gameObject);
                UpdateFusionCountUI();

                // Vérifier la condition de victoire globale
                if (CheckAllFusionsComplete())
                {
                    Win();
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
                fusionCountTexts[i].text = $"{fruitTag} : {fusionCounts[fruitTag]}/{requiredFusions[i]}";
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

    void Win()
    {
        // Afficher l'interface de victoire
        winUI.SetActive(true);
        Time.timeScale = 0;

        // Déterminer le nombre d'étoiles en fonction du temps restant
        int stars = starRatingManager.CalculateStars(timer);

        // Récupérer les étoiles actuellement stockées pour ce niveau
        int currentStars = PlayerPrefs.GetInt("Stars" + level, 0);

        // Comparer les étoiles actuelles avec celles calculées et ne garder que le maximum
        if (stars > currentStars)
        {
            // Mettre à jour les étoiles si le nouveau nombre est supérieur
            PlayerPrefs.SetInt("Stars" + level, stars);
        }

        // Enregistrer le niveau comme complété
        PlayerPrefs.SetInt("isComplete" + level, 1);

        // Débloquer le niveau suivant
        int nextLevel = level + 1;
        if (nextLevel <= LevelManager.Instance.levelButtons.Length)
        {
            PlayerPrefs.SetInt("isComplete" + nextLevel, 1);
        }

        // Appeler la mise à jour de l'affichage des étoiles dans LevelManager
        LevelManager.Instance.UpdateLevelStars(level, Mathf.Max(stars, currentStars));
    }




    public void RestartGame()
    {
        Time.timeScale = 1;
        timer = 9999999f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        timer = 9999999f;
    }

    void GameOver()
    {
        if (isGameOver) return; // Empêche d'appeler plusieurs fois
        isGameOver = true;

        gameOverUI.SetActive(true);
        Time.timeScale = 0;
        // Arrêter toutes les interactions sans modifier Time.timeScale
    }

    public void DisplayVfx(GameObject obj)
    {
        fusionSpawnManager.RegisterFusion(obj);
    }
}
























