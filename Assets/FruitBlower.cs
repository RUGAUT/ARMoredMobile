using System.Collections;
using UnityEngine;

public class FruitBlower : MonoBehaviour
{
    public LayerMask fruitLayer; // Layer des fruits
    public Vector2 blowForce = new Vector2(5f, 2f); // Force du souffle (X, Y)
    public float activeTime = 2f; // Temps d'activation
    public float inactiveTime = 2f; // Temps d'inactivité

    private Collider2D blowZone; // Collider de détection
    private bool isActive = true; // État du souffleur

    private void Start()
    {
        blowZone = GetComponent<Collider2D>();
        if (blowZone == null)
        {
            Debug.LogError("Ajoute un Collider2D (Trigger) au souffleur !");
            return;
        }
        blowZone.isTrigger = true; // S'assure que le collider est bien en trigger

        StartCoroutine(ToggleBlower()); // Démarre l'alternance entre actif/inactif
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isActive && ((1 << other.gameObject.layer) & fruitLayer) != 0)
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(blowForce, ForceMode2D.Impulse);
            }
        }
    }

    private IEnumerator ToggleBlower()
    {
        while (true)
        {
            isActive = true;
            yield return new WaitForSeconds(activeTime); // Temps d'activation

            isActive = false;
            yield return new WaitForSeconds(inactiveTime); // Temps d'inactivité
        }
    }
}

