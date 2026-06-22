using System.Collections;
using UnityEngine;

public class BlinkUntilFruitLimit : MonoBehaviour
{
    public float blinkInterval = 0.1f;
    public int fruitEntryLimit = 3;

    private SpriteRenderer spriteRenderer;
    private bool isBlinking = true;
    private int fruitCount = 0;

    void Start()
    {
        // 🔥 cherche aussi sur les enfants (plus robuste)
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        }

        if (spriteRenderer == null)
        {
            Debug.LogWarning(
                "SpriteRenderer introuvable sur " + gameObject.name +
                " → script désactivé"
            );

            enabled = false;
            return;
        }

        StartCoroutine(Blink());
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Fruit"))
        {
            fruitCount++;

            if (fruitCount >= fruitEntryLimit)
            {
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator Blink()
    {
        while (isBlinking && spriteRenderer != null)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }

        if (spriteRenderer != null)
            spriteRenderer.enabled = true;
    }

    public void StopBlinking()
    {
        isBlinking = false;

        if (spriteRenderer != null)
            spriteRenderer.enabled = true;
    }
}