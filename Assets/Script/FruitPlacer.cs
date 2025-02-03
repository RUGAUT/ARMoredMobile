using UnityEngine;

public class FruitPlacer : MonoBehaviour
{
    public GameObject[] fruits; // Liste des pr�fabs de fruits
    private GameObject currentFruit; // Fruit actuellement s�lectionn�
    private GameObject nextFruit; // Le prochain fruit qui va appara�tre
    private bool isDragging = false;
    public Transform nextFruitPosition; // Position o� afficher le prochain fruit
    public Collider2D forbiddenZoneCollider; // Zone o� les fruits ne peuvent pas �tre plac�s
    public Collider2D spawnZoneCollider; // Zone o� les fruits peuvent appara�tre
    public Camera mainCamera; // La cam�ra principale

    [Header("Difficulty Settings")]
    public DifficultyManager difficultyManager; // R�f�rence au script DifficultyManager
    private float nextFruitSpawnTimer; // Minuteur pour le prochain fruit

    [Header("Cooldown Settings")]
    public float fruitCooldown = 1.0f; // Temps de cooldown entre les placements de fruits
    private float fruitCooldownTimer = 0f; // Minuteur pour le cooldown

    [Header("Fruit Rarity Settings")]
    [Range(0f, 1f)] public float[] fruitRarity; // Probabilit�s pour chaque fruit d'appara�tre (somme totale doit �tre 1)

    void Start()
    {
        // Initialiser le prochain fruit
        SpawnNextFruit();

        // Initialiser le minuteur en fonction de la difficult�
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
        // R�initialiser les variables et le comportement des fruits
        currentFruit = null;
        nextFruit = null;

        // R�initialiser le prochain fruit
        SpawnNextFruit();
    }

    void HandleInput()
    {
        if (Input.GetMouseButtonDown(0) && fruitCooldownTimer <= 0) // V�rifier si le cooldown est termin�
        {
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            // V�rifier si le clic est sur un Collider2D donn�
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
        // Limiter la position du fruit � la zone de spawn
        Vector3 clampedPosition = ClampPositionToSpawnZone(mousePosition);

        // Instancier un fruit al�atoire � la position limit�e
        int randomIndex = GetFruitIndexByRarity();
        Instantiate(fruits[randomIndex], clampedPosition, Quaternion.identity);

        // R�initialiser le cooldown
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
        // Si aucun fruit n'est en cours, lancer un nouveau spawn apr�s un d�lai
        if (currentFruit == null && nextFruit == null)
        {
            nextFruitSpawnTimer -= Time.deltaTime;

            if (nextFruitSpawnTimer <= 0)
            {
                SpawnNextFruit();

                // R�initialiser le minuteur
                if (difficultyManager != null)
                {
                    nextFruitSpawnTimer = difficultyManager.fruitSpawnInterval;
                }
            }
        }
    }

    void HandleCooldown()
    {
        // D�cr�menter le timer de cooldown si n�cessaire
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

                // R�initialiser le cooldown
                fruitCooldownTimer = fruitCooldown;
            }
            else
            {
                Debug.Log("Position invalide, fruit non plac�.");
            }
        }
    }

    void SpawnFruitInSpawnZone()
    {
        // G�n�rer une position al�atoire dans la zone de spawn
        Bounds spawnBounds = spawnZoneCollider.bounds;
        Vector3 randomPosition = new Vector3(
            Random.Range(spawnBounds.min.x, spawnBounds.max.x),
            Random.Range(spawnBounds.min.y, spawnBounds.max.y),
            0
        );

        // Instancier un fruit al�atoire
        int randomIndex = GetFruitIndexByRarity();
        Instantiate(fruits[randomIndex], randomPosition, Quaternion.identity);

        // R�initialiser le cooldown
        fruitCooldownTimer = fruitCooldown;
    }

    void DragFruitToMouse()
    {
        if (currentFruit != null)
        {
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            // Limiter la position du fruit � la zone de spawn
            if (spawnZoneCollider.bounds.Contains(mousePosition))
            {
                // D�placez le fruit uniquement si le point est � l'int�rieur de la zone de spawn
                currentFruit.transform.position = mousePosition;
            }
            else
            {
                // Si le fruit d�passe la zone, le maintenir � la limite de la zone
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
                Debug.Log("Impossible de d�poser le fruit dans la zone interdite ou hors de la zone de spawn.");
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

        int randomIndex = GetFruitIndexByRarity(); // Choisir un fruit en fonction de la probabilit�
        Vector3 spawnPosition = nextFruitPosition.position;

        nextFruit = Instantiate(fruits[randomIndex], spawnPosition, Quaternion.identity);

        // D�sactiver imm�diatement l'AudioSource pour emp�cher tout son
        AudioSource audioSource = nextFruit.GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.enabled = false;
        }

        Rigidbody2D rb = nextFruit.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic; // Emp�cher la chute du fruit au d�but
        }

        Collider2D collider = nextFruit.GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.enabled = false; // D�sactiver la collision au d�but
        }
    }

    int GetFruitIndexByRarity()
    {
        float totalChance = 0f;

        // Calculer la somme des probabilit�s
        foreach (float rarity in fruitRarity)
        {
            totalChance += rarity;
        }

        float randomChance = Random.Range(0f, totalChance); // Choisir un nombre al�atoire entre 0 et la somme des chances
        float cumulativeChance = 0f;

        // Choisir un fruit bas� sur les probabilit�s cumul�es
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

















