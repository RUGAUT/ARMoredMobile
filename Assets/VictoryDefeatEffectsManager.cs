using UnityEngine;
using System.Collections;

public class VictoryDefeatEffectsManager : MonoBehaviour
{
    public ParticleSystem victoryVFX; // Effet de particules pour la victoire
    public ParticleSystem defeatVFX; // Effet de particules pour la défaite
    public AudioClip victorySound; // Son de victoire
    public AudioClip defeatSound; // Son de défaite
    public AudioSource audioSource; // Source audio pour jouer les sons
    public FruitFusionGameManager gameManager; // Référence au GameManager du jeu

    public float effectDelay = 2f; // Temps avant d'afficher l'UI après les effets
    public float pauseDelay = 2f; // Temps avant de mettre le jeu en pause après la victoire/défaite

    public Transform effectsPosition; // Point où les effets de particules apparaîtront

    private bool hasPlayedVictorySound = false; // Pour vérifier si le son de victoire a déjà été joué
    private bool hasPlayedDefeatSound = false; // Pour vérifier si le son de défaite a déjà été joué

    private Vector3 victoryVFXOriginalPosition; // Position originale du VFX de victoire
    private Vector3 defeatVFXOriginalPosition; // Position originale du VFX de défaite

    void Start()
    {
        // Si aucune source audio n'est assignée, essayez d'en trouver une sur le même GameObject
        if (!audioSource)
        {
            audioSource = GetComponent<AudioSource>();
        }

        // Stocker les positions originales des effets de particules
        if (victoryVFX != null)
        {
            victoryVFXOriginalPosition = victoryVFX.transform.position;
        }

        if (defeatVFX != null)
        {
            defeatVFXOriginalPosition = defeatVFX.transform.position;
        }
    }

    // Méthode pour jouer les effets de victoire
    public void PlayVictoryEffects()
    {
        StartCoroutine(PlayVictoryEffectsCoroutine());
    }

    // Méthode pour jouer les effets de défaite
    public void PlayDefeatEffects()
    {
        StartCoroutine(PlayDefeatEffectsCoroutine());
    }

    // Coroutine pour gérer les effets de victoire
    private IEnumerator PlayVictoryEffectsCoroutine()
    {
        Debug.Log("Début des effets de victoire");

        // Déplacer et jouer le VFX de victoire
        if (victoryVFX != null)
        {
            victoryVFX.transform.position = effectsPosition.position; // Déplacer à la position souhaitée
            victoryVFX.Play();
            Debug.Log("Victory VFX joué");
        }

        // Jouer le son de victoire une seule fois
        if (victorySound != null && !hasPlayedVictorySound)
        {
            audioSource.PlayOneShot(victorySound);
            hasPlayedVictorySound = true; // Marquer que le son a été joué
            Debug.Log("Victory Sound joué");
        }

        // Attendre avant d'afficher le panel de victoire
        yield return new WaitForSeconds(effectDelay);
        Debug.Log("EffectDelay terminé");

        // Afficher l'interface de victoire
        if (gameManager != null && gameManager.winUI != null)
        {
            gameManager.winUI.SetActive(true);
        }

        // Attendre le délai configuré avant de mettre le jeu en pause
        yield return new WaitForSeconds(pauseDelay);
        Debug.Log("PauseDelay terminé");

        // Mettre le jeu en pause
        Time.timeScale = 0;
        Debug.Log("Jeu mis en pause");

        // Restaurer la position originale du VFX de victoire
        if (victoryVFX != null)
        {
            victoryVFX.transform.position = victoryVFXOriginalPosition;
        }
    }

    // Coroutine pour gérer les effets de défaite
    private IEnumerator PlayDefeatEffectsCoroutine()
    {
        Debug.Log("Début des effets de défaite");

        // Déplacer et jouer le VFX de défaite
        if (defeatVFX != null)
        {
            defeatVFX.transform.position = effectsPosition.position; // Déplacer à la position souhaitée
            defeatVFX.Play();
            Debug.Log("Defeat VFX joué");
        }

        // Jouer le son de défaite une seule fois
        if (defeatSound != null && !hasPlayedDefeatSound)
        {
            audioSource.PlayOneShot(defeatSound);
            hasPlayedDefeatSound = true; // Marquer que le son a été joué
            Debug.Log("Defeat Sound joué");
        }

        // Attendre avant d'afficher le panel de défaite
        yield return new WaitForSeconds(effectDelay);
        Debug.Log("EffectDelay terminé");

        // Afficher l'interface de défaite
        if (gameManager != null && gameManager.gameOverUI != null)
        {
            gameManager.gameOverUI.SetActive(true);
        }

        // Attendre le délai configuré avant de mettre le jeu en pause
        yield return new WaitForSeconds(pauseDelay);
        Debug.Log("PauseDelay terminé");

        // Mettre le jeu en pause
        Time.timeScale = 0;
        Debug.Log("Jeu mis en pause");

        // Restaurer la position originale du VFX de défaite
        if (defeatVFX != null)
        {
            defeatVFX.transform.position = defeatVFXOriginalPosition;
        }
    }

    // Méthode pour réinitialiser les états des sons (utile si vous rechargez la scène ou recommencez le jeu)
    public void ResetSoundStates()
    {
        hasPlayedVictorySound = false;
        hasPlayedDefeatSound = false;
        Debug.Log("États des sons réinitialisés");
    }
}

