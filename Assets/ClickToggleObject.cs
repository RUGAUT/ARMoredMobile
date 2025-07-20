using UnityEngine;

public class ClickToggleObject : MonoBehaviour
{
    public GameObject objectToToggle;        // L'objet à afficher/masquer
    public int clicksToShow = 5;             // Nombre de clics pour afficher l'objet
    public float disappearAfterSeconds = 3f; // Temps avant disparition auto

    private int clickCount = 0;
    private bool isObjectVisible = false;
    private float timer = 0f;

    private bool hasBeenHidden = false; // ✅ Empêche la réapparition

    void Start()
    {
        if (objectToToggle != null)
        {
            objectToToggle.SetActive(false);
        }
    }

    void Update()
    {
        if (hasBeenHidden) return; // ✅ Ne rien faire si déjà disparu une fois

        if (Input.GetMouseButtonDown(0))
        {
            if (!isObjectVisible)
            {
                clickCount++;

                if (clickCount >= clicksToShow)
                {
                    ShowObject();
                }
            }
            else
            {
                if (IsPointerOverObject(objectToToggle))
                {
                    HideObject();
                }
            }
        }

        if (isObjectVisible)
        {
            timer += Time.deltaTime;
            if (timer >= disappearAfterSeconds)
            {
                HideObject();
            }
        }
    }

    void ShowObject()
    {
        if (objectToToggle != null)
        {
            objectToToggle.SetActive(true);
            isObjectVisible = true;
            timer = 0f;
        }
    }

    void HideObject()
    {
        if (objectToToggle != null)
        {
            objectToToggle.SetActive(false);
            isObjectVisible = false;
            clickCount = 0;
            timer = 0f;
            hasBeenHidden = true; // ✅ L’objet ne peut plus être affiché
        }
    }

    bool IsPointerOverObject(GameObject obj)
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D collider = obj.GetComponent<Collider2D>();
        return collider != null && collider.OverlapPoint(mousePosition);
    }
}
