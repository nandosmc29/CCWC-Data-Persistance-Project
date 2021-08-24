using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif
using TMPro;

public class MenuScript : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }

    public void StartNew()
    {
        GameObject.Find("ScenePersistent").GetComponent<ScenePersistentData>().playerName = inputName.text;
        SceneManager.LoadScene(1);
    }
}
