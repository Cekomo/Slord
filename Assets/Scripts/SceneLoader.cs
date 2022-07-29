using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public SwipeController swipeController;

    // levelWords are the array that specifies the correct word should be matched per level 
    // increasing the array means increasing levels at the same time
    private string[] levelWords = { "GOAT", "PIXAR", "SCAM", "BOOTY", "PARADISE", "MILKY" };
    [HideInInspector] public string theWord; // the word should be matched to pass the level determined by level index

    [HideInInspector] public int activeScene; // to determine and save the index of the scene
    private int temp;
    //private int currentScene; // to determine and save the index of the scene
    //private int loadScene; // variable determined by activeScene to load a scene

    void Awake()
    {
        // to adjust the fps as 60
        //QualitySettings.vSyncCount = 0;
        //Application.targetFrameRate = 60;
        
        // PlayerPrefs.SetInt("ActiveScene", 0); // to reset the level
        // the place allows maximum of 9 letters for word (not wide letters included)
        activeScene = PlayerPrefs.GetInt("ActiveScene");
        theWord = levelWords[activeScene];
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && swipeController.isFinished)
        {
            SaveLevel();
            SceneLoad();
        }


    }

    public void SceneLoad()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        activeScene = PlayerPrefs.GetInt("ActiveScene");
        //SceneManager.LoadScene(activeScene, LoadSceneMode.Single);
    }

    public void SaveLevel()
    {
        temp = PlayerPrefs.GetInt("ActiveScene");
        PlayerPrefs.SetInt("ActiveScene", activeScene+1);
        PlayerPrefs.Save();
    }

}
