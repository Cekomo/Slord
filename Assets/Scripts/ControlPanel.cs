using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// this class is to control buttons and texts in the blue region of the screen
public class ControlPanel : MonoBehaviour
{
    public SceneLoader sceneLoader;
    public SwipeController swipeController;

    private GameObject[] interfaces;

    private string score; // abbreviation of score interface
    private int zeroAdd; // to add zero for each missing number on score board
    private float floatScore; // assign the score as float from string

    void Start()
    {
        // each interface represents specific text field or button
        interfaces = GameObject.FindGameObjectsWithTag("InterfaceText");
        // interfaces[0] = TheWorldBar, interfaces[1] = Score, interfaces[2] = LevelNumber
        interfaces[0].GetComponent<Text>().text = sceneLoader.theWord;
        SetLevel(sceneLoader.activeScene);
        
        // to set the initial score shown in board
        interfaces[1].GetComponent<Text>().text = (100 * sceneLoader.theWord.Length).ToString(); // to determine interface score      
        score = interfaces[1].GetComponent<Text>().text;
        zeroAdd = 6 - score.Length;
        for (int i = 0; i < zeroAdd; i++)
            score = "0" + score;        
        interfaces[1].GetComponent<Text>().text = score;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            // decrease the score with each swipe operation leter by letter
            // ------------------------------------
            floatScore = float.Parse(interfaces[1].GetComponent<Text>().text);
            floatScore = floatScore - swipeController.pointDecrement;
            swipeController.pointDecrement = 0;
            interfaces[1].GetComponent<Text>().text = floatScore.ToString();
            // ------------------------------------

            // to adjust the score board repeatedly
            // ------------------------------------
            zeroAdd = 6 - interfaces[1].GetComponent<Text>().text.Length;
            for (int i = 0; i < zeroAdd; i++)
                interfaces[1].GetComponent<Text>().text = "0" + interfaces[1].GetComponent<Text>().text;
            // ------------------------------------
            
        }
    }
    
    public void ResetTheGame()
    {
        if (!swipeController.isFinished)
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
