using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordCatcher : MonoBehaviour
{
    public SwipeController swipeController; 

    private Vector2 letterPos; // (temp) position of the letter 
    private Vector2 letterPosX; // (temp) position of the letter to build desired word 
    private GameObject[] letters; // represents all letters in the system
    private GameObject[] tableLetters; // represents all letters in the table
    
    private string theWord1 = "HELLO"; // the word needed to be found in table
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
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0) && !swipeController.isFinished) // check if it causes any problem
        {
            k = 0;
            for (int i = 0; i < letters.Length; i++)
            {
                letterPos = letters[i].transform.position;
                // the coordinates below can be optimized
                if (letterPos.x < 1060 && letterPos.x > 20 && letterPos.y < 1480 && letterPos.y > 250)
                {   // this statement provides the algorithm to detect the letters on the table
                    tableLetters[k] = letters[i];
                    k++;
                }
            }

            t = 0;
            for (int i = 0; i < 56; i++) // letter number on the table (56)
                for (int j = 0; j < theWord1.Length; j++)
                    if (tableLetters[i].GetComponent<Text>().text[0] == theWord1[j])
                    {
                        tableLetters[t] = tableLetters[i];
                        t++;
                    }

            //for (int i = 0; i < t; i++) // too see the the needed letters in the table
            //    print(tableLetters[i].ToString() + ": " + tableLetters[i].GetComponent<Text>().text[0].ToString());
            desiredWord1 = theWord1[0].ToString();
            
            for (int k = 0; k < t; k++)
                if (tableLetters[k].GetComponent<Text>().text[0] == theWord1[0])
                {
                    letterPos = tableLetters[k].transform.position;
                    p = 1;
                    for (int j = 0; j < t; j++)
                    {
                        letterPosX = tableLetters[j].transform.position;

                        // to determine if the next word is the one or not by looking x/y-axis
                        if (tableLetters[j].GetComponent<Text>().text[0] == theWord1[p] && letterPosX.x < letterPos.x+155 && letterPosX.x > letterPos.x+145 && letterPos.y+5 > letterPosX.y && letterPos.y-5 < letterPosX.y)
                        {
                            desiredWord1 += theWord1[p].ToString();
                            letterPos = tableLetters[j].transform.position;
                        }
                        else if (tableLetters[j].GetComponent<Text>().text[0] == theWord1[p] && letterPosX.y > letterPos.y-160 && letterPosX.y < letterPos.y-150 && letterPos.x+5 > letterPosX.x && letterPos.x-5 < letterPosX.x)
                        {
                            desiredWord1 += theWord1[p].ToString();
                            letterPos = tableLetters[j].transform.position;
                        }
                        // if statement is checking for next needed letter in the respective axis
                        if (j == t-1 && p < theWord1.Length-1) { p++; j = 0; }
                    }
                } 
            
            tempWord1 = desiredWord1;
            print(tempWord1);
            desiredWord1 = "";
            for (int i = 0; i < theWord1.Length; i++)
                if (tempWord1.Length >= theWord1.Length) // this statement is for avoiding outofrange exception
                    desiredWord1 += tempWord1[i];
            
            if (desiredWord1 == theWord1) 
            {
                print("You Win!");
                swipeController.isFinished = true; // deactivate this for experimental pusposes
            }    
        }
    }
}
