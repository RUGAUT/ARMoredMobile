using UnityEngine;

public class FusionSpawnManager : MonoBehaviour
{
    [Header("Configuration du BoxCollider2D")]
    public BoxCollider2D boxCollider2D; // Zone o� se produisent les fusions

    [Header("Layer des objets fusionnables")]
    public LayerMask fusionLayer; // Layer des objets pouvant fusionner

    [Header("Prefab � g�n�rer")]
    public GameObject prefabToSpawn; // Le prefab � instancier
    public Transform spawnLocation; // L'endroit o� le prefab doit appara�tre

    [Header("Fusions n�cessaires pour le spawn")]
    public int requiredFusions = 2; // Nombre de fusions cons�cutives n�cessaires

    [Header("Dur�e limite entre deux fusions")]
    public float fusionTimeLimit = 5f; // Temps (en secondes) entre deux fusions pour �tre compt�es comme successives

    [Header("Effet sonore")]
    public AudioClip spawnSoundEffect; // Son jou� lors du spawn
    public AudioSource audioSource; // Composant AudioSource pour jouer le son

    private int fusionCount = 0; // Compteur de fusions successives
    private float lastFusionTime = 0f; // Temps de la derni�re fusion enregistr�e

    private void Start()
    {
        if (boxCollider2D == null)
        {
            boxCollider2D = GetComponent<BoxCollider2D>();
            if (boxCollider2D == null)
            {
                Debug.LogError("Aucun BoxCollider2D n'est assign� ou pr�sent sur l'objet !");
                enabled = false;
                return;
            }
        }

        if (!boxCollider2D.isTrigger)
        {
            Debug.LogWarning("Le BoxCollider2D doit �tre d�fini comme Trigger !");
            boxCollider2D.isTrigger = true; // Assure que le collider est en mode Trigger
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("Aucun AudioSource assign� ou trouv� sur cet objet !");
            }
        }
    }

    public void RegisterFusion(GameObject fusionResult)
    {
        // V�rifie si le r�sultat de la fusion est dans le bon Layer
        if (IsInLayerMask(fusionResult, fusionLayer))
        {
            float currentTime = Time.time;

            // V�rifie si la derni�re fusion a �t� trop �loign�e dans le temps
            if (currentTime - lastFusionTime > fusionTimeLimit)
            {
                fusionCount = 0; // R�initialise le compteur
                Debug.Log("Temps �coul� trop long entre les fusions. Compteur r�initialis�.");
            }

            fusionCount++;
            lastFusionTime = currentTime;

            Debug.Log($"Fusion enregistr�e. Compteur actuel : {fusionCount}");

            // Si le nombre de fusions atteint le seuil requis
            if (fusionCount >= requiredFusions)
            {
                SpawnPrefab();
                fusionCount = 0; // R�initialise le compteur apr�s le spawn
            }
        }
    }

    private void SpawnPrefab()
    {
        Debug.Log("Prefab instanci� apr�s deux fusions !");
        if (prefabToSpawn != null && spawnLocation != null)
        {
            Instantiate(prefabToSpawn, spawnLocation.position, spawnLocation.rotation);
            Debug.Log("Prefab instanci� apr�s deux fusions !");

            // Joue l'effet sonore
            PlaySoundEffect();
        }
        else
        {
            Debug.LogError("Prefab ou emplacement non d�fini !");
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
            Debug.LogWarning("Aucun effet sonore ou AudioSource n'est d�fini !");
        }
    }

    // V�rifie si un GameObject appartient au LayerMask
    private bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return (layerMask.value & (1 << obj.layer)) != 0;
    }
}




