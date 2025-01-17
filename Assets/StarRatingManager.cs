using UnityEngine;
using UnityEngine.UI;

public class StarRatingManager : MonoBehaviour
{
    public Image[] stars; // Assigner les images des étoiles dans l'inspecteur
    public float maxTime = 60f; // Temps maximum pour obtenir 3 étoiles
    public float minTime = 10f; // Temps minimum pour obtenir 1 étoile

    public void UpdateStarRating(float remainingTime)
    {
        int starCount = CalculateStars(remainingTime);
        Debug.Log("Calculated stars: " + starCount); // Debug log

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].gameObject.SetActive(i < starCount); // Activer/désactiver les GameObjects des étoiles
        }

        // Forcer la mise à jour du canvas si nécessaire
        Canvas.ForceUpdateCanvases();
    }

    public int CalculateStars(float timeRemaining)
    {
        int stars = 0;

        if (timeRemaining > 45)
        {
            stars = 3; // 3 étoiles si plus de 45 secondes restantes
        }
        else if (timeRemaining > 30)
        {
            stars = 2; // 2 étoiles si entre 31 et 45 secondes
        }
        else if (timeRemaining > 10)
        {
            stars = 1; // 1 étoile si entre 11 et 30 secondes
        }
        else
        {
            stars = 0; // 0 étoile si 10 secondes ou moins
        }

        Debug.Log("Time remaining: " + timeRemaining + ", Stars awarded: " + stars); // Debug log
        return stars;
    }
}





