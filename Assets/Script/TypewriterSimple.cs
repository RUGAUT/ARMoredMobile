using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TypewriterSimple : MonoBehaviour
{
    public Text textComponent; // R�f�rence au composant texte UI
    public float typingSpeed = 0.05f; // Vitesse d'�criture
    private string fullText;

    void Start()
    {
        fullText = textComponent.text; // R�cup�re le texte de base
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

