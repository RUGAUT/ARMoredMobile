using UnityEngine;

public class FruitBehaviour : MonoBehaviour
{
    public GameObject nextFruitPrefab; // Le fruit supérieur après fusion
    public int points = 10; // Points attribués pour la fusion de ce fruit
    private bool isMerging = false; // Empêche les fusions multiples simultanées
    public GameObject fusionVFXPrefab; // Le VFX à afficher lors de la fusion (Animation de sprite)
    public AudioSource fusionAudioSource; // L'AudioSource pour jouer le son de fusion
    public GameObject pointsDisplayPrefab; // Le prefab à afficher pour indiquer les points gagnés
    public float pointsDisplayHeight = 1.0f; // La hauteur d'apparition du GameObject affichant les points

    private const string multiFruitTag = "MultiFruit";

    private void Start()
    {
        if (fusionAudioSource == null)
        {
            fusionAudioSource = GetComponent<AudioSource>();
            if (fusionAudioSource == null)
            {
                Debug.LogError("Aucun AudioSource assigné ou trouvé sur cet objet !");
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isMerging && collision.gameObject.layer == this.gameObject.layer)
        {
            FruitBehaviour otherFruit = collision.gameObject.GetComponent<FruitBehaviour>();

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

    private void Merge(FruitBehaviour otherFruit)
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
            var _gameManager = GameObject.Find("GameManager");
            var fusionManager = GameObject.Find("FusionManager");
            if (_gameManager != null && _gameManager.activeSelf)
            {
                _gameManager.GetComponent<GameManager>().DisplayVfx(newFruit);
            }
            else
            {
                fusionManager.GetComponent<FruitFusionGameManager>().DisplayVfx(newFruit);
            }
            
            
            
            Rigidbody2D rb = newFruit.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
        }

        GameManager gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.AddScore(points + otherFruit.points);
        }

        if (fusionVFXPrefab != null)
        {
            GameObject vfx = Instantiate(fusionVFXPrefab, transform.position, Quaternion.identity);
            Animator vfxAnimator = vfx.GetComponent<Animator>();
            if (vfxAnimator != null)
            {
                vfxAnimator.Play("FusionAnimation");
            }
            Destroy(vfx, 1f);
        }

        if (fusionAudioSource != null)
        {
            fusionAudioSource.Play();
        }

        // Ajouter l'affichage des points
        if (pointsDisplayPrefab != null)
        {
            Vector3 displayPosition = transform.position + new Vector3(0, pointsDisplayHeight, 0);
            GameObject pointsDisplay = Instantiate(pointsDisplayPrefab, displayPosition, Quaternion.identity);
            TextMesh textMesh = pointsDisplay.GetComponent<TextMesh>();
            if (textMesh != null)
            {
                textMesh.text = $"+{points + otherFruit.points}";
            }
            Destroy(pointsDisplay, 1.5f); // Détruire l'affichage après 1,5 seconde
        }
        

        Destroy(otherFruit.gameObject);
        Destroy(gameObject);
    }

    private void EvolveAndDestroy(FruitBehaviour otherFruit)
    {
        if (otherFruit != null && otherFruit.nextFruitPrefab != null)
        {
            GameObject evolvedFruit = Instantiate(otherFruit.nextFruitPrefab, otherFruit.transform.position, Quaternion.identity);

            Destroy(otherFruit.gameObject);

            if (fusionVFXPrefab != null)
            {
                GameObject vfx = Instantiate(fusionVFXPrefab, otherFruit.transform.position, Quaternion.identity);
                Animator vfxAnimator = vfx.GetComponent<Animator>();
                if (vfxAnimator != null)
                {
                    vfxAnimator.Play("FusionAnimation");
                }
                Destroy(vfx, 1f);
            }

            if (fusionAudioSource != null)
            {
                fusionAudioSource.Play();
            }

            var fusionManager = GameObject.Find("FusionManager");
            fusionManager.GetComponent<FruitFusionGameManager>().DisplayVfx(evolvedFruit);

            Debug.Log("tes");
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.AddScore(otherFruit.points);
            }

            Destroy(gameObject);
        }
    }
}

























