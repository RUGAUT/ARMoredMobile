using UnityEngine;
using static LevelManager;

public class LevelButtonsUI : MonoBehaviour
{
    public static LevelButtonsUI Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            //Destroy(gameObject); // Destroy this duplicate instance
            return;
        }
        Instance = this;
    }

    public LevelButton[] levelButtons;
    private void Start()
    {
        Debug.Log("levels button in level ui" + levelButtons);
    }
}
