using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager Instance;

    public enum Language
    {
        French,
        English,
        Spanish
    }

    public Language currentLanguage = Language.French;

    private Dictionary<string, string> frenchTexts = new();
    private Dictionary<string, string> englishTexts = new();
    private Dictionary<string, string> spanishTexts = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            if (!PlayerPrefs.HasKey("Language"))
                DetectSystemLanguage();
            else
                currentLanguage = (Language)PlayerPrefs.GetInt("Language", (int)Language.French);

            LoadTexts();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void DetectSystemLanguage()
    {
        currentLanguage = Application.systemLanguage switch
        {
            SystemLanguage.French => Language.French,
            SystemLanguage.Spanish => Language.Spanish,
            _ => Language.English
        };

        PlayerPrefs.SetInt("Language", (int)currentLanguage);
        PlayerPrefs.Save();
    }

    private void LoadTexts()
    {
        // 🎮 Texte général
        AddText("game_goal",
            "Le but du jeu est de fusionner les fruits / légumes du plus petit au plus grand.",
            "The goal of the game is to merge the fruits / vegetables from the smallest to the largest.",
            "El objetivo del juego es fusionar las frutas / verduras de la más pequeña a la más grande.");

        AddText("start_button", "Commencer", "Start", "Comenzar");

        AddText("tutorial_info",
            "Clique sur l’écran pour faire apparaître des fruits, puis fusionne ceux du même type pour obtenir les fruits demandés avant le temps imparti.",
            "Tap the screen to make fruits appear, then merge the same type to get the requested fruits before time runs out.",
            "Toca la pantalla para hacer aparecer frutas, luego fusiona las del mismo tipo para conseguir las frutas solicitadas antes de que se acabe el tiempo.");

        AddText("congratulations",
            "Félicitations ! Vous avez terminé le niveau avec succès",
            "Congratulations! You have completed the level successfully",
            "¡Felicidades! Has completado el nivel con éxito");

        AddText("retry", "Réessayer", "Retry", "Reintentar");
        AddText("continue", "Continuer", "Continue", "Continuar");

        // 🌍 Pays
        AddText("senegal", "Sénégal", "Senegal", "Senegal");
        AddText("morocco", "Maroc", "Morocco", "Marruecos");
        AddText("south_africa", "Afrique du Sud", "South Africa", "Sudáfrica");
        AddText("nigeria", "Nigeria", "Nigeria", "Nigeria");
        AddText("ivory_coast", "Côte d'Ivoire", "Ivory Coast", "Costa de Marfil");
    }

    private void AddText(string key, string fr, string en, string es)
    {
        frenchTexts[key] = fr;
        englishTexts[key] = en;
        spanishTexts[key] = es;
    }

    public string GetText(string key)
    {
        return currentLanguage switch
        {
            Language.French => frenchTexts.TryGetValue(key, out var fr) ? fr : $"[{key}]",
            Language.English => englishTexts.TryGetValue(key, out var en) ? en : $"[{key}]",
            Language.Spanish => spanishTexts.TryGetValue(key, out var es) ? es : $"[{key}]",
            _ => $"[{key}]"
        };
    }

    public void SetLanguage(Language lang)
    {
        currentLanguage = lang;
        PlayerPrefs.SetInt("Language", (int)lang);
        PlayerPrefs.Save();

        foreach (var text in FindObjectsOfType<LocalizedText>())
        {
            text.UpdateText();
        }
    }
}
