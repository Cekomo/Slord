using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// this class is to control buttons and texts in the blue region of the screen
public class ControlPanel : MonoBehaviour
{
    public SceneLoader sceneLoader;

    private GameObject[] interfaces;

    void Start()
    {
        // each interface represents specific text field or button
        interfaces = GameObject.FindGameObjectsWithTag("InterfaceText");
        // interfaces[0] = TheWorldBar, interfaces[1] = Score, interfaces[2] = LevelNumber
        interfaces[0].GetComponent<Text>().text = sceneLoader.theWord;
        SetLevel(sceneLoader.activeScene);
    }

    void Update()
    {
        
    }
    
    public void ResetTheGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void SetLevel(int level)
    {
        if (level < 10) // add "0" to make the level more convenient in the screen
            interfaces[2].GetComponent<Text>().text = "Level 0" + (level+1).ToString();
        else
            interfaces[2].GetComponent<Text>().text = "Level " + (level + 1).ToString();
    }

    /*

     public void OpenPanel()
    {
        if(HowToPanel != null)
        {
            Time.timeScale = 0;

            HowToPanel.SetActive(true);
            Panel.SetActive(false);
            HelpPanel.SetActive(false);
        }
        
    }
    public void OpenPanell()
    {
        if (Panel != null)
        {
            Time.timeScale = 1;

            //txt.text = "?";
            HowToPanel.SetActive(false);
            Panel.SetActive(true);
            HelpPanel.SetActive(true);
        }
        
    } 
    */
}
