using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordCatcher : MonoBehaviour
{
    private Vector2 letterPos;
    private Vector2 letterPosX;
    private GameObject[] letters; // represents all letters in the system
    private GameObject[] tableLetters; // represents all letters in the table
    private string theWord1 = "AMK"; // the word needed to be found in table
    private string desiredWord1; // the word that is constructed by user

    private GameObject nextLetterX; // next letter of the spotted letter in X-axis
    private GameObject nextLetterY; // next letter of the spotted letter in Y-axis

    private int k; // increment to add elements in tableLetters array
    private int t; // increment to add elements in text letter array
    //private Text letterText; // text of the specified letter (temp)

    void Start()
    {
        letters = GameObject.FindGameObjectsWithTag("Letter");
        // try to find a way to eliminate letters out of scope (tableLetters)
        tableLetters = GameObject.FindGameObjectsWithTag("Letter");
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0)) // check if it causes any problem
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

            for (int k = 0; k < t; k++)
                if (tableLetters[k].GetComponent<Text>().text[0] == theWord1[0])
                {
                    letterPos = tableLetters[k].transform.position; 
                    for (int j = 0; j < t; j++)
                    {
                        letterPosX = tableLetters[j].transform.position;
                        if (tableLetters[j].GetComponent<Text>().text[0] == theWord1[1] && letterPosX.x < letterPos.x+155 && letterPosX.x > letterPos.x+145 && letterPos.y+5 > letterPosX.y && letterPos.y-5 < letterPosX.y)
                            desiredWord1 = theWord1[0].ToString() + theWord1[1].ToString();
                        else if (tableLetters[j].GetComponent<Text>().text[0] == theWord1[1] && letterPosX.y > letterPos.y-160 && letterPosX.y < letterPos.y-150 && letterPos.x+5 > letterPosX.x && letterPos.x-5 < letterPosX.x)
                            desiredWord1 = theWord1[0].ToString() + theWord1[1].ToString();
                    }
                }
            print(desiredWord1);
            desiredWord1 = null;
        }
        // to inspect table letters
        //for (int i = 0; i < 56; i++)
        //    print(tableLetters[i]);
    }
    
}
