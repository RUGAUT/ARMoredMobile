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
        text.text = LanguageManager.Instance.GetText(key);
    }
}
