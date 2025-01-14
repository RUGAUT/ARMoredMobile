using UnityEngine;

public class LaserOnCollision : MonoBehaviour
{
    [Header("Laser Settings")]
    private LineRenderer lineRenderer; // R�f�rence au LineRenderer attach� au GameObject

    [Header("Laser Settings")]
    public float laserHeight = 10f; // Longueur maximale du laser
    public LayerMask obstacleLayer; // Layer des colliders que le laser ne doit pas traverser

    [Header("Collider Settings")]
    public string targetTag = "Spawn"; // Tag du collider � d�sactiver

    private void Start()
    {
        // R�cup�re le LineRenderer attach� au GameObject
        lineRenderer = GetComponent<LineRenderer>();

        // Active le LineRenderer par d�faut
        if (lineRenderer != null)
        {
            lineRenderer.enabled = true;
        }
    }

    private void Update()
    {
        // Si le LineRenderer est activ�, mettre � jour ses positions
        if (lineRenderer.enabled)
        {
            UpdateLaserLine();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // V�rifie si l'objet qui entre dans le collider a le tag sp�cifi�
        if (collision.CompareTag(targetTag))
        {
            // D�sactive le LineRenderer
            if (lineRenderer != null)
            {
                lineRenderer.enabled = false;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // V�rifie si l'objet qui sort du collider a le tag sp�cifi�
        if (collision.CompareTag(targetTag))
        {
            // R�active le LineRenderer
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

            // D�finir la direction du laser (vers le haut)
            Vector3 direction = Vector3.up;

            // Effectuer un Raycast pour d�tecter des collisions avec la layer "obstacleLayer"
            RaycastHit2D hit = Physics2D.Raycast(startPosition, direction, laserHeight, obstacleLayer);

            if (hit.collider != null)
            {
                // Si une collision est d�tect�e, ajuster la longueur du laser
                lineRenderer.SetPosition(0, startPosition); // Point de d�part
                lineRenderer.SetPosition(1, hit.point); // Point o� le laser s'arr�te
            }
            else
            {
                // Si aucune collision, utiliser la longueur maximale du laser
                lineRenderer.SetPosition(0, startPosition); // Point de d�part
                lineRenderer.SetPosition(1, startPosition + direction * laserHeight); // Point final
            }
        }
    }
}










