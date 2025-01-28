using UnityEngine;

public class PrefabDataSaver : MonoBehaviour
{
    public string prefabName; // Nom unique pour identifier les données de ce prefab

    void Start()
    {
        // Charger les données enregistrées si elles existent
        if (PlayerPrefs.HasKey(prefabName + "_posX"))
        {
            float posX = PlayerPrefs.GetFloat(prefabName + "_posX");
            float posY = PlayerPrefs.GetFloat(prefabName + "_posY");
            float posZ = PlayerPrefs.GetFloat(prefabName + "_posZ");

            float rotX = PlayerPrefs.GetFloat(prefabName + "_rotX");
            float rotY = PlayerPrefs.GetFloat(prefabName + "_rotY");
            float rotZ = PlayerPrefs.GetFloat(prefabName + "_rotZ");
            float rotW = PlayerPrefs.GetFloat(prefabName + "_rotW");

            // Restaurer la position et la rotation
            transform.position = new Vector3(posX, posY, posZ);
            transform.rotation = new Quaternion(rotX, rotY, rotZ, rotW);
        }
    }

    void OnDestroy()
    {
        // Sauvegarder la position et la rotation
        PlayerPrefs.SetFloat(prefabName + "_posX", transform.position.x);
        PlayerPrefs.SetFloat(prefabName + "_posY", transform.position.y);
        PlayerPrefs.SetFloat(prefabName + "_posZ", transform.position.z);

        PlayerPrefs.SetFloat(prefabName + "_rotX", transform.rotation.x);
        PlayerPrefs.SetFloat(prefabName + "_rotY", transform.rotation.y);
        PlayerPrefs.SetFloat(prefabName + "_rotZ", transform.rotation.z);
        PlayerPrefs.SetFloat(prefabName + "_rotW", transform.rotation.w);

        // S'assurer que les données sont sauvegardées immédiatement
        PlayerPrefs.Save();
    }
}

