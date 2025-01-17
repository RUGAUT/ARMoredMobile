using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using static LevelManager;

public class GameManager : MonoBehaviour
{
    public Text timerText; // Texte pour afficher le temps restant
    public Text currentScoreText; // Texte pour afficher le score actuel
    public Text targetScoreText; // Texte pour afficher le score cible
    public GameObject winUI; // L'interface de victoire
    public GameObject gameOverUI; // L'interface de Game Over
    public Transform topBoundary; // La limite supérieure du jeu
    public LayerMask fruitLayer; // Layer des fruits
    public FruitPlacer fruitPlacer; // Gestionnaire de fruits
    public Collider2D touchCollider; // Collider que le fruit doit toucher pour déplacer le topBoundary
    public float moveDistance = 3f; // Distance de déplacement du topBoundary
    public float returnDelay = 3f; // Délai avant de ramener le topBoundary
    public Collider2D topBoundaryCollider; // Collider de la zone Game Over

    public FusionSpawnManager fusionSpawnManager;

    public int targetScore = 100; // Score cible à atteindre
    public float timeLimit = 60f; // Temps imparti pour atteindre le score cible

    private float timer;
    private int currentScore = 0; // Score actuel
    private Vector3 initialTopBoundaryPosition; // Position initiale du topBoundary
    private bool isTopBoundaryMoving = false; // Indique si le topBoundary est en mouvement

    private bool isGameOver = false; // Ajout d'une variable pour suivre l'état de pause

    public LevelManager levelManager; // Référence au script LevelManager
    public int level;

    // Ajout de la référence au système de notation par étoiles
    public StarRatingManager starRatingManager;

    void Start()
    {
        // Initialiser le timer
        timer = timeLimit;

        // Sauvegarder la position initiale du topBoundary
        initialTopBoundaryPosition = topBoundary.position;
        Debug.Log("target score:" + targetScore);

        // Mettre à jour l'affichage du score cible
        targetScoreText.text = "Score requis : " + targetScore.ToString();

        // Mettre à jour le score actuel
        UpdateScoreUI();
    }

    void Update()
    {
        // Réduire le temps
        timer -= Time.deltaTime;
        timerText.text = "Temps restant : " + Mathf.Ceil(timer).ToString() + "s";

        // Mettre à jour les étoiles
        starRatingManager.UpdateStarRating(timer);

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
            StartCoroutine(MoveTopBoundary());
        }

        // Vérifier si les fruits touchent la zone de Game Over
        List<Collider2D> gameOverFruits = new List<Collider2D>();
        topBoundaryCollider.Overlap(filter, gameOverFruits);

        if (gameOverFruits.Count > 0)
        {
            GameOver();
        }

        // Vérifier si le joueur a atteint le score cible
        if (currentScore >= targetScore)
        {
            Win();
        }

        if (isGameOver) return; // Arrête la logique si le jeu est en pause
    }

    // Coroutine pour déplacer le topBoundary horizontalement
    IEnumerator MoveTopBoundary()
    {
        isTopBoundaryMoving = true;
        topBoundary.position += new Vector3(moveDistance, 0, 0);
        yield return new WaitForSeconds(returnDelay);
        StartCoroutine(ReturnTopBoundary());
    }

    // Coroutine pour ramener le topBoundary à sa position initiale
    IEnumerator ReturnTopBoundary()
    {
        yield return new WaitForSeconds(returnDelay);
        topBoundary.position = initialTopBoundaryPosition;
        isTopBoundaryMoving = false;
    }

    public void AddScore(int points)
    {
        currentScore += points;
        UpdateScoreUI(); // Mettre à jour l'affichage du score
    }

    void UpdateScoreUI()
    {
        // Mettre à jour l'interface utilisateur avec le score actuel
        currentScoreText.text = "Score actuel : " + currentScore.ToString();
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
        // Réinitialiser le temps et redémarrer la scène
        Time.timeScale = 1;
        timer = 999999999f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        timer = 999999999f;
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

























