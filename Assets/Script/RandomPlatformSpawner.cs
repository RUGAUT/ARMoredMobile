using System.Collections;
using UnityEngine;

public class DynamicPlatformSpawner : MonoBehaviour
{
    [Header("Zone de spawn")]
    public Vector2 spawnAreaMin; // Coordonn�es du coin inf�rieur gauche de la zone
    public Vector2 spawnAreaMax; // Coordonn�es du coin sup�rieur droit de la zone

    [Header("Param�tres des plateformes")]
    public GameObject platformPrefab; // Prefab de la plateforme � instancier
    public float platformLifetime = 3f; // Temps avant que la plateforme disparaisse
    public float respawnDelay = 1f; // Temps avant qu'une nouvelle plateforme r�apparaisse

    private GameObject currentPlatform; // R�f�rence � la plateforme active

    private void Start()
    {
        StartCoroutine(SpawnAndRespawnPlatform());
    }

    private IEnumerator SpawnAndRespawnPlatform()
    {
        while (true)
        {
            // Si une plateforme existe, on la d�truit
            if (currentPlatform != null)
            {
                Destroy(currentPlatform);
                yield return new WaitForSeconds(respawnDelay); // Pause avant de r�appara�tre
            }

            // G�n�rer une nouvelle position al�atoire dans la zone
            Vector2 spawnPosition = GenerateRandomPosition();

            // Instancier une nouvelle plateforme
            currentPlatform = Instantiate(platformPrefab, spawnPosition, Quaternion.identity);

            // Attendre la dur�e de vie avant de la faire dispara�tre
            yield return new WaitForSeconds(platformLifetime);
        }
    }

    private Vector2 GenerateRandomPosition()
    {
        float x = Random.Range(spawnAreaMin.x, spawnAreaMax.x);
        float y = Random.Range(spawnAreaMin.y, spawnAreaMax.y);
        return new Vector2(x, y);
    }

    private void OnDrawGizmosSelected()
    {
        // Dessiner la zone de spawn dans l'�diteur
        Gizmos.color = new Color(0, 1, 0, 0.3f); // Vert semi-transparent
        Vector2 center = (spawnAreaMin + spawnAreaMax) / 2;
        Vector2 size = spawnAreaMax - spawnAreaMin;
        Gizmos.DrawCube(center, size); // Zone remplie

        Gizmos.color = Color.green; // Contour vert
        Gizmos.DrawWireCube(center, size);
    }
}



