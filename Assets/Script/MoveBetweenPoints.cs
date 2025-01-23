using UnityEngine;
using System.Collections.Generic;

public class MoveBetweenPoints : MonoBehaviour
{
    [Header("Points de déplacement")]
    public List<Transform> points = new List<Transform>(); // Liste des points de déplacement

    [Header("Paramètres de mouvement")]
    [Range(0.1f, 10f)] public float speed = 2f; // Vitesse de déplacement
    public bool loop = true; // Revenir au premier point après le dernier

    private int currentPointIndex = 0; // Index du point actuel
    private Vector3 targetPosition; // Position cible actuelle

    void Start()
    {
        if (points.Count < 2)
        {
            Debug.LogError("Il faut au moins deux points dans la liste !");
            return;
        }

        targetPosition = points[currentPointIndex].position; // Initialisation
    }

    void Update()
    {
        if (points.Count < 2) return;

        // Déplacement vers la position cible
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Vérification si l'objet atteint la position cible
        if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
        {
            // Passer au point suivant
            currentPointIndex++;

            if (currentPointIndex >= points.Count)
            {
                if (loop)
                {
                    currentPointIndex = 0; // Revenir au premier point
                }
                else
                {
                    enabled = false; // Arrêter le mouvement
                    return;
                }
            }

            targetPosition = points[currentPointIndex].position;
        }
    }

    // Dessiner les points dans l'éditeur
    void OnDrawGizmos()
    {
        if (points.Count > 1)
        {
            Gizmos.color = Color.green;
            for (int i = 0; i < points.Count; i++)
            {
                Gizmos.DrawSphere(points[i].position, 0.1f);

                if (i < points.Count - 1)
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawLine(points[i].position, points[i + 1].position);
                }

                if (loop && i == points.Count - 1)
                {
                    Gizmos.DrawLine(points[i].position, points[0].position);
                }
            }
        }
    }
}


