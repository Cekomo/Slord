using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharGenerator : MonoBehaviour
{
    // ** occuring of vowels must be more frequent to create more meaningful words
    // it can be done by adding multiple wovels inside the alphabet variable or mathematically

    // it will be english characters only for now
    //private string alphabet = "AABCDEEFGHIIJKLMNOOPQRSTUUVWXYZ"; // letter stack to pick one of the element
    private string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; // letter stack to pick one of the element
    //private string alphabet = "M"; // to control other functions rather than charGenerator

    private string l; // it is a string converted from char that contains single random letter
    private GameObject[] letters; // all letters in LetterGround
    private int k = 0;

    void Awake() // random char generation should be inside Awake() function due to the priority 
    {
        letters = GameObject.FindGameObjectsWithTag("Letter");

        for (int i = 0; i < letters.Length; i++)
        {
            //l = alphabet[Random.Range(0, alphabet.Length)].ToString(); // takes random char from alphabet
            //letters[i].GetComponent<Text>().text = l; // assign that random char to l variable as string
                                                      ////print(l);

            //if (i%2 == 0)           
            //    letters[i].GetComponent<Text>().text = "A";
            //else
            //    letters[i].GetComponent<Text>().text = "M";

            //if (i % 7 == 0)

            letters[i].GetComponent<Text>().text = alphabet[k].ToString();
            k++;
            if (k == 25)
                k = 0;
        }
    }
}
