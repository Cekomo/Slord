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
    
    private GameObject tempLetter; // temp variable to swap the letters regarding their positions
    private Vector2 tempLetterIndex; // temp variable to assign the index of positions to execute swap operation

    //private string theWord1; // the word needed to be found in table
    private string desiredWord1; // the word that is constructed by user
    private string tempWord1; // temp for checking created word by sliding
    //private string tempWord2; // temp for checking created word by sliding
    private int l; // to compare length of the actual word and formed one 

    private GameObject nextLetterX; // next letter of the spotted letter in X-axis
    private GameObject nextLetterY; // next letter of the spotted letter in Y-axis

    private int k; // increment to add elements in tableLetters array
    private int t; // increment to add elements in text letter array
    private int p; // to determine index of a word in for 
    private int q; // to make sure the desiredWord variable focuses on the main matched word

    //private bool isMatched = false; // to paint the matched word

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
        if (Input.GetTouch(0).phase == TouchPhase.Ended && !swipeController.isFinished)  // Mobile
        {
            k = 0;
            for (int i = 0; i < letters.Length; i++)
            {
                letterPos = letters[i].transform.position;
                // the coordinates below can be optimized
                if (letterPos.x > 20 && letterPos.x < 1050 && letterPos.y > 295 && letterPos.y < 1495)
                {   // this statement provides the algorithm to detect the letters on the table
                    tableLetters[k] = letters[i];
                    k++;
                }
            }

            // O(n^2) sorting algorithm worst case of 56*56 operations
            //for (int i = 0; i < 8; i++)
            //    for (int j = 0; j < 7; j++)
            //    {
            //        letterPos = tableLetters[i * 7 + j].transform.position;
            //        tempLetterIndex.x = Mathf.Round((letterPos.x - 90.45f) / 149.85f);
            //        tempLetterIndex.y = Mathf.Round(7 - ((letterPos.y - 310) / 155));

            //        tempLetter = tableLetters[(int)(tempLetterIndex.y * 7 + tempLetterIndex.x)]; // ideal position of current letter
            //        tableLetters[(int)(tempLetterIndex.y * 7 + tempLetterIndex.x)] = tableLetters[i * 7 + j];
            //        print(tableLetters[i * 7 + j]);
            //        tableLetters[i * 7 + j] = tempLetter; // the table letter goes its sorted position
            //        print(tableLetters[i * 7 + j]);
            //        //print(tempLetterIndex);
            //    }

            // O(n^2) sorting algorithm --> 55k operations on average, need to be optimised
            q = 0; // represents y-axis
            p = 0; // represents x-axis
            for (int i = 0; i < tableLetters.Length; i++)
            {
                for (int j = 0; j < letters.Length; j++)
                {
                    letterPos = letters[j].transform.position;
                    if (letterPos.x < (90.45f + p * 149.85f + 5) && letterPos.x > (90.45f + p * 149.85f - 5) && letterPos.y < (1395 - q * 155 + 5) && letterPos.y > (1395 - q * 155 - 5))
                    {
                        tableLetters[q * 7 + p] = letters[j]; // adjust the table letter as the letter satisfying position condition
                        break;
                    }
                }

                p++; // to adjust the index of tableLetters array
                if (p % 7 == 0)
                {
                    p = 0;
                    q++;
                }
            }

            tempWord1 = "";
            for (int i = 0; i < tableLetters.Length; i++)
            {
                if (tableLetters[i].GetComponent<Text>().text[0] == sceneLoader.theWord[0])
                {
                    for (int j = 1; j < sceneLoader.theWord.Length; j++)
                    {
                        if (tableLetters[i + j].GetComponent<Text>().text[0] == sceneLoader.theWord[j])
                            tempWord1 += sceneLoader.theWord[j];
                        else if (tableLetters[i + j*7].GetComponent<Text>().text[0] == sceneLoader.theWord[j])
                            tempWord1 += sceneLoader.theWord[j];
                        else 
                            break;
                    }
                }
            }

            desiredWord1 = sceneLoader.theWord[0].ToString();
            for (int i = 0; i < sceneLoader.theWord.Length - 1; i++)
                desiredWord1 += tempWord1[i]; // index bound error fix it

            print(desiredWord1);
            //for (int i = 0; i < tableLetters.Length; i++)
            //    print(tableLetters[i].GetComponent<Text>().text[0]);


            //for (int i = 0; i < 8; i++)
            //{
            //    tempWord1 = ""; //tempWord2 = "";
            //    for (int j = 0; j < 7; j++)
            //    {
            //        letterPos = tableLetters[i * 7 + j].transform.position;
            //        if (letterPos.y > 1395 - i * 155 - 5 && letterPos.y < 1395 - i * 155 + 5)
            //            tempWord1 += tableLetters[i * 7 + j].GetComponent<Text>().text[0];
            //    }
            //    //print(tempWord1);
            //}
        }
    }

    //    void Update()
    //    {
    //        //if (Input.GetMouseButtonUp(0) && !swipeController.isFinished) // PC check if it causes any problem
    //        if (Input.GetTouch(0).phase == TouchPhase.Ended && !swipeController.isFinished)  // Mobile
    //        {
    //            k = 0;
    //            for (int i = 0; i < letters.Length; i++)
    //            {
    //                letterPos = letters[i].transform.position;
    //                // the coordinates below can be optimized
    //                if (letterPos.x > 20 && letterPos.x < 1050 && letterPos.y > 295 && letterPos.y < 1495)
    //                {   // this statement provides the algorithm to detect the letters on the table
    //                    tableLetters[k] = letters[i];
    //                    k++;                    
    //                }
    //            }

    //            // to determine if any letters in the table found in specified word
    //            t = 0;
    //            for (int i = 0; i < 56; i++) // letter number on the table (56)
    //                for (int j = 0; j < sceneLoader.theWord.Length; j++)
    //                    if (tableLetters[i].GetComponent<Text>().text[0] == sceneLoader.theWord[j])
    //                    {
    //                        tableLetters[t] = tableLetters[i];
    //                        t++; // it is used to find next letter matched as upper boundary
    //                    }


    //            //for (int i = 0; i < t; i++) // too see the the needed letters in the table
    //            //    print(tableLetters[i].ToString() + ": " + tableLetters[i].GetComponent<Text>().text[0].ToString());

    //            // the major problem is if matched letter comes next to the another first matched letter, the word converges
    //            desiredWord1 = sceneLoader.theWord[0].ToString();
    //            for (int k = 0; k < t; k++) // check if the next letter right/below is matched with this letter wrt the word
    //            {
    //                q = 0;
    //                if (tableLetters[k].GetComponent<Text>().text[0] == sceneLoader.theWord[0])
    //                {
    //                    // for coloring the matching word
    //                    //if (isMatched) tableLetters[k].GetComponent<Text>().color = Color.green;
    //                    //else tableLetters[k].GetComponent<Text>().color = Color.black;

    //                    letterPos = tableLetters[k].transform.position;
    //                    p = 1;
    //                    for (int j = 0; j < t; j++)
    //                    {
    //                        letterPosX = tableLetters[j].transform.position;

    //                        // to determine if the next word is the one or not by looking x/y-axis
    //                        if (tableLetters[j].GetComponent<Text>().text[0] == sceneLoader.theWord[p] && letterPosX.x < letterPos.x + 155 && letterPosX.x > letterPos.x + 145 && letterPos.y + 5 > letterPosX.y && letterPos.y - 5 < letterPosX.y)
    //                        {
    //                            q++;
    //                            desiredWord1 += sceneLoader.theWord[p].ToString();
    //                            letterPos = tableLetters[j].transform.position;
    //                            //if (isMatched) tableLetters[j].GetComponent<Text>().color = Color.green;
    //                            //else tableLetters[j].GetComponent<Text>().color = Color.black;
    //                        }
    //                        else if (tableLetters[j].GetComponent<Text>().text[0] == sceneLoader.theWord[p] && letterPosX.y > letterPos.y - 160 && letterPosX.y < letterPos.y - 150 && letterPos.x + 5 > letterPosX.x && letterPos.x - 5 < letterPosX.x)
    //                        {
    //                            q++;
    //                            desiredWord1 += sceneLoader.theWord[p].ToString();
    //                            letterPos = tableLetters[j].transform.position;
    //                            //if (isMatched) tableLetters[j].GetComponent<Text>().color = Color.green;
    //                            //else tableLetters[j].GetComponent<Text>().color = Color.black;
    //                        }
    //                        // if statement is checking for next needed letter in the respective axis
    //                        if (j == t - 1 && p < sceneLoader.theWord.Length-1) { p++; j = 0; }
    //                    }
    //                    //print("1st "+desiredWord1);
    //                    //if (q < 2) // to prevent interference of the specified first letters if they get matched letter next to it 
    //                    //{
    //                    //    tempWord1 = desiredWord1; desiredWord1 = "";
    //                    //    for (int i = 0; i < tempWord1.Length - q; i++)
    //                    //        desiredWord1 += tempWord1[i].ToString();
    //                    //}
    //                }
    //            }

    //            //if (desiredWord1.Length <= sceneLoader.theWord.Length)
    //            //    l = desiredWord1.Length;
    //            //else
    //            //    l = sceneLoader.theWord.Length;
    //            //print("2nd "+desiredWord1);
    //            tempWord1 = desiredWord1;
    //            desiredWord1 = "";
    //            for (int i = 0; i < sceneLoader.theWord.Length; i++)
    //                if (tempWord1.Length >= sceneLoader.theWord.Length) // this statement is for avoiding outofrange exception
    //                    desiredWord1 += tempWord1[i];

    //            tempWord1 = "";
    //            // below codes to color the object when matched
    //            //tempWord2 = "";
    //            //for (int i = 0; i < l-1; i++)
    //            //{
    //            //    print(desiredWord1[i] + " " + sceneLoader.theWord[i]);
    //            //    tempWord1 += desiredWord1[i]; // tempWord1[i] = desiredWord1[i];
    //            //    tempWord2 += sceneLoader.theWord[i]; // tempWord2[i] = sceneLoader.theWord[i];
    //            //}

    //            //print(tempWord1.ToString() + " " + tempWord2.ToString() +" yo");
    //            //if (tempWord1 == tempWord2)
    //            //    isMatched = true;
    //            //else 
    //            //    isMatched = false;
    //            //print(desiredWord1);
    //            if (desiredWord1 == sceneLoader.theWord)
    //            {
    //                print("You Win!");
    //                swipeController.isFinished = true; // deactivate this for experimental pusposes
    //            }
    //        }
    //    }
}