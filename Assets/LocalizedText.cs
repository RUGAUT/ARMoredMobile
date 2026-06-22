using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class LocalizedText : MonoBehaviour
{
    public string key;
    private TMP_Text text;

    private void Awake()
    {
        text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        UpdateText();
    }

    public void UpdateText()
    {
        if (text == null)
        {
            Debug.LogWarning("TMP_Text manquant sur " + gameObject.name);
            return;
        }

        if (LanguageManager.Instance == null)
        {
            Debug.LogWarning("LanguageManager.Instance est NULL !");
            text.text = key; // fallback
            return;
        }

        string value = LanguageManager.Instance.GetText(key);

        if (string.IsNullOrEmpty(value))
        {
            text.text = key; // fallback si traduction absente
        }
        else
        {
            text.text = value;
        }
    }
}