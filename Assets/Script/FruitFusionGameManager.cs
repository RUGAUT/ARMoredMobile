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
    public List<Text> fusionCountTexts;
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

    public List<GameObject> targetFruitPrefabs;
    public List<int> requiredFusions;

    public List<ParticleSystem> fusionVFXPrefabs;
    public List<Transform> fusionVFXSpawnPoints;

    private Dictionary<string, int> fusionCounts = new Dictionary<string, int>();
    private HashSet<GameObject> fusedFruits = new HashSet<GameObject>();

    private int currentScore = 0;
    public float timer = 60f;
    private Vector3 initialTopBoundaryPosition;
    private bool isTopBoundaryMoving = false;
    private bool isGameOver = false;

    public StarRatingManager starRatingManager;
    public int level;

    public bool canContinueWin = false;
    private bool isGameFinish = false;

    public List<GameObject> targetFinishObjects;

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
        timerText.text = ": " + Mathf.Ceil(timer).ToString() + "s";

        if (timer <= 0)
        {
            GameOver();
            return;
        }

        List<Collider2D> fruits = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(fruitLayer);
        touchCollider.Overlap(filter, fruits);

        if (fruits.Count > 0 && !isTopBoundaryMoving)
        {
            StartCoroutine(MoveTopBoundary(Vector3.right));
        }

        List<Collider2D> gameOverFruits = new List<Collider2D>();
        topBoundaryCollider.Overlap(filter, gameOverFruits);

        if (gameOverFruits.Count > 0)
        {
            GameOver();
        }

        if (isGameOver) return;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & fruitLayer) != 0 && !fusedFruits.Contains(other.gameObject))
        {
            Debug.Log("Fruit entré dans le trigger");

            string fruitTag = other.gameObject.tag;

            if (fusionCounts.ContainsKey(fruitTag))
            {
                int index = targetFruitPrefabs.FindIndex(fruit => fruit.tag == fruitTag);
                if (index != -1 && fusionCounts[fruitTag] < requiredFusions[index])
                {
                    fusionCounts[fruitTag]++;
                    fusedFruits.Add(other.gameObject);
                    UpdateFusionCountUI();

                    if (fusionCounts[fruitTag] == requiredFusions[index])
                    {
                        PlayFusionVFX(index);

                        if (index < targetFinishObjects.Count && targetFinishObjects[index] != null)
                        {
                            targetFinishObjects[index].SetActive(true);
                        }
                    }

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

        // Vérification et appel sécurisé de VictoryDefeatEffectsManager
        VictoryDefeatEffectsManager effectsManager = GetComponent<VictoryDefeatEffectsManager>();
        if (effectsManager != null)
        {
            effectsManager.PlayVictoryEffects();
        }
        else
        {
            Debug.LogWarning("VictoryDefeatEffectsManager introuvable sur ce GameObject !");
        }

        winUI.SetActive(true);
        yield return new WaitUntil(() => canContinueWin == true);

        // Vérification de starRatingManager
        if (starRatingManager != null)
        {
            int stars = starRatingManager.CalculateStars(timer);
            starRatingManager.UpdateStarRating(timer);
            int currentStars = PlayerPrefs.GetInt("Stars" + level, 0);

            if (stars > currentStars)
            {
                PlayerPrefs.SetInt("Stars" + level, stars);
            }

            PlayerPrefs.SetInt("LevelComplete", level);
            PlayerPrefs.SetInt("isComplete" + level, 1);

            // Vérification de LevelManager.Instance
            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.UpdateLevelStars(level, Mathf.Max(stars, currentStars));
            }
            else
            {
                Debug.LogWarning("LevelManager.Instance est null !");
            }
        }
        else
        {
            Debug.LogWarning("starRatingManager est null !");
        }
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
        // Vérification et appel sécurisé de VictoryDefeatEffectsManager
        VictoryDefeatEffectsManager effectsManager = GetComponent<VictoryDefeatEffectsManager>();
        if (effectsManager != null)
        {
            effectsManager.PlayDefeatEffects();
        }
        else
        {
            Debug.LogWarning("VictoryDefeatEffectsManager introuvable sur ce GameObject !");
        }

        if (isGameOver) return;
        isGameOver = true;

        gameOverUI.SetActive(true);
        Time.timeScale = 0;
    }

    public void DisplayVfx(GameObject obj)
    {
        fusionSpawnManager.RegisterFusion(obj);
    }

    private void PlayFusionVFX(int index)
    {
        if (index >= 0 && index < fusionVFXPrefabs.Count && index < fusionVFXSpawnPoints.Count)
        {
            Transform spawnPoint = fusionVFXSpawnPoints[index];
            ParticleSystem fusionVFX = Instantiate(fusionVFXPrefabs[index], spawnPoint.position, Quaternion.identity);
            fusionVFX.Play();
            Destroy(fusionVFX.gameObject, fusionVFX.main.duration);
        }
        else
        {
            Debug.LogWarning("Aucun VFX n'est défini pour l'index " + index);
        }
    }
}