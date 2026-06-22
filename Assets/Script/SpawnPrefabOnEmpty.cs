using UnityEngine;

public class FusionSpawnManager : MonoBehaviour
{
    [Header("Configuration du BoxCollider2D")]
    public BoxCollider2D boxCollider2D;

    [Header("Layer des objets fusionnables")]
    public LayerMask fusionLayer;

    [Header("Prefab à générer")]
    public GameObject prefabToSpawn;
    public Transform spawnLocation;

    [Header("Fusions nécessaires pour le spawn")]
    public int requiredFusions = 2;

    [Header("Durée limite entre deux fusions")]
    public float fusionTimeLimit = 5f;

    [Header("Effet sonore")]
    public AudioClip spawnSoundEffect;
    public AudioSource audioSource;

    private int fusionCount = 0;
    private float lastFusionTime = 0f;

    private void Start()
    {
        if (boxCollider2D == null)
        {
            boxCollider2D = GetComponent<BoxCollider2D>();

            if (boxCollider2D == null)
            {
                Debug.LogError("BoxCollider2D manquant !");
                enabled = false;
                return;
            }
        }

        boxCollider2D.isTrigger = true;

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();

            if (audioSource == null)
            {
                Debug.LogWarning("AudioSource manquant → son désactivé");
            }
        }
    }

    public void RegisterFusion(GameObject fusionResult)
    {
        if (!IsInLayerMask(fusionResult, fusionLayer))
            return;

        float currentTime = Time.time;

        if (currentTime - lastFusionTime > fusionTimeLimit)
        {
            fusionCount = 0;
        }

        fusionCount++;
        lastFusionTime = currentTime;

        if (fusionCount >= requiredFusions)
        {
            SpawnPrefab();
            fusionCount = 0;
        }
    }

    private void SpawnPrefab()
    {
        if (prefabToSpawn == null || spawnLocation == null)
        {
            Debug.LogError("Prefab ou spawnLocation manquant !");
            return;
        }

        Instantiate(prefabToSpawn, spawnLocation.position, spawnLocation.rotation);

        PlaySoundEffect();
    }

    private void PlaySoundEffect()
    {
        if (audioSource == null || spawnSoundEffect == null)
            return;

        audioSource.PlayOneShot(spawnSoundEffect);
    }

    private bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return (layerMask.value & (1 << obj.layer)) != 0;
    }
}