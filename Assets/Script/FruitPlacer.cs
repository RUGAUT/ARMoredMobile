using UnityEngine;

public class FruitPlacer : MonoBehaviour
{
    public GameObject[] fruits; // Liste des préfabs de fruits
    private GameObject currentFruit; // Fruit actuellement sélectionné
    private GameObject nextFruit; // Le prochain fruit qui va apparaître
    private bool isDragging = false;
    public Transform nextFruitPosition; // Position où afficher le prochain fruit
    public Collider2D forbiddenZoneCollider; // Zone où les fruits ne peuvent pas être placés
    public Collider2D spawnZoneCollider; // Zone où les fruits peuvent apparaître
    public Camera mainCamera; // La caméra principale

    [Header("Difficulty Settings")]
    public DifficultyManager difficultyManager; // Référence au script DifficultyManager
    private float nextFruitSpawnTimer; // Minuteur pour le prochain fruit

    [Header("Cooldown Settings")]
    public float fruitCooldown = 1.0f; // Temps de cooldown entre les placements de fruits
    private float fruitCooldownTimer = 0f; // Minuteur pour le cooldown

    [Header("Fruit Rarity Settings")]
    [Range(0f, 1f)] public float[] fruitRarity; // Probabilités pour chaque fruit d'apparaître (somme totale doit être 1)

    void Start()
    {
        // Initialiser le prochain fruit
        SpawnNextFruit();

        // Initialiser le minuteur en fonction de la difficulté
        if (difficultyManager != null)
        {
            nextFruitSpawnTimer = difficultyManager.fruitSpawnInterval;
        }
    }

    void Update()
    {
        HandleInput();
        HandleFruitSpawnTimer();
        HandleCooldown();
    }

    public void ResetFruitSpawner()
    {
        // Réinitialiser les variables et le comportement des fruits
        currentFruit = null;
        nextFruit = null;

        // Réinitialiser le prochain fruit
        SpawnNextFruit();
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0) && fruitCooldownTimer <= 0) // Vérifier si le cooldown est terminé
        {
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            // Vérifier si le clic est sur un Collider2D donné
            Collider2D clickedCollider = Physics2D.OverlapPoint(mousePosition);

            if (clickedCollider != null)
            {
                if (clickedCollider != forbiddenZoneCollider && clickedCollider != spawnZoneCollider)
                {
                    // Si le clic est sur un autre collider, spawn un fruit sous la souris dans la zone de spawn
                    SpawnFruitAtMousePosition(mousePosition);
                    return;
                }
            }

            if (currentFruit == null)
            {
                SpawnFruitAtMouse();
                isDragging = true;
            }
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            DragFruitToMouse();
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            ReleaseFruit();
        }
    }

    void SpawnFruitAtMousePosition(Vector3 mousePosition)
    {
        // Limiter la position du fruit à la zone de spawn
        Vector3 clampedPosition = ClampPositionToSpawnZone(mousePosition);

        // Instancier un fruit aléatoire à la position limitée
        int randomIndex = GetFruitIndexByRarity();
        Instantiate(fruits[randomIndex], clampedPosition, Quaternion.identity);

        // Réinitialiser le cooldown
        fruitCooldownTimer = fruitCooldown;
    }

    Vector3 ClampPositionToSpawnZone(Vector3 position)
    {
        // Obtenir les limites de la zone de spawn
        Bounds spawnBounds = spawnZoneCollider.bounds;

        // Limiter la position en X et Y pour qu'elle reste dans les limites de la zone de spawn
        float clampedX = Mathf.Clamp(position.x, spawnBounds.min.x, spawnBounds.max.x);
        float clampedY = Mathf.Clamp(position.y, spawnBounds.min.y, spawnBounds.max.y);

        return new Vector3(clampedX, clampedY, position.z);
    }


    void HandleFruitSpawnTimer()
    {
        // Si aucun fruit n'est en cours, lancer un nouveau spawn après un délai
        if (currentFruit == null && nextFruit == null)
        {
            nextFruitSpawnTimer -= Time.deltaTime;

            if (nextFruitSpawnTimer <= 0)
            {
                SpawnNextFruit();

                // Réinitialiser le minuteur
                if (difficultyManager != null)
                {
                    nextFruitSpawnTimer = difficultyManager.fruitSpawnInterval;
                }
            }
        }
    }

    void HandleCooldown()
    {
        // Décrémenter le timer de cooldown si nécessaire
        if (fruitCooldownTimer > 0)
        {
            fruitCooldownTimer -= Time.deltaTime;
        }
    }

    void SpawnFruitAtMouse()
    {
        if (currentFruit == null)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            if (!IsPositionInForbiddenZone(mousePosition) && IsPositionInSpawnZone(mousePosition))
            {
                currentFruit = Instantiate(nextFruit, mousePosition, Quaternion.identity);
                Rigidbody2D rb = currentFruit.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.bodyType = RigidbodyType2D.Kinematic;
                }

                Collider2D collider = currentFruit.GetComponent<Collider2D>();
                if (collider != null)
                {
                    collider.enabled = false;
                }

                // Réinitialiser le cooldown
                fruitCooldownTimer = fruitCooldown;
            }
            else
            {
                Debug.Log("Position invalide, fruit non placé.");
            }
        }
    }

    void SpawnFruitInSpawnZone()
    {
        // Générer une position aléatoire dans la zone de spawn
        Bounds spawnBounds = spawnZoneCollider.bounds;
        Vector3 randomPosition = new Vector3(
            Random.Range(spawnBounds.min.x, spawnBounds.max.x),
            Random.Range(spawnBounds.min.y, spawnBounds.max.y),
            0
        );

        // Instancier un fruit aléatoire
        int randomIndex = GetFruitIndexByRarity();
        Instantiate(fruits[randomIndex], randomPosition, Quaternion.identity);

        // Réinitialiser le cooldown
        fruitCooldownTimer = fruitCooldown;
    }

    void DragFruitToMouse()
    {
        if (currentFruit != null)
        {
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            // Limiter la position du fruit à la zone de spawn
            if (spawnZoneCollider.bounds.Contains(mousePosition))
            {
                // Déplacez le fruit uniquement si le point est à l'intérieur de la zone de spawn
                currentFruit.transform.position = mousePosition;
            }
            else
            {
                // Si le fruit dépasse la zone, le maintenir à la limite de la zone
                Vector3 clampedPosition = spawnZoneCollider.bounds.ClosestPoint(mousePosition);
                currentFruit.transform.position = clampedPosition;
            }
        }
    }

    void ReleaseFruit()
    {
        if (currentFruit != null)
        {
            if (IsPositionInForbiddenZone(currentFruit.transform.position) || !IsPositionInSpawnZone(currentFruit.transform.position))
            {
                Debug.Log("Impossible de déposer le fruit dans la zone interdite ou hors de la zone de spawn.");
                return;
            }

            Collider2D collider = currentFruit.GetComponent<Collider2D>();
            if (collider != null)
            {
                collider.enabled = true;
            }

            Rigidbody2D rb = currentFruit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }

            currentFruit = null;
            SpawnNextFruit();
        }

        isDragging = false;
    }

    void SpawnNextFruit()
    {
        if (nextFruit != null)
        {
            Destroy(nextFruit); // Supprimer le fruit actuel, s'il existe
        }

        int randomIndex = GetFruitIndexByRarity(); // Choisir un fruit en fonction de la probabilité
        Vector3 spawnPosition = nextFruitPosition.position;

        nextFruit = Instantiate(fruits[randomIndex], spawnPosition, Quaternion.identity);

        // Désactiver immédiatement l'AudioSource pour empêcher tout son
        AudioSource audioSource = nextFruit.GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.enabled = false;
        }

        Rigidbody2D rb = nextFruit.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic; // Empêcher la chute du fruit au début
        }

        Collider2D collider = nextFruit.GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false; // Désactiver la collision au début
        }
    }

    int GetFruitIndexByRarity()
    {
        float totalChance = 0f;

        // Calculer la somme des probabilités
        foreach (float rarity in fruitRarity)
        {
            totalChance += rarity;
        }

        float randomChance = Random.Range(0f, totalChance); // Choisir un nombre aléatoire entre 0 et la somme des chances
        float cumulativeChance = 0f;

        // Choisir un fruit basé sur les probabilités cumulées
        for (int i = 0; i < fruitRarity.Length; i++)
        {
            cumulativeChance += fruitRarity[i];
            if (randomChance <= cumulativeChance)
            {
                return i;
            }
        }

        return 0; // Retourner le premier fruit si quelque chose va mal
    }

    bool IsPositionInForbiddenZone(Vector3 position)
    {
        return forbiddenZoneCollider.bounds.Contains(position);
    }

    bool IsPositionInSpawnZone(Vector3 position)
    {
        return spawnZoneCollider.bounds.Contains(position);
    }
}

















