using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordCatcher : MonoBehaviour
{
    
    private GameObject[] letters; // represents all letters in the system
    private GameObject[] tableLetters; // represents all letters in the table
    //private string theWord1 = "AMK"; // the word needed to be found in table

    void Start()
    {
        letters = GameObject.FindGameObjectsWithTag("Letter");
    }

    
    void Update()
    {
        
    }
}
