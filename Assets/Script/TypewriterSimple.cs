using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TypewriterSimple : MonoBehaviour
{
    public Text textComponent; // Référence au composant texte UI
    public float typingSpeed = 0.05f; // Vitesse d'écriture
    private string fullText;

    void Start()
    {
        fullText = textComponent.text; // Récupère le texte de base
        textComponent.text = ""; // Vide le texte
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        foreach (char letter in fullText)
        {
            textComponent.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}

