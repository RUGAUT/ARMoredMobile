using UnityEngine;
using UnityEngine.UI; // Pour l'utilisation des �l�ments d'interface utilisateur
using UnityEngine.SceneManagement; // Pour red�marrer la sc�ne
using System.Collections; // N�cessaire pour IEnumerator

public class GameManagerFixed : MonoBehaviour
{
    public Text scoreText; // Texte pour afficher le score actuel
    public Text bestScoreText; // Texte pour afficher le meilleur score
    public GameObject gameOverUI; // L'interface de Game Over
    public Transform topBoundary; // La limite sup�rieure du jeu
    public LayerMask fruitLayer; // Layer d�di� aux fruits
    public FruitPlacer fruitPlacer; // R�f�rence au gestionnaire de fruits
    public Collider2D touchCollider; // Le collider sp�cifique que le fruit doit toucher pour d�placer le topBoundary
    public float moveDistance = 3f; // Distance de d�placement du topBoundary
    public float returnDelay = 3f; // D�lai avant de ramener le topBoundary � sa position initiale
    public Collider2D topBoundaryCollider; // Collider sp�cifique de la zone Game Over

    private int currentScore = 0; // Score actuel
    private int bestScore = 0; // Meilleur score sauvegard�
    private Vector3 initialTopBoundaryPosition; // Position de d�part du topBoundary
    private bool isTopBoundaryMoving = false; // V�rifier si le topBoundary est en mouvement

    void Start()
    {
        // Sauvegarder la position initiale du topBoundary
        initialTopBoundaryPosition = topBoundary.position;

        // Charger le meilleur score sauvegard�
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        UpdateScoreUI();
    }

    void Update()
    {
        // V�rifier si le topBoundary est touch� par un fruit
        if (touchCollider.IsTouchingLayers(fruitLayer) && !isTopBoundaryMoving)
        {
            StartCoroutine(MoveTopBoundary());
        }

        // V�rifier si un fruit atteint la zone de Game Over
        if (topBoundaryCollider.IsTouchingLayers(fruitLayer))
        {
            GameOver();
        }
    }

    IEnumerator MoveTopBoundary()
    {
        isTopBoundaryMoving = true;

        // D�placer le topBoundary vers la droite (ou ajustez selon vos besoins)
        topBoundary.position += new Vector3(moveDistance, 0, 0);

        yield return new WaitForSeconds(returnDelay);

        // Ramener le topBoundary � sa position initiale
        topBoundary.position = initialTopBoundaryPosition;
        isTopBoundaryMoving = false;
    }

    public void AddScore(int points)
    {
        currentScore += points;
        UpdateScoreUI();

        // Mettre � jour le meilleur score si n�cessaire
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // Recharger la sc�ne actuelle
    }

    void GameOver()
    {
        gameOverUI.SetActive(true);
        Time.timeScale = 0; // Mettre le jeu en pause
    }
}


