using UnityEngine;
using System.Collections;

public class VictoryDefeatEffectsManager : MonoBehaviour
{
    public ParticleSystem victoryVFX; // Effet de particules pour la victoire
    public ParticleSystem defeatVFX; // Effet de particules pour la d�faite
    public AudioClip victorySound; // Son de victoire
    public AudioClip defeatSound; // Son de d�faite
    public AudioSource audioSource; // Source audio pour jouer les sons
    public FruitFusionGameManager gameManager; // R�f�rence au GameManager du jeu

    public float effectDelay = 2f; // Temps avant d'afficher l'UI apr�s les effets
    public float pauseDelay = 2f; // Temps avant de mettre le jeu en pause apr�s la victoire/d�faite

    public Transform effectsPosition; // Point o� les effets de particules appara�tront

    private bool hasPlayedVictorySound = false; // Pour v�rifier si le son de victoire a d�j� �t� jou�
    private bool hasPlayedDefeatSound = false; // Pour v�rifier si le son de d�faite a d�j� �t� jou�

    private Vector3 victoryVFXOriginalPosition; // Position originale du VFX de victoire
    private Vector3 defeatVFXOriginalPosition; // Position originale du VFX de d�faite

    void Start()
    {
        // Si aucune source audio n'est assign�e, essayez d'en trouver une sur le m�me GameObject
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

    // M�thode pour jouer les effets de victoire
    public void PlayVictoryEffects()
    {
        StartCoroutine(PlayVictoryEffectsCoroutine());
    }

    // M�thode pour jouer les effets de d�faite
    public void PlayDefeatEffects()
    {
        StartCoroutine(PlayDefeatEffectsCoroutine());
    }

    // Coroutine pour g�rer les effets de victoire
    private IEnumerator PlayVictoryEffectsCoroutine()
    {
        Debug.Log("D�but des effets de victoire");

        // D�placer et jouer le VFX de victoire
        if (victoryVFX != null)
        {
            victoryVFX.transform.position = effectsPosition.position; // D�placer � la position souhait�e
            victoryVFX.Play();
            Debug.Log("Victory VFX jou�");
        }

        // Jouer le son de victoire une seule fois
        if (victorySound != null && !hasPlayedVictorySound)
        {
            audioSource.PlayOneShot(victorySound);
            hasPlayedVictorySound = true; // Marquer que le son a �t� jou�
            Debug.Log("Victory Sound jou�");
        }

        // Attendre avant d'afficher le panel de victoire
        yield return new WaitForSeconds(effectDelay);
        Debug.Log("EffectDelay termin�");

        // Afficher l'interface de victoire
        if (gameManager != null && gameManager.winUI != null)
        {
            gameManager.winUI.SetActive(true);
        }

        // Attendre le d�lai configur� avant de mettre le jeu en pause
        yield return new WaitForSeconds(pauseDelay);
        Debug.Log("PauseDelay termin�");

        // Mettre le jeu en pause
        Time.timeScale = 0;
        Debug.Log("Jeu mis en pause");

        // Restaurer la position originale du VFX de victoire
        if (victoryVFX != null)
        {
            victoryVFX.transform.position = victoryVFXOriginalPosition;
        }
    }

    // Coroutine pour g�rer les effets de d�faite
    private IEnumerator PlayDefeatEffectsCoroutine()
    {
        Debug.Log("D�but des effets de d�faite");

        // D�placer et jouer le VFX de d�faite
        if (defeatVFX != null)
        {
            defeatVFX.transform.position = effectsPosition.position; // D�placer � la position souhait�e
            defeatVFX.Play();
            Debug.Log("Defeat VFX jou�");
        }

        // Jouer le son de d�faite une seule fois
        if (defeatSound != null && !hasPlayedDefeatSound)
        {
            audioSource.PlayOneShot(defeatSound);
            hasPlayedDefeatSound = true; // Marquer que le son a �t� jou�
            Debug.Log("Defeat Sound jou�");
        }

        // Attendre avant d'afficher le panel de d�faite
        yield return new WaitForSeconds(effectDelay);
        Debug.Log("EffectDelay termin�");

        // Afficher l'interface de d�faite
        if (gameManager != null && gameManager.gameOverUI != null)
        {
            gameManager.gameOverUI.SetActive(true);
        }

        // Attendre le d�lai configur� avant de mettre le jeu en pause
        yield return new WaitForSeconds(pauseDelay);
        Debug.Log("PauseDelay termin�");

        // Mettre le jeu en pause
        Time.timeScale = 0;
        Debug.Log("Jeu mis en pause");

        // Restaurer la position originale du VFX de d�faite
        if (defeatVFX != null)
        {
            defeatVFX.transform.position = defeatVFXOriginalPosition;
        }
    }

    // M�thode pour r�initialiser les �tats des sons (utile si vous rechargez la sc�ne ou recommencez le jeu)
    public void ResetSoundStates()
    {
        hasPlayedVictorySound = false;
        hasPlayedDefeatSound = false;
        Debug.Log("�tats des sons r�initialis�s");
    }
}

