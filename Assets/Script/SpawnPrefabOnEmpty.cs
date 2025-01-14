using UnityEngine;

public class FusionSpawnManager : MonoBehaviour
{
    [Header("Configuration du BoxCollider2D")]
    public BoxCollider2D boxCollider2D; // Zone où se produisent les fusions

    [Header("Layer des objets fusionnables")]
    public LayerMask fusionLayer; // Layer des objets pouvant fusionner

    [Header("Prefab à générer")]
    public GameObject prefabToSpawn; // Le prefab à instancier
    public Transform spawnLocation; // L'endroit où le prefab doit apparaître

    [Header("Fusions nécessaires pour le spawn")]
    public int requiredFusions = 2; // Nombre de fusions consécutives nécessaires

    [Header("Durée limite entre deux fusions")]
    public float fusionTimeLimit = 5f; // Temps (en secondes) entre deux fusions pour être comptées comme successives

    [Header("Effet sonore")]
    public AudioClip spawnSoundEffect; // Son joué lors du spawn
    public AudioSource audioSource; // Composant AudioSource pour jouer le son

    private int fusionCount = 0; // Compteur de fusions successives
    private float lastFusionTime = 0f; // Temps de la dernière fusion enregistrée

    private void Start()
    {
        if (boxCollider2D == null)
        {
            boxCollider2D = GetComponent<BoxCollider2D>();
            if (boxCollider2D == null)
            {
                Debug.LogError("Aucun BoxCollider2D n'est assigné ou présent sur l'objet !");
                enabled = false;
                return;
            }
        }

        if (!boxCollider2D.isTrigger)
        {
            Debug.LogWarning("Le BoxCollider2D doit être défini comme Trigger !");
            boxCollider2D.isTrigger = true; // Assure que le collider est en mode Trigger
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("Aucun AudioSource assigné ou trouvé sur cet objet !");
            }
        }
    }

    public void RegisterFusion(GameObject fusionResult)
    {
        // Vérifie si le résultat de la fusion est dans le bon Layer
        if (IsInLayerMask(fusionResult, fusionLayer))
        {
            float currentTime = Time.time;

            // Vérifie si la dernière fusion a été trop éloignée dans le temps
            if (currentTime - lastFusionTime > fusionTimeLimit)
            {
                fusionCount = 0; // Réinitialise le compteur
                Debug.Log("Temps écoulé trop long entre les fusions. Compteur réinitialisé.");
            }

            fusionCount++;
            lastFusionTime = currentTime;

            Debug.Log($"Fusion enregistrée. Compteur actuel : {fusionCount}");

            // Si le nombre de fusions atteint le seuil requis
            if (fusionCount >= requiredFusions)
            {
                SpawnPrefab();
                fusionCount = 0; // Réinitialise le compteur après le spawn
            }
        }
    }

    private void SpawnPrefab()
    {
        Debug.Log("Prefab instancié après deux fusions !");
        if (prefabToSpawn != null && spawnLocation != null)
        {
            Instantiate(prefabToSpawn, spawnLocation.position, spawnLocation.rotation);
            Debug.Log("Prefab instancié après deux fusions !");

            // Joue l'effet sonore
            PlaySoundEffect();
        }
        else
        {
            Debug.LogError("Prefab ou emplacement non défini !");
        }
    }

    private void PlaySoundEffect()
    {
        if (audioSource != null && spawnSoundEffect != null)
        {
            audioSource.PlayOneShot(spawnSoundEffect);
        }
        else
        {
            Debug.LogWarning("Aucun effet sonore ou AudioSource n'est défini !");
        }
    }

    // Vérifie si un GameObject appartient au LayerMask
    private bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return (layerMask.value & (1 << obj.layer)) != 0;
    }
}




