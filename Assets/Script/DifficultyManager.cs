using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public enum DifficultyLevel
    {
        Easy,
        Medium,
        Hard
    }

    [Header("Difficulty Settings")]
    public DifficultyLevel currentDifficulty = DifficultyLevel.Medium;

    [Header("Gameplay Settings")]
    public float fruitSpawnInterval = 2.0f; // Intervalle entre les apparitions des fruits
    public float obstacleSpawnInterval = 3.0f; // Intervalle entre les obstacles (si applicable)

    private float easyMultiplier = 1.5f; // Modificateur pour le niveau Facile
    private float hardMultiplier = 0.7f; // Modificateur pour le niveau Difficile

    void Start()
    {
        ApplyDifficultySettings();
    }

    public void ApplyDifficultySettings()
    {
        switch (currentDifficulty)
        {
            case DifficultyLevel.Easy:
                fruitSpawnInterval *= easyMultiplier;
                obstacleSpawnInterval *= easyMultiplier;
                break;

            case DifficultyLevel.Hard:
                fruitSpawnInterval *= hardMultiplier;
                obstacleSpawnInterval *= hardMultiplier;
                break;

            default:
                // Medium, pas de modification
                break;
        }

        Debug.Log($"Difficulty set to {currentDifficulty}: Fruit spawn every {fruitSpawnInterval} seconds.");
    }
}

