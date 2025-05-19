using UnityEngine;

public class ClickToggleObject : MonoBehaviour
{
    public GameObject objectToToggle; // L'objet � afficher/masquer
    public int clicksToShow = 5; // Nombre de clics pour afficher l'objet
    public int clicksToHide = 3; // Nombre de clics pour le cacher

    private int clickCount = 0;
    private bool isObjectVisible = false;

    void Start()
    {
        if (objectToToggle != null)
        {
            objectToToggle.SetActive(false); // Cache l'objet au d�part
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // D�tecte un tap sur l'�cran (clic gauche pour test PC)
        {
            clickCount++;

            if (!isObjectVisible && clickCount >= clicksToShow)
            {
                ShowObject();
            }
            else if (isObjectVisible && clickCount >= clicksToShow + clicksToHide)
            {
                HideObject();
                clickCount = 0; // R�initialise le compteur apr�s avoir cach� l'objet
            }
        }
    }

    void ShowObject()
    {
        if (objectToToggle != null)
        {
            objectToToggle.SetActive(true);
            isObjectVisible = true;
        }
    }

    void HideObject()
    {
        if (objectToToggle != null)
        {
            objectToToggle.SetActive(false);
            isObjectVisible = false;
        }
    }
}

