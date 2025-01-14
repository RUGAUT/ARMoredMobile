using UnityEngine;

public class MoveBetweenPoints : MonoBehaviour
{
    [Header("Points de d�placement")]
    public Transform pointA; // Point de d�part
    public Transform pointB; // Point d'arriv�e

    [Header("Param�tres de mouvement")]
    [Range(0.1f, 10f)] public float speed = 2f; // Vitesse de d�placement
    public bool loop = true; // Revenir au point A apr�s avoir atteint le point B

    private Vector3 targetPosition; // Position cible actuelle
    private bool movingToB = true; // Indique si l'objet se d�place vers B

    void Start()
    {
        if (pointA == null || pointB == null)
        {
            Debug.LogError("Les points A et B doivent �tre d�finis !");
            return;
        }

        targetPosition = pointB.position; // Initialisation
    }

    void Update()
    {
        if (pointA == null || pointB == null) return;

        // D�placement vers la position cible
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // V�rification si l'objet atteint la position cible
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            if (loop)
            {
                // Alterne entre les points
                movingToB = !movingToB;
                targetPosition = movingToB ? pointB.position : pointA.position;
            }
        }
    }

    // Dessiner les points A et B dans l'�diteur
    void OnDrawGizmos()
    {
        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(pointA.position, 0.1f);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(pointB.position, 0.1f);

            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(pointA.position, pointB.position);
        }
    }
}

