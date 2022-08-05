using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// there is a problem about catching the word
// it does not catch the word when there is a letter inside the word 
//..is near to one of the letters fix this

// the lack of catching can be dependent of single letter; for instance 
//..when "A" causes problem, swapping that "A" with other letter fixes the problem

public class WordCatcher : MonoBehaviour
{
    public SwipeController swipeController;
    public SceneLoader sceneLoader;

    private Vector2 letterPos; // (temp) position of the letter 
    private Vector2 letterPosX; // (temp) position of the letter to build desired word 
    private GameObject[] letters; // represents all letters in the system
    private GameObject[] tableLetters; // represents all letters in the table

    //private string theWord1; // the word needed to be found in table
    private string desiredWord1; // the word that is constructed by user
    private string tempWord1; // temp for checking created word by sliding

    private GameObject nextLetterX; // next letter of the spotted letter in X-axis
    private GameObject nextLetterY; // next letter of the spotted letter in Y-axis

    private int k; // increment to add elements in tableLetters array
    private int t; // increment to add elements in text letter array
    private int p; //to determine index of a word in for 

    void Start()
    {

        letters = GameObject.FindGameObjectsWithTag("Letter");
        // try to find a way to eliminate letters out of scope (tableLetters)
        tableLetters = GameObject.FindGameObjectsWithTag("Letter");

        //for (int i = 0; i < letters.Length; i++)
        //    print(letters[i].ToString() + " X: " + letters[i].transform.position.x.ToString() + " Y: " + letters[i].transform.position.y.ToString());
    }

    void Update()
    {
        //if (Input.GetMouseButtonUp(0) && !swipeController.isFinished) // PC check if it causes any problem
        if (Input.GetTouch(0).phase == TouchPhase.Ended && !swipeController.isFinished)  // Mobile
        {
            k = 0;
            for (int i = 0; i < letters.Length; i++)
            {
                letterPos = letters[i].transform.position;
                // the coordinates below can be optimized
                if (letterPos.x < 995 && letterPos.x > 85 && letterPos.y < 1380 && letterPos.y > 285)
                {   // this statement provides the algorithm to detect the letters on the table
                    tableLetters[k] = letters[i];
                    k++;
                }
            }

            t = 0;
            for (int i = 0; i < 56; i++) // letter number on the table (56)
                for (int j = 0; j < sceneLoader.theWord.Length; j++)
                    if (tableLetters[i].GetComponent<Text>().text[0] == sceneLoader.theWord[j])
                    {
                        tableLetters[t] = tableLetters[i];
                        t++;
                    }
          

            //for (int i = 0; i < t; i++) // too see the the needed letters in the table
            //    print(tableLetters[i].ToString() + ": " + tableLetters[i].GetComponent<Text>().text[0].ToString());
            desiredWord1 = sceneLoader.theWord[0].ToString();

            for (int k = 0; k < t; k++)
                if (tableLetters[k].GetComponent<Text>().text[0] == sceneLoader.theWord[0])
                {
                    letterPos = tableLetters[k].transform.position;
                    p = 1;
                    for (int j = 0; j < t; j++)
                    {
                        letterPosX = tableLetters[j].transform.position;

                        // to determine if the next word is the one or not by looking x/y-axis
                        if (tableLetters[j].GetComponent<Text>().text[0] == sceneLoader.theWord[p] && letterPosX.x < letterPos.x + 105 && letterPosX.x > letterPos.x + 115 && letterPos.y + 4 > letterPosX.y && letterPos.y - 4 < letterPosX.y)
                        {
                            desiredWord1 += sceneLoader.theWord[p].ToString();
                            letterPos = tableLetters[j].transform.position;
                        }
                        else if (tableLetters[j].GetComponent<Text>().text[0] == sceneLoader.theWord[p] && letterPosX.y > letterPos.y - 120 && letterPosX.y < letterPos.y - 110 && letterPos.x + 4 > letterPosX.x && letterPos.x - 4 < letterPosX.x)
                        {
                            desiredWord1 += sceneLoader.theWord[p].ToString();
                            letterPos = tableLetters[j].transform.position;
                        }
                        // if statement is checking for next needed letter in the respective axis
                        if (j == t - 1 && p < sceneLoader.theWord.Length - 1) { p++; j = 0; }
                    }
                }

            tempWord1 = desiredWord1;
            print(tempWord1);
            desiredWord1 = "";
            for (int i = 0; i < sceneLoader.theWord.Length; i++)
                if (tempWord1.Length >= sceneLoader.theWord.Length) // this statement is for avoiding outofrange exception
                    desiredWord1 += tempWord1[i];

            if (desiredWord1 == sceneLoader.theWord)
            {
                print("You Win!");
                swipeController.isFinished = true; // deactivate this for experimental pusposes
            }
        }
    }
}