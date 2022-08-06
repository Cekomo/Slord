using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// note that sceneLoader is to not load different scene, it loads the words when the word matched correctly
public class SceneLoader : MonoBehaviour
{
    public SwipeController swipeController;

    // levelWords are the array that specifies the correct word should be matched per level 
    // increasing the array means increasing levels at the same time
    private string[] levelWords =
    { 
        "DIET", "POUR", "BIND", "ARMY", "OVEN", "SOUL", "WHIP", "DOME", "TRIP", "SIGH", "HERO",
        "CHASE", "ROUTE", "FRAME", "ASSET", "HONOR", "STEAK", "SPITE", "GUESS", "RANGE", "FRESH",
        "TRUNK", "EJECT", "STUFF", "SHOCK", "SWARM", "PATCH", "PLUCK", "STYLE", "WITCH", "THICK",
        "TISSUE", "JOCKEY", "GOSSIP", "CHANGE", "DOLLAR", "DOUBLE", "FAMILY", "UPDATE", "COLONY",
        "RECORD", "SODIUM", "THESIS", "LIKELY", "SYMBOL", "SLEEVE", "REWARD", "BOTTOM", "MIRROR", 
        "EXCLUDE", "RETREAT", "STUMBLE", "PYRAMID", "CRUSADE", "MONSTER", "PENALTY", "THIRSTY",
        "**VICTORY**"
    };

    [HideInInspector] public string theWord; // the word should be matched to pass the level determined by level index

    [HideInInspector] public int activeScene; // to determine and save the index of the scene
    private int temp; // temproray variable for scene number

    void Awake()
    {
        // to adjust the fps as 60
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        // to reset the level
        //PlayerPrefs.SetInt("ActiveScene", 23);
        //PlayerPrefs.Save();

        // the place allows maximum of 9 letters for word (not wide letters included)
        activeScene = PlayerPrefs.GetInt("ActiveScene");
        theWord = levelWords[activeScene];
    }

    public void SceneLoad()
    {
        if (swipeController.isFinished && activeScene < 57)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            activeScene = PlayerPrefs.GetInt("ActiveScene");
        }       
        //SceneManager.LoadScene(activeScene, LoadSceneMode.Single);
    }

    public void SaveLevel()
    {
        if (swipeController.isFinished && activeScene < 57)
        {
            temp = PlayerPrefs.GetInt("ActiveScene");
            PlayerPrefs.SetInt("ActiveScene", activeScene + 1);
            PlayerPrefs.Save();
        }
    }

}
