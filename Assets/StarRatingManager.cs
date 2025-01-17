using UnityEngine;
using UnityEngine.UI;

public class StarRatingManager : MonoBehaviour
{
    public Image[] stars; // Assigner les images des �toiles dans l'inspecteur
    public float maxTime = 60f; // Temps maximum pour obtenir 3 �toiles
    public float minTime = 10f; // Temps minimum pour obtenir 1 �toile

    public void UpdateStarRating(float remainingTime)
    {
        int starCount = CalculateStars(remainingTime);
        Debug.Log("Calculated stars: " + starCount); // Debug log

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].gameObject.SetActive(i < starCount); // Activer/d�sactiver les GameObjects des �toiles
        }

        // Forcer la mise � jour du canvas si n�cessaire
        Canvas.ForceUpdateCanvases();
    }

    public int CalculateStars(float timeRemaining)
    {
        int stars = 0;

        if (timeRemaining > 45)
        {
            stars = 3; // 3 �toiles si plus de 45 secondes restantes
        }
        else if (timeRemaining > 30)
        {
            stars = 2; // 2 �toiles si entre 31 et 45 secondes
        }
        else if (timeRemaining > 10)
        {
            stars = 1; // 1 �toile si entre 11 et 30 secondes
        }
        else
        {
            stars = 0; // 0 �toile si 10 secondes ou moins
        }

        Debug.Log("Time remaining: " + timeRemaining + ", Stars awarded: " + stars); // Debug log
        return stars;
    }
}





