using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharGenerator : MonoBehaviour
{
    // ** occuring of vowels must be more frequent to create more meaningful words
    // it can be done by adding multiple wovels inside the alphabet variable or mathematically
    
    // it will be english characters only for now
    //private string alphabet = "AAABCDEEEFGHIIIJKLMNOOOPQRSTUUUVWXYZ"; // letter stack to pick one of the element
    private string alphabet = "N"; // letter stack to pick one of the element
    private string l; // it is a string converted from char that contains single random letter
    private GameObject[] letters; // all letters in LetterGround

    void Start()
    {
        letters = GameObject.FindGameObjectsWithTag("Letter");

        for (int i = 0; i < letters.Length; i++)
        {
            l = alphabet[Random.Range(0, alphabet.Length)].ToString(); // takes random char from alphabet
            letters[i].GetComponent<Text>().text = l; // assign that random char to l variable as string
            //print(l);
        }
    }
}
