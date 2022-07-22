using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// for now, direction change and ball forward movement work together
// try to seperate them so that, while swiping serves for direction change, 
// ..continuous touch serves ball forward movement

public class SwipeController : MonoBehaviour
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

    private float surplusL; // position surplus to place the letters into specific coordinate

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

            // very up-boundary X: -450 / 450, Y: 1680
            // very down-boundary X: -450 / 450, Y: -1885
            // very right-boundary X: 1500, Y: 440 / -645
            // very left-boundary X: -1500, Y: 440 / -645

            // xMove part causes some problems
            if (isXmove)
                if (Input.mousePosition.x <= startPos.x - pD || Input.mousePosition.x >= startPos.x + pD)
                {
                    isYmove = false;
                    fingerDown = false;
                    //theLetter.transform.Translate(moveX * laneSpeed * laneSpeed * Time.deltaTime, 0f, 0f);
                    for (int k = 0; (k < n1); k++) // needs to be recoded
                    {
                        if(moveX*laneSpeed*Time.deltaTime <= 4.5f && moveX*laneSpeed*Time.deltaTime >= -4.5f)
                            lettersX[k].transform.Translate(moveX * laneSpeed * Time.deltaTime, 0f, 0f);
                        else if (moveX*laneSpeed*Time.deltaTime > 4.5f)
                            lettersX[k].transform.Translate(4.5f, 0f, 0f);
                        else 
                            lettersX[k].transform.Translate(-4.5f, 0f, 0f);

                        //print(moveY * laneSpeed * Time.deltaTime);
                        print(lettersX[k].ToString() +": "+lettersX[k].transform.position.x.ToString());

                        if (lettersX[k].transform.position.x > 2190)
                            lettersX[k].transform.position = new Vector2(-960, lettersX[k].transform.position.y);
                        else if (lettersX[k].transform.position.x < -1110)
                            lettersX[k].transform.position = new Vector2(2040, lettersX[k].transform.position.y);
                    }
                }

            if (isYmove)
            {
                if (Input.mousePosition.y >= startPos.y + pD || Input.mousePosition.y <= startPos.y - pD)
                {
                    isXmove = false;
                    fingerDown = false;
                    //theLetter.transform.Translate(0f, moveY * laneSpeed * laneSpeed * Time.deltaTime, 0f);                   
                    for (int k = 0; (k < n2); k++) // needs to be recoded                  
                    {
                        if (moveY * laneSpeed * Time.deltaTime <= 4.5f && moveY * laneSpeed * Time.deltaTime >= -4.5f)
                            lettersY[k].transform.Translate(0f, moveY * laneSpeed * Time.deltaTime, 0f);
                        else if (moveY * laneSpeed * Time.deltaTime >= 4.5f)
                            lettersY[k].transform.Translate(0f, 4.5f, 0f);
                        else
                            lettersY[k].transform.Translate(0f, -4.5f, 0f);

                        //print(lettersY[k].ToString() +": "+lettersY[k].transform.position.y.ToString());

                        if (lettersY[k].transform.position.y > 2795)
                            lettersY[k].transform.position = new Vector2(lettersY[k].transform.position.x, -925);
                        else if (lettersY[k].transform.position.y < -1080)
                            lettersY[k].transform.position = new Vector2(lettersY[k].transform.position.x, 2640);
                    }
                }
            }
        }

        // statement that enables targeted axis motion and disables other
        if (Input.GetMouseButtonUp(0))
        {
            // (2640, -155, -925) !Below statement is not functional!
            //if (!isXmove) // work from here if you need to rearrange letters due to 
            //    for (int k = 0; k < n2; k++)
            //        if ((lettersY[k].transform.position.y - 5) % 155 != 0)
            //            for (int i = 0; i < lettersY.Length; i++)
            //                if (lettersY[i].transform.position.y > (i * 155 - 925) - 77.5f && lettersY[i].transform.position.y < (i * 155 - 925) + 77.5f)
            //                    lettersY[i].transform.position = new Vector2(lettersY[i].transform.position.x, i * 155 - 925);

            // (-960, 150, 2040)
            if (!isYmove)
            {
                surplusL = (lettersX[0].transform.position.x + 60) % 150;
                if (surplusL > 75)
                    surplusL = -(150 - surplusL);

                for (int k = 0; k < n1; k++)
                {
                    lettersX[k].transform.position = new Vector2(lettersX[k].transform.position.x - surplusL, lettersX[k].transform.position.y);
                    if (lettersX[k].transform.position.x > 2040)
                        lettersX[k].transform.position = new Vector2(-960, lettersX[k].transform.position.y);
                    else if (lettersX[k].transform.position.x < -960)
                        lettersX[k].transform.position = new Vector2(2040, lettersX[k].transform.position.y);
                }
            }
            if (!isXmove)
            {
                surplusL = (lettersY[0].transform.position.y - 5) % 155;
                if (surplusL > 77.5f)
                    surplusL = -(155 - surplusL);
                
                for (int k = 0; k < n2; k++)
                {
                    lettersY[k].transform.position = new Vector2(lettersY[k].transform.position.x, lettersY[k].transform.position.y - surplusL);
                    if (lettersY[k].transform.position.y > 2640)
                        lettersY[k].transform.position = new Vector2(lettersY[k].transform.position.x, -925);
                    else if (lettersY[k].transform.position.y < -925)
                        lettersY[k].transform.position = new Vector2(lettersY[k].transform.position.x, 2640);
                }

                // to check 
                //print(lettersY[0].ToString() + ": "+ (lettersY[0].transform.position.y+5).ToString());
                //print(surplusL);
            }
            


            isXmove = true;
            isYmove = true;
            n1 = 0; n2 = 0;
        }
    }
}