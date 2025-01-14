using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    public Collider2D upperCollider; // Collider sup�rieur (trigger)
    public Collider2D lowerCollider; // Collider inf�rieur (trigger pour la zone de Game Over)
    public float disableDuration = 1.0f; // Temps pendant lequel le collider inf�rieur est d�sactiv�
    public LayerMask fruitLayer; // Layer des fruits

    private bool isLowerColliderDisabled = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        // V�rifie si l'objet entrant appartient au layer des fruits
        if (((1 << collision.gameObject.layer) & fruitLayer) != 0)
        {
            Debug.Log("Fruit d�tect� dans la zone sup�rieure!");

            // D�sactive temporairement le collider inf�rieur
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
            lowerCollider.enabled = false; // D�sactive le collider inf�rieur
            Debug.Log("Zone de Game Over d�sactiv�e temporairement.");
        }

        yield return new WaitForSeconds(disableDuration); // Attendre la dur�e d�finie

        if (lowerCollider != null)
        {
            lowerCollider.enabled = true; // R�active le collider inf�rieur
            Debug.Log("Zone de Game Over r�activ�e.");
        }

        isLowerColliderDisabled = false;
    }
}


