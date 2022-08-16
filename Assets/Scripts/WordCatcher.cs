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
                    t = 0; // to terminate the loop if the elements of matched word is equal to desired word
                    p = 1; q = 1; // to determine the next letter's position on x/y axis 
                    for (int j = 1; j < sceneLoader.theWord.Length; j++)
                    {
                        if (tableLetters[i + p + (q-1)*7].GetComponent<Text>().text[0] == sceneLoader.theWord[j])
                        { // detect the next matched letter on x-axis
                            tempWord1 += sceneLoader.theWord[j];
                            p += 1; t++;
                        }
                        else if (tableLetters[i + p - 1 + q*7].GetComponent<Text>().text[0] == sceneLoader.theWord[j])
                        { // detect the next matched letter on y-axis
                            tempWord1 += sceneLoader.theWord[j];
                            q += 1; t++;
                        }
                        else 
                            break;
                    }
                    if (t >= sceneLoader.theWord.Length - 1) // this can cause problem, check it
                        break;
                }
            }

            desiredWord1 = sceneLoader.theWord[0].ToString();
            for (int i = 0; i < tempWord1.Length; i++) // tempword1 -> sceneLoader.theWord
                desiredWord1 += tempWord1[i]; // index bound error fix it

            //print(desiredWord1);

            if (desiredWord1 == sceneLoader.theWord)
            {
                print("You Win!");
                swipeController.isFinished = true; // deactivate this for experimental pusposes
            }           
        }
    }
}