using UnityEngine;

public class FruitLogic : MonoBehaviour
{
    public GameObject nextFruitPrefab; // Le fruit sup�rieur apr�s fusion
    public int points = 10; // Points attribu�s pour la fusion de ce fruit
    private bool isMerging = false; // Emp�che les fusions multiples simultan�es
    public GameObject fusionVFXPrefab; // Le VFX � afficher lors de la fusion (Animation de sprite)
    public GameObject temporaryPrefab; // Le prefab temporaire � afficher lors de la fusion
    public float temporaryPrefabLifetime = 2f; // Temps de vie du prefab temporaire
    public AudioSource fusionAudioSource; // L'AudioSource pour jouer le son de fusion

    private const string multiFruitTag = "MultiFruit";

    private void Start()
    {
        if (fusionAudioSource == null)
        {
            fusionAudioSource = GetComponent<AudioSource>();
            if (fusionAudioSource == null)
            {
                Debug.LogError("Aucun AudioSource assign� ou trouv� sur cet objet !");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isMerging && collision.gameObject.layer == this.gameObject.layer)
        {
            FruitLogic otherFruit = collision.gameObject.GetComponent<FruitLogic>();

            if (otherFruit != null && !otherFruit.isMerging)
            {
                if (this.CompareTag(multiFruitTag))
                {
                    EvolveAndDestroy(otherFruit);
                }
                else if (this.CompareTag(otherFruit.gameObject.tag))
                {
                    Merge(otherFruit);
                }
            }
        }
    }

    private void Merge(FruitLogic otherFruit)
    {
        isMerging = true;
        otherFruit.isMerging = true;

        if (nextFruitPrefab != null)
        {
            GameObject newFruit = Instantiate(nextFruitPrefab, transform.position, Quaternion.identity);
            Collider2D newFruitCollider = newFruit.GetComponent<Collider2D>();
            if (newFruitCollider != null)
            {
                newFruitCollider.enabled = true;
            }

            Rigidbody2D rb = newFruit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
        }

        // Appel au GameManager pour ajouter les points
        GameManagerFixed gameManager = FindObjectOfType<GameManagerFixed>();
        if (gameManager != null)
        {
            gameManager.AddScore(points + otherFruit.points);
        }

        // Ajouter un effet visuel de fusion
        if (fusionVFXPrefab != null)
        {
            GameObject vfx = Instantiate(fusionVFXPrefab, transform.position, Quaternion.identity);
            Destroy(vfx, 1f);
        }

        // Faire appara�tre le prefab temporaire
        if (temporaryPrefab != null)
        {
            GameObject temp = Instantiate(temporaryPrefab, transform.position, Quaternion.identity);
            Destroy(temp, temporaryPrefabLifetime);
        }

        // Jouer le son de fusion
        if (fusionAudioSource != null)
        {
            fusionAudioSource.Play();
        }

        // D�truire les deux fruits initiaux
        Destroy(otherFruit.gameObject);
        Destroy(gameObject);
    }

    private void EvolveAndDestroy(FruitLogic otherFruit)
    {
        if (otherFruit != null && otherFruit.nextFruitPrefab != null)
        {
            GameObject evolvedFruit = Instantiate(otherFruit.nextFruitPrefab, otherFruit.transform.position, Quaternion.identity);

            Destroy(otherFruit.gameObject);

            if (fusionVFXPrefab != null)
            {
                GameObject vfx = Instantiate(fusionVFXPrefab, otherFruit.transform.position, Quaternion.identity);
                Destroy(vfx, 1f);
            }

            if (temporaryPrefab != null)
            {
                GameObject temp = Instantiate(temporaryPrefab, otherFruit.transform.position, Quaternion.identity);
                Destroy(temp, temporaryPrefabLifetime);
            }

            if (fusionAudioSource != null)
            {
                fusionAudioSource.Play();
            }

            GameManagerFixed gameManager = FindObjectOfType<GameManagerFixed>();
            if (gameManager != null)
            {
                gameManager.AddScore(otherFruit.points);
            }

            Destroy(gameObject);
        }
    }
}
