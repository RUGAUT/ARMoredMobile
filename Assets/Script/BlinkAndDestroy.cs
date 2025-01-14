using System.Collections;
using UnityEngine;

public class BlinkUntilFruitLimit : MonoBehaviour
{
    public float blinkInterval = 0.1f; // Intervalle entre chaque clignotement
    public int fruitEntryLimit = 3; // Nombre maximum de fruits autorisés avant destruction

    private SpriteRenderer spriteRenderer;
    private bool isBlinking = true;
    private int fruitCount = 0;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("No SpriteRenderer found on the GameObject.");
            return;
        }

        // Démarre le clignotement dès le début
        StartCoroutine(Blink());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Fruit"))
        {
            fruitCount++;
            if (fruitCount >= fruitEntryLimit)
            {
                Destroy(gameObject); // Détruit l'objet après avoir atteint la limite
            }
        }
    }

    private IEnumerator Blink()
    {
        while (isBlinking)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }

        spriteRenderer.enabled = true; // Assure que l'objet est visible à la fin
    }

    public void StopBlinking()
    {
        isBlinking = false;
        spriteRenderer.enabled = true;
    }
}



