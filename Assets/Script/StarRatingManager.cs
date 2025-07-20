using UnityEngine;
using UnityEngine.UI;

public class StarRatingManager : MonoBehaviour
{
    public Image[] stars; // À assigner dans l'inspecteur
    public float maxTime = 150f; // Temps maximum pour avoir 3 étoiles

    public void UpdateStarRating(float remainingTime)
    {
        int starCount = CalculateStars(remainingTime);

        for (int i = 0; i < stars.Length; i++)
        {
            stars[i].gameObject.SetActive(i < starCount);
        }

        Canvas.ForceUpdateCanvases();
    }

    public int CalculateStars(float timeRemaining)
    {
        float oneStarThreshold = maxTime / 3f;
        float twoStarThreshold = (maxTime / 3f) * 2f;

        if (timeRemaining >= maxTime)
        {
            return 3;
        }
        else if (timeRemaining >= twoStarThreshold)
        {
            return 2;
        }
        else
        {
            return 1; // Toujours au moins 1 étoile
        }
    }
}

