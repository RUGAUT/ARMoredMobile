using UnityEngine;
using TMPro; // ✅ Nécessaire pour TMP_Dropdown

public class LanguageDropdown : MonoBehaviour
{
    public TMP_Dropdown languageDropdown;

    void Start()
    {
        if (languageDropdown.options.Count == 0)
        {
            languageDropdown.options.Clear();
            languageDropdown.options.Add(new TMP_Dropdown.OptionData("Français"));
            languageDropdown.options.Add(new TMP_Dropdown.OptionData("English"));
        }

        languageDropdown.value = (int)LanguageManager.Instance.currentLanguage;
        languageDropdown.RefreshShownValue();

        languageDropdown.onValueChanged.AddListener(OnLanguageChanged);
    }

    private void OnLanguageChanged(int index)
    {
        LanguageManager.Language selectedLanguage = (LanguageManager.Language)index;
        LanguageManager.Instance.SetLanguage(selectedLanguage);
    }
}
