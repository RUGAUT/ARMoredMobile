using UnityEngine;

public class WheelRotation : MonoBehaviour
{
    // Vitesse de rotation de la roue
    public float rotationSpeed = 100f;

    // Définir la direction de rotation: true pour sens des aiguilles d'une montre, false pour le sens inverse
    public bool clockwise = true;

    void Update()
    {
        // Détermine la direction de rotation
        float direction = clockwise ? -1 : 1;

        // Applique la rotation à la roue
        transform.Rotate(0, 0, rotationSpeed * direction * Time.deltaTime);
    }
}

