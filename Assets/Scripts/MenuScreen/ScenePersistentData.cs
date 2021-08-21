using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePersistentData : MonoBehaviour
{
    public static ScenePersistentData Instance;
    public string playerName;

    private void Awake()
    {
        playerName = "AAA";
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

}
