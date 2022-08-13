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

    private GameObject[] upperReflector; // reflectors placed upper side
    private GameObject[] lowerReflector; // reflectors placed lower side
    private GameObject[] leftReflector; // reflectors placed left side
    private GameObject[] rightReflector; // reflectors placed right side

    private float timePassed; // clock to fade the hint reflector

    private Vector2 letterPos; // to determine the position of letter

    private Vector2 startPos;
    private bool isFound; // checks if specified letter is found in letters array
    private bool startFading = false; // to initiate fading process

    public GameObject wordBar; // represents text of word bar
    public GameObject scoreBar; // represents text of score board
    public GameObject levelBar; // represents level of the game
    public GameObject wordMatched; // represents popping up of word matched

    private int zeroAdd; // to add zero for each missing number on score board
    private float floatScore; // assign the score as float from string
    private int totalScore; // total score gaining from each level
    private bool scoreBool; // boolean to block multiple additions to the total score

    private string tableWord; // the letters needed to match the word
    private int i; // temporary variable representing index
    private string tempWord; // temporary word to detetct missing letters on the table
    private bool isRepeated; // to determine if any letter is repeated in the word

    void Start()
    {
        tableLetters = GameObject.FindGameObjectsWithTag("Letter");
        
        upperReflector = GameObject.FindGameObjectsWithTag("UpperReflector");
        lowerReflector = GameObject.FindGameObjectsWithTag("LowerReflector");
        leftReflector = GameObject.FindGameObjectsWithTag("LeftReflector");
        rightReflector = GameObject.FindGameObjectsWithTag("RightReflector");

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
            if (floatScore > 20 * sceneLoader.theWord.Length) 
                floatScore = floatScore - swipeController.pointDecrement;
            else if (floatScore < 20 * sceneLoader.theWord.Length)
                floatScore = 20 * sceneLoader.theWord.Length;

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

        if (startFading)
        {
            timePassed += Time.deltaTime;
            if (timePassed > 6f)
            {
                for (int i = 0; i < upperReflector.Length; i++)
                {
                    upperReflector[i].GetComponent<Image>().color = Color.black;
                    lowerReflector[i].GetComponent<Image>().color = Color.black;
                }
                for (int i = 0; i < leftReflector.Length; i++)
                {
                    leftReflector[i].GetComponent<Image>().color = Color.black;
                    rightReflector[i].GetComponent<Image>().color = Color.black;
                }
                startFading = false;
                timePassed = 0f;
            }
        }
    }

    public void ResetTheGame()
    {
        if (!swipeController.isFinished)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        
        totalScore = PlayerPrefs.GetInt("TotalScore");
        PlayerPrefs.SetInt("TotalScore", totalScore - 75);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void SetLevel(int level)
    {
        if (level+1 < 10) // add "0" to make the level more convenient in the screen
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
        if (!startFading) // boolean is used for activity of hint button as well as fading mechanism
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
                        letters[i].GetComponent<Text>().text += "."; // to distinguish the letters which are repreated
                        isFound = false;
                    }
                    i++;
                }
            }
            print(tableWord);

            i = 0; // to represent index of char array 
            for (int j = 0; j < letters.Length; j++)
            {
                if (letters[j].GetComponent<Text>().text.Length == 2) // to prevent OutOfRange error
                    if (letters[j].GetComponent<Text>().text[1].ToString() == ".") // revert the text of letters by removing the dot
                    {
                        letters[j].GetComponent<Text>().text = letters[j].GetComponent<Text>().text[0].ToString();
                    }
            }

            // this for loop may need some work 
            // it is responsible from detecting the word letters not present on the table
            tempWord = ""; 
            for (int j = 0; j < sceneLoader.theWord.Length; j++)
            {
                i = 0;
                for (int t = 0; t < sceneLoader.theWord.Length; t++)
                    if (t != j && sceneLoader.theWord[j] == sceneLoader.theWord[t])
                        i++;
                
                isFound = true;
                for (int k = 0; k < tableWord.Length; k++)
                {
                    if (sceneLoader.theWord[j] == tableWord[k] && i <= 0)
                        isFound = false;
                    else if (sceneLoader.theWord[j] == tableWord[k] && i > 0)
                        i--;
                }
                if (isFound)
                    tempWord += sceneLoader.theWord[j];
            }
            print(tempWord.Length);

            if (tempWord.Length > 0 && floatScore > 20 * sceneLoader.theWord.Length) 
            {
                floatScore -= 75f;
                scoreBar.GetComponent<Text>().text = floatScore.ToString();
                ScoreSetter(); // to adjust the score board 

                // decrease the total point by 75 for each hint 
                //totalScore = PlayerPrefs.GetInt("TotalScore"); 
                //PlayerPrefs.SetInt("TotalScore", totalScore - 75);
            }

            // to light the respective area of black area with blue by considering the position of missing letter
            i = 0; isFound = false; 
            while (i < tempWord.Length && !isFound)
            {
                for (int j = 0; j < letters.Length; j++)
                    if (tempWord[i].ToString() == letters[j].GetComponent<Text>().text)
                    {
                        letterPos = letters[j].transform.position;

                        if (letterPos.x > 990)
                            for (int k = 0; k < rightReflector.Length; k++)
                            {
                                if (letterPos.y < (k * -155 + 1395) + 10 && letterPos.y > (k * -155 + 1395) - 10)
                                    rightReflector[k].GetComponent<Image>().color = Color.yellow;
                                isFound = true;
                            }
                        else if (letterPos.x < 90)
                            for (int k = 0; k < leftReflector.Length; k++)
                            {
                                if (letterPos.y < (k * -155 + 1395) + 10 && letterPos.y > (k * -155 + 1395) - 10)
                                    leftReflector[k].GetComponent<Image>().color = Color.yellow;
                                isFound = true;
                            }
                        else if (letterPos.y > 1395)
                            for (int k = 0; k < upperReflector.Length; k++)
                            {
                                if (letterPos.x < (k * 149.85f + 90.45f) + 10 && letterPos.x > (k * 149.85f + 90.45f) - 10)
                                    upperReflector[k].GetComponent<Image>().color = Color.yellow;
                                isFound = true;
                            }
                        else if (letterPos.y < 310)
                            for (int k = 0; k < lowerReflector.Length; k++)
                            {
                                if (letterPos.x < (k * 149.85f + 90.45f) + 10 && letterPos.x > (k * 149.85f + 90.45f) - 10)
                                    lowerReflector[k].GetComponent<Image>().color = Color.yellow;
                                isFound = true;
                            }
                        if (isFound)
                            break;
                    }
                i++;
            }
            startFading = true;
        }
    }
    
}
