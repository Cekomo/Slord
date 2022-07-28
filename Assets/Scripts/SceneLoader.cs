using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public SwipeController swipeController;

    // levelWords are the array that specifies the correct word should be matched per level 
    private string[] levelWords = { "GOAT", "PIXAR", "SCENE" };
    [HideInInspector] public string theWord; // the word should be matched to pass the level determined by level index

    private int currentScene; // to determine and save the index of the scene
    //private int loadScene; // variable determined by activeScene to load a scene

    void Start()
    {
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 60;

        currentScene = SceneManager.GetActiveScene().buildIndex;
        theWord = levelWords[currentScene];
        print(theWord);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && swipeController.isFinished)
            SceneLoad();

    }

    public void SceneLoad()
    {
        //loadScene = PlayerPrefs.GetInt("ActiveScene");
        SceneManager.LoadScene(currentScene+1, LoadSceneMode.Single);
    }
}
