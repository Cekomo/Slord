using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordCatcher : MonoBehaviour
{
    private Vector2 letterPos;
    private GameObject[] letters; // represents all letters in the system
    private GameObject[] tableLetters; // represents all letters in the table
    //private string theWord1 = "AMK"; // the word needed to be found in table

    private int k; // increment to add elements in tableLetters array

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
                {   // this statement helps the algorithm to detect the letters on the table
                    tableLetters[k] = letters[i];
                    k++;
                }
            }

            //for (int i = 0; i < 56; i++)
            //{
            //    print(tableLetters[i].Text.text); // fix this
            //}
        }
        // to inspect table letters
        //for (int i = 0; i < 56; i++)
        //    print(tableLetters[i]);
    }
    //for (int i = 0; i < letters.Length; i++)
    //    if(letters[i].name == theWord1[0])

    //}
}
