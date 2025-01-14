using UnityEngine;

public class LaserOnCollision : MonoBehaviour
{
    [Header("Laser Settings")]
    private LineRenderer lineRenderer; // Référence au LineRenderer attaché au GameObject

    [Header("Laser Settings")]
    public float laserHeight = 10f; // Longueur maximale du laser
    public LayerMask obstacleLayer; // Layer des colliders que le laser ne doit pas traverser

    [Header("Collider Settings")]
    public string targetTag = "Spawn"; // Tag du collider à désactiver

    private void Start()
    {
        // Récupère le LineRenderer attaché au GameObject
        lineRenderer = GetComponent<LineRenderer>();

        // Active le LineRenderer par défaut
        if (lineRenderer != null)
        {
            lineRenderer.enabled = true;
        }
    }

    private void Update()
    {
        // Si le LineRenderer est activé, mettre à jour ses positions
        if (lineRenderer.enabled)
        {
            UpdateLaserLine();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Vérifie si l'objet qui entre dans le collider a le tag spécifié
        if (collision.CompareTag(targetTag))
        {
            // Désactive le LineRenderer
            if (lineRenderer != null)
            {
                lineRenderer.enabled = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Vérifie si l'objet qui sort du collider a le tag spécifié
        if (collision.CompareTag(targetTag))
        {
            // Réactive le LineRenderer
            if (lineRenderer != null)
            {
                lineRenderer.enabled = true;
            }
        }
    }

    private void UpdateLaserLine()
    {
        if (lineRenderer != null)
        {
            // Point d'origine du laser
            Vector3 startPosition = transform.position;

            // Définir la direction du laser (vers le haut)
            Vector3 direction = Vector3.up;

            // Effectuer un Raycast pour détecter des collisions avec la layer "obstacleLayer"
            RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, laserHeight, obstacleLayer);

            if (hit.collider != null)
            {
                // Si une collision est détectée, ajuster la longueur du laser
                lineRenderer.SetPosition(0, startPosition); // Point de départ
                lineRenderer.SetPosition(1, hit.point); // Point où le laser s'arrête
            }
            else
            {
                // Si aucune collision, utiliser la longueur maximale du laser
                lineRenderer.SetPosition(0, startPosition); // Point de départ
                lineRenderer.SetPosition(1, startPosition + direction * laserHeight); // Point final
            }
        }
    }
}










