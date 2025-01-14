using UnityEngine;
using UnityEngine.UI; // Pour l'utilisation des éléments d'interface utilisateur
using UnityEngine.SceneManagement; // Pour redémarrer la scène
using System.Collections; // Nécessaire pour IEnumerator

public class GameManagerFixed : MonoBehaviour
{
    public Text scoreText; // Texte pour afficher le score actuel
    public Text bestScoreText; // Texte pour afficher le meilleur score
    public GameObject gameOverUI; // L'interface de Game Over
    public Transform topBoundary; // La limite supérieure du jeu
    public LayerMask fruitLayer; // Layer dédié aux fruits
    public FruitPlacer fruitPlacer; // Référence au gestionnaire de fruits
    public Collider2D touchCollider; // Le collider spécifique que le fruit doit toucher pour déplacer le topBoundary
    public float moveDistance = 3f; // Distance de déplacement du topBoundary
    public float returnDelay = 3f; // Délai avant de ramener le topBoundary à sa position initiale
    public Collider2D topBoundaryCollider; // Collider spécifique de la zone Game Over

    private int currentScore = 0; // Score actuel
    private int bestScore = 0; // Meilleur score sauvegardé
    private Vector3 initialTopBoundaryPosition; // Position de départ du topBoundary
    private bool isTopBoundaryMoving = false; // Vérifier si le topBoundary est en mouvement

    void Start()
    {
        // Sauvegarder la position initiale du topBoundary
        initialTopBoundaryPosition = topBoundary.position;

        // Charger le meilleur score sauvegardé
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        UpdateScoreUI();
    }

    void Update()
    {
        // Vérifier si le topBoundary est touché par un fruit
        if (touchCollider.IsTouchingLayers(fruitLayer) && !isTopBoundaryMoving)
        {
            StartCoroutine(MoveTopBoundary());
        }

        // Vérifier si un fruit atteint la zone de Game Over
        if (topBoundaryCollider.IsTouchingLayers(fruitLayer))
        {
            GameOver();
        }
    }

    IEnumerator MoveTopBoundary()
    {
        isTopBoundaryMoving = true;

        // Déplacer le topBoundary vers la droite (ou ajustez selon vos besoins)
        topBoundary.position += new Vector3(moveDistance, 0, 0);

        yield return new WaitForSeconds(returnDelay);

        // Ramener le topBoundary à sa position initiale
        topBoundary.position = initialTopBoundaryPosition;
        isTopBoundaryMoving = false;
    }

    public void AddScore(int points)
    {
        currentScore += points;
        UpdateScoreUI();

        // Mettre à jour le meilleur score si nécessaire
        if (currentScore > bestScore)
        {
            bestScore = currentScore;
            PlayerPrefs.SetInt("BestScore", bestScore); // Sauvegarder le meilleur score
        }
    }

    void UpdateScoreUI()
    {
        scoreText.text = $"Score: {currentScore}";
        bestScoreText.text = $"Meilleur Score: {bestScore}";
    }

    public void RestartGame()
    {
        Time.timeScale = 1; // Remettre le temps en marche
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Recharger la scène actuelle
    }

    void GameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0; // Mettre le jeu en pause
    }
}


