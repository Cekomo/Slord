using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// for now, direction change and ball forward movement work together
// try to seperate them so that, while swiping serves for direction change, 
// ..continuous touch serves ball forward movement

// THIS IS THE OLDER VERSION THAT LIMITS THE ACTION OF THE LETTERS WHICH 
//..REACHED THE SPECIFIED BORDERS

public class SwipeControl : MonoBehaviour
{
    private Vector2 startPos;
    private int pD = 15; // pixel distance to detect the swipe action
    [HideInInspector] public bool fingerDown;

    public float laneSpeed;
    private float moveX;
    private float moveY;

    private GameObject[] letters; // represents all letters in the table
    private GameObject theLetter; // represents clicked letter
    private Vector2 letterPos; // represents clicked letter's position
    private Vector2 tileBoundary1; // to limit movement of the letters from one end
    private Vector2 tileBoundary2; // to limit movement of the letters from other end

    private GameObject[] lettersX; // all letters at the same x-axis of clicked letter
    private GameObject[] lettersY; // all letters at the same y-axis of clicked letter
    private int n1; // index helps to save the correct letters inside lettersX/Y
    private int n2; // index helps to save the correct letters inside lettersX/Y
    private int n3; // indext to rearrange the limit exceeding lane

    private bool isYmove = true; // enables to move the letters along y-axis
    private bool isXmove = true; // enables to move the letters along x-axis
    private bool isEmpty = false; // disables sliding operation when cell is empty

    // Each block (letter) covers spesific area in coordinate lane
    // Find a way to determine the first-clicked block's coordinate
    //..and move it by sliding 

    // *aproximate coordinate values
    // left-top (14, 1487)
    // right-top (1065, 1487)
    // left-bottom (14, 240)
    // right-bottom (1065, 240)

    // one edge of the square is 150

    // as an approach, name all the squares, determine their borders
    void Start()
    {
        letters = GameObject.FindGameObjectsWithTag("Letter");
        lettersX = GameObject.FindGameObjectsWithTag("Letter"); // handle'em
        lettersY = GameObject.FindGameObjectsWithTag("Letter");
    }

    // after messing the letters, some places trigger other lanes to move
    void Update()
    {
        // -_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-
        //                  FOR PC
        // -_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-       
        print(theLetter);
        if (Input.GetMouseButtonDown(0))
        {
            //n = 0;
            startPos = Input.mousePosition; // this and below variable may cause problem due to common use
            // below loop provides us correct letter by handling with x/y coordinates
            for (int i = 0; i < 8; i++)  // this for loop is the vertical determinant
                for (int j = 0; j < 7; j++) // this for loop is the horizontal determinant                
                    if (startPos.x > (15 + j * 150) && startPos.x < (15 + (j + 1) * 150) && startPos.y > (1485 - (i + 1) * 150) && startPos.y < (1485 - i * 150))
                        theLetter = letters[(i * 7) + j];                

            letterPos = theLetter.transform.position;
            // adjust these loops as single of them will be executed at each buttonDown action
            for (int k = 0; k < letters.Length; k++)
                if (letters[k].transform.position.y > letterPos.y - 75 && letters[k].transform.position.y < letterPos.y + 75)
                {
                    lettersX[n1] = letters[k];
                    n1++;
                }
            //n = 0;
            for (int l = 0; l < letters.Length; l++)
                if (letters[l].transform.position.x > letterPos.x - 75 && letters[l].transform.position.x < letterPos.x + 75)
                {
                    lettersY[n2] = letters[l];
                    n2++;
                }

            // OLD Code for x/y-lane movement
            //for (int k = 0; k < 8; k++)
            //    lettersY[k] = letters[k * 7 + jTemp];
            //for (int l = 0; l < 7; l++)
            //    lettersX[l] = letters[iTemp * 7 + l];

            //print("Tile number: " + ((i * 7) + (j + 1)).ToString()); // finding the number of the block
            //print("i: " + (i+1).ToString() + " j: " + (j+1).ToString());
        }

        moveX = Input.GetAxis("Mouse X");
        moveY = Input.GetAxis("Mouse Y");

        if (fingerDown == false && Input.GetMouseButton(0))
            fingerDown = true;

        if (Input.GetMouseButtonDown(0))
            startPos = Input.mousePosition;

        if (fingerDown)
        {
            // up-boundary X: -450 / 450, Y: 595
            // down-boundary X: -450 / 450, Y: -800
            // right-boundary X: 600, Y: 440 / -645
            // left-boundary X: -600, Y: 440 / -645

            // xMove part causes some problems
            if (isXmove)
                if (Input.mousePosition.x <= startPos.x - pD || Input.mousePosition.x >= startPos.x + pD)
                {
                    isYmove = false;
                    fingerDown = false;
                    //theLetter.transform.Translate(moveX * laneSpeed * laneSpeed * Time.deltaTime, 0f, 0f);
                    for (int k = 0; (k < n1); k++) // needs to be recoded
                    {
                        if (lettersX[n1 - 1].transform.position.x > 90)
                        {
                            isEmpty = true;
                            n3 = 0;
                            // the reason for two for-loop is the non-sequential letter indexes
                            for (int i = n1 - 1; i > 2 * n1 / 3 - 1; i--)
                            {
                                lettersX[i].transform.position = new Vector2(89.9f + n3 * 150, lettersX[i].transform.position.y);
                                n3++;
                            }
                            n3 = 0;
                            for (int i = 0; i < 2 * n1 / 3; i++)
                            {
                                lettersX[i].transform.position = new Vector2(1140 + n3 * 150, lettersX[i].transform.position.y);
                                n3++;
                            }
                            //lettersX[n1 - 1].transform.position = new Vector2(90, lettersX[n1 - 1].transform.position.y);
                        }
                        else if (lettersX[2 * n1 / 3 - 1].transform.position.x < 990)
                        {
                            isEmpty = true;
                            n3 = 0;
                            // the reason for two for-loop is the non-sequential letter indexes
                            for (int i = n1 - 1; i > 2 * n1 / 3 - 1; i--)
                            {
                                lettersX[i].transform.position = new Vector2(-2010 + n3 * 150, lettersX[i].transform.position.y);
                                n3++;
                            }
                            n3 = 0;
                            for (int i = 0; i < 2 * n1 / 3; i++)
                            {
                                lettersX[i].transform.position = new Vector2(-960 + n3 * 150.1f, lettersX[i].transform.position.y);
                                n3++;
                            }
                            //lettersX[2*n1/3-1].transform.position = new Vector2(990, lettersX[2*n1/3-1].transform.position.y);                            
                        }
                        if (!isEmpty)
                            lettersX[k].transform.Translate(moveX * laneSpeed * laneSpeed * Time.deltaTime, 0f, 0f);
                    }
                }

            if (isYmove)
                if (Input.mousePosition.y >= startPos.y + pD || Input.mousePosition.y <= startPos.y - pD)
                {
                    isXmove = false;
                    fingerDown = false;
                    //theLetter.transform.Translate(0f, moveY * laneSpeed * laneSpeed * Time.deltaTime, 0f);                   
                    for (int k = 0; (k < n2); k++) // needs to be recoded                  
                    {
                        if (lettersY[n2 - 1].transform.position.y < 1400)
                        {
                            isEmpty = true;
                            n3 = 0;
                            // the reason for two for-loop is the non-sequential letter indexes
                            for (int i = n2 - 1; i > 2 * n2 / 3 - 1; i--)
                            {
                                lettersY[i].transform.position = new Vector2(lettersY[i].transform.position.x, 1400 - n3 * 155.1f);
                                //lettersY[n2-1].transform.position = new Vector2(lettersY[n2-1].transform.position.x, 1400);
                                n3++;
                            }
                            n3 = 0;
                            for (int i = 0; i < 2 * n2 / 3; i++)
                            {
                                lettersY[i].transform.position = new Vector2(lettersY[i].transform.position.x, 160 - n3 * 155.1f);
                                n3++;
                            }
                        }
                        else if (lettersY[2 * n2 / 3 - 1].transform.position.y > 310)
                        {
                            isEmpty = true;
                            // the reason for two for-loop is the non-sequential letter indexes
                            // note that if the sequence changes: inside for loop, if letter number changes: Vector2.y changes
                            n3 = 0;
                            for (int i = n2 - 1; i > 2 * n2 / 3 - 1; i--)
                            {
                                lettersY[i].transform.position = new Vector2(lettersY[i].transform.position.x, 3875 - n3 * 155.1f);
                                n3++;
                            }
                            n3 = 0;
                            for (int i = 0; i < 2 * n2 / 3; i++)
                            {
                                lettersY[i].transform.position = new Vector2(lettersY[i].transform.position.x, 2635 - n3 * 155.1f);
                                //lettersY[2*n2/3-1].transform.position = new Vector2(lettersY[2*n2/3-1].transform.position.x, 1550-n3*155);
                                n3++;
                            }
                        }
                        if (!isEmpty)
                            lettersY[k].transform.Translate(0f, moveY * laneSpeed * laneSpeed * Time.deltaTime, 0f);
                    }
                }
        }

        // statement that enables targeted axis motion and disables other
        if (Input.GetMouseButtonUp(0))
        {
            isXmove = true;
            isYmove = true;
            n1 = 0; n2 = 0;
            isEmpty = false;
        }

        //if (fingerDown)
        //{
        //    if (Input.mousePosition.x <= startPos.x - pD)
        //    {
        //        fingerDown = false;
        //        //Debug.Log("Swipe Left!");             
        //    }
        //    else if (Input.mousePosition.x >= startPos.x + pD)
        //    {
        //        fingerDown = false;
        //        //Debug.Log("Swipe Right!");
        //    }
        //    else if (Input.mousePosition.y >= startPos.y + pD)
        //    {
        //        fingerDown = false;
        //        //Debug.Log("Swipe Up!");               
        //    }
        //    else if (Input.mousePosition.y <= startPos.y - pD)
        //    {
        //        fingerDown = false;
        //        //Debug.Log("Swipe Down!");                
        //    }
        //}

    }
}