using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class LevelImageManager : MonoBehaviour
{
    public ScrollRect scrollView;
    public RectTransform contentPanel;
    public GameObject imagePrefab; // Préfabriqué pour l'image
    public int totalLevels = 100; // Nombre total de niveaux
    public int interval = 10; // Intervalle pour afficher une image

    private List<GameObject> levelImages = new List<GameObject>();

    void Start()
    {
        GenerateLevelImages();
    }

    void GenerateLevelImages()
    {
        for (int i = 1; i <= totalLevels; i++)
        {
            if (i % interval == 0)
            {
                // Instancier l'image
                GameObject levelImage = Instantiate(imagePrefab, contentPanel);
                levelImages.Add(levelImage);

                // Positionner l'image dans la ScrollView
                RectTransform rectTransform = levelImage.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(0, -i * 100); // Ajustez la position selon vos besoins

                // Charger l'image depuis les ressources
                Image imageComponent = levelImage.GetComponent<Image>();
                imageComponent.sprite = Resources.Load<Sprite>($"LevelImages/Level_{i}");
            }
        }

        // Ajuster la taille du contentPanel pour contenir toutes les images
        contentPanel.sizeDelta = new Vector2(contentPanel.sizeDelta.x, totalLevels * 100);
    }
}
