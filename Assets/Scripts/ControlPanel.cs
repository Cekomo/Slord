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

    private GameObject[] letters;
    private GameObject[] tableLetters;

    private Vector2 startPos;
    private bool isFound; // checks if specified letter is found in letters array

    public GameObject wordBar; // represents text of word bar
    public GameObject scoreBar; // represents text of score board
    public GameObject levelBar; // represents level of the game
    public GameObject wordMatched; // represents popping up of word matched

    private int zeroAdd; // to add zero for each missing number on score board
    private float floatScore; // assign the score as float from string
    private int totalScore; // total score gaining from each level
    private bool scoreBool; // boolean to block multiple additions to the total score

    private string tableWord; // the letters needed to match the word
    private int i; // temprorary variable representing index
    private string tempWord; // temporary word to detetct missing letters on the table

    void Start()
    {
        tableLetters = GameObject.FindGameObjectsWithTag("Letter");

        wordMatched.SetActive(false);
        scoreBool = true;

        wordBar.GetComponent<Text>().text = sceneLoader.theWord;
        SetLevel(sceneLoader.activeScene);

        // to set the initial score shown in board
        scoreBar.GetComponent<Text>().text = (100 * sceneLoader.theWord.Length).ToString(); // to determine interface score      
        ScoreSetter();

        // for experimental purposes
        //PlayerPrefs.SetInt("TotalScore", 0);
        //PlayerPrefs.Save();
    }

    void Update()
    {
        //if (Input.GetMouseButton(0)) // PC
        if (Input.touchCount > 0) // Mobile
        {
            // decrease the score with each swipe operation leter by letter
            // ------------------------------------
            floatScore = float.Parse(scoreBar.GetComponent<Text>().text);
            if (floatScore >= 20 * sceneLoader.theWord.Length) // it has problem (not stop at %20 percent)
                floatScore = floatScore - swipeController.pointDecrement;

            swipeController.pointDecrement = 0;
            scoreBar.GetComponent<Text>().text = floatScore.ToString();

            // ------------------------------------

            ScoreSetter(); // to adjust the score board 
        }

        if (swipeController.isFinished && scoreBool) // show total score when the level is completed until going next level
        {
            sceneLoader.SaveLevel();

            totalScore = PlayerPrefs.GetInt("TotalScore");
            totalScore += (int)floatScore;
            //print(totalScore);
            PlayerPrefs.SetInt("TotalScore", totalScore);
            PlayerPrefs.Save();
            scoreBar.GetComponent<Text>().text = totalScore.ToString();

            scoreBool = false;
            ScoreSetter(); // to adjust the score board 

            wordMatched.SetActive(true);
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
            levelBar.GetComponent<Text>().text = "Level 0" + (level + 1).ToString();
        else
            levelBar.GetComponent<Text>().text = "Level " + (level + 1).ToString();
    }

    void ScoreSetter()
    {
        zeroAdd = 6 - scoreBar.GetComponent<Text>().text.Length;
        for (int i = 0; i < zeroAdd; i++)
            scoreBar.GetComponent<Text>().text = "0" + scoreBar.GetComponent<Text>().text;
    }

    public void HintLetter()
    {
        letters = GameObject.FindGameObjectsWithTag("Letter");

        // to receive the needed letters inside the table
        tableWord = "";
        for (int j = 0; j < sceneLoader.theWord.Length; j++)
        {
            isFound = true; i = 0;
            while (isFound && i < letters.Length)
            {
                startPos = letters[i].transform.position;
                if (letters[i].GetComponent<Text>().text == sceneLoader.theWord[j].ToString() && startPos.x > 20 && startPos.x < 1050 && startPos.y > 295 && startPos.y < 1495)
                {
                    tableWord += letters[i].GetComponent<Text>().text;
                    isFound = false; 
                }
                i++;
            }
        }
        print(tableWord);

        tempWord = "";
        for (int i = 0; i < sceneLoader.theWord.Length; i++)
        {
            isFound = true;
            for (int j = 0; j < tableWord.Length; j++)
                if (sceneLoader.theWord[i] == tableWord[j])
                    isFound = false;
            if (isFound)
                tempWord += sceneLoader.theWord[i];
        }

        print(tempWord);
            
    }
}
