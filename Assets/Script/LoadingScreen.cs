using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FakeLoadingScreen : MonoBehaviour
{
    public GameObject loadingScreen; // Référence au panneau d'écran de chargement
    public Slider progressBar;       // Référence au slider UI
    public float fakeLoadingDuration = 3f; // Durée du faux chargement

    void Start()
    {
        StartCoroutine(FakeLoading());
    }

    private IEnumerator FakeLoading()
    {
        // Pause le temps dans le jeu
        Time.timeScale = 0f;

        // Active l'écran de chargement
        loadingScreen.SetActive(true);

        float elapsedTime = 0f;

        while (elapsedTime < fakeLoadingDuration)
        {
            // Simule la progression
            progressBar.value = Mathf.Clamp01(elapsedTime / fakeLoadingDuration);
            elapsedTime += Time.unscaledDeltaTime; // Utilise unscaledDeltaTime pour ignorer la pause du temps
            yield return null;
        }

        // Assure que la barre est pleine
        progressBar.value = 1f;

        // Cache l'écran de chargement
        loadingScreen.SetActive(false);

        // Reprend le temps dans le jeu
        Time.timeScale = 1f;
    }
}

