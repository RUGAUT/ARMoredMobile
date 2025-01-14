using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    public Collider2D upperCollider; // Collider supérieur (trigger)
    public Collider2D lowerCollider; // Collider inférieur (trigger pour la zone de Game Over)
    public float disableDuration = 1.0f; // Temps pendant lequel le collider inférieur est désactivé
    public LayerMask fruitLayer; // Layer des fruits

    private bool isLowerColliderDisabled = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Vérifie si l'objet entrant appartient au layer des fruits
        if (((1 << collision.gameObject.layer) & fruitLayer) != 0)
        {
            Debug.Log("Fruit détecté dans la zone supérieure!");

            // Désactive temporairement le collider inférieur
            if (!isLowerColliderDisabled)
            {
                StartCoroutine(DisableLowerCollider());
            }
        }
    }

    private System.Collections.IEnumerator DisableLowerCollider()
    {
        isLowerColliderDisabled = true;

        if (lowerCollider != null)
        {
            lowerCollider.enabled = false; // Désactive le collider inférieur
            Debug.Log("Zone de Game Over désactivée temporairement.");
        }

        yield return new WaitForSeconds(disableDuration); // Attendre la durée définie

        if (lowerCollider != null)
        {
            lowerCollider.enabled = true; // Réactive le collider inférieur
            Debug.Log("Zone de Game Over réactivée.");
        }

        isLowerColliderDisabled = false;
    }
}


