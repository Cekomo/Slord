using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// limit the swipe controller inside of the table
// currently, outside letters can be swiped
// sometimes picked letter does not place the swiped but the previous one

// with the exact placement of the letters to their specific positions, some the 
//..statemets become redundant, you can fix'em
public class SwipeController : MonoBehaviour
{
    private Vector2 startPos; // starting position of mouse click
    private int pD = 15; // pixel distance to detect the swipe action
    [HideInInspector] public bool fingerDown;

    public float laneSpeed; // speed constant of the lane while moving it
    private float moveX; // to move the lane in x-axis
    private float moveY; // to move the lane in y-axis

    private bool inTable; // to determine if the mouse click is in table or not

    private GameObject[] letters; // represents all letters in the table
    private GameObject theLetter; // represents clicked letter
    private Vector2 letterPos; // represents clicked letter's position
    private Vector2 tileBoundary1; // to limit movement of the letters from one end
    private Vector2 tileBoundary2; // to limit movement of the letters from other end

    [HideInInspector] public float pointDecrement; // to decrease the point with each swipe
    private float distDiff; // to determine point decrement by checking the swiping distance

    private GameObject[] lettersX; // all letters at the same x-axis of clicked letter
    private GameObject[] lettersY; // all letters at the same y-axis of clicked letter
    private int n1; // index helps to save the correct letters inside lettersX/Y
    private int n2; // index helps to save the correct letters inside lettersX/Y
    private int n3; // indext to rearrange the limit exceeding lane

    private bool isYmove = true; // enables to move the letters along y-axis
    private bool isXmove = true; // enables to move the letters along x-axis

    private float surplusL; // position surplus to place the letters into specific coordinate

    [HideInInspector] public bool isFinished = false;

    // Each block (letter) covers spesific area in coordinate lane
    // Find a way to determine the first-clicked block's coordinate
    //..and move it by sliding 

    // *aproximate coordinate values
    // left-top (14, 1487)
    // right-top (1065, 1487)
    // left-bottom (14, 240)
    // right-bottom (1065, 240)

    // one edge of the square is 150 for x-axis and 155 for y-axis

    // as an approach, name all the squares, determine their borders
    void Start()
    {
        pointDecrement = 0f; // make it zero for each level

        letters = GameObject.FindGameObjectsWithTag("Letter");
        lettersX = GameObject.FindGameObjectsWithTag("Letter"); // handle'em
        lettersY = GameObject.FindGameObjectsWithTag("Letter");

        //for (int i = 0; i < letters.Length; i++)
        //    print(letters[i].ToString() + " : " + letters[i].transform.position.ToString());

        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 7; j++)
            {
                letters[(7 * i) + j].transform.position = new Vector2(letters[(7 * i) + j].transform.position.x, (9 - i) * 155);
                print(letters[(7 * i) + j].ToString() + " X: " + letters[(7 * i) + j].transform.position.x.ToString() + " Y: " + letters[(7 * i) + j].transform.position.y.ToString());
            }

        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 7; j++)
            {
                letters[(7 * i) + j + 56].transform.position = new Vector2(letters[7 * (i + 8) + j].transform.position.x, (i - 1) * -155);
                print(letters[(7 * i) + j + 56].ToString() + " X: " + letters[(7 * i) + j + 56].transform.position.x.ToString() + " Y: " + letters[(7 * i) + j + 56].transform.position.y.ToString());
            }
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 7; j++)
            {
                letters[(7 * i) + j + 112].transform.position = new Vector2(letters[(7 * i) + j + 112].transform.position.x, i * 155 + 1550);
                print(letters[(7 * i) + j + 112].ToString() + " X: " + letters[(7 * i) + j + 112].transform.position.x.ToString() + " Y: " + letters[(7 * i) + j + 112].transform.position.y.ToString());
            }
        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 7; j++)
            {
                letters[(7 * i) + j + 168].transform.position = new Vector2(letters[(7 * i) + j + 168].transform.position.x, (9 - i) * 155);
                print(letters[(7 * i) + j + 168].ToString() + " X: " + letters[(7 * i) + j + 168].transform.position.x.ToString() + " Y: " + letters[(7 * i) + j + 168].transform.position.y.ToString());
            }

        for (int i = 0; i < 8; i++)
            for (int j = 0; j < 7; j++)
            {
                letters[(7 * i) + j + 224].transform.position = new Vector2(letters[(7 * i) + j + 224].transform.position.x, (9 - i) * 155);
                print(letters[(7 * i) + j + 224].ToString() + " X: " + letters[(7 * i) + j + 224].transform.position.x.ToString() + " Y: " + letters[(7 * i) + j + 224].transform.position.y.ToString());
            }

    }
    
    /*
    void Update()
    {
        // -_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-
        //                  FOR PC
        // -_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-
        if (Input.GetMouseButtonDown(0)) startPos = Input.mousePosition;
        if (startPos.x > 85 && startPos.x < 995 && startPos.y < 1380 && startPos.y > 285) inTable = true;

        if (Input.GetMouseButtonDown(0) && inTable && !isFinished)
        {
            //n = 0;          
            startPos = Input.mousePosition; // this and below variable may cause problem due to common use
            for (int i = 0; i < letters.Length; i++)
            {
                letterPos = letters[i].transform.position; // to determine the letter to move on x/y-axis
                if (startPos.x > letterPos.x - 75 && startPos.x < letterPos.x + 75 && startPos.y > letterPos.y - 77.5f && startPos.y < letterPos.y + 77.5f)
                        theLetter = letters[i];
            }

            //for (int i = 0; i < 8; i++)  // this for loop is the vertical determinant
            //    for (int j = 0; j < 7; j++) // this for loop is the horizontal determinant                
            //        if (startPos.x > (15+j*150) && startPos.x < (15+(j+1)*150) && startPos.y > (1485-(i+1)*150) && startPos.y < (1485-i*150))                 
            //            theLetter = letters[(i * 7) + j];                    

            //print(theLetter); // inspect the letter transition to understand the error
            letterPos = theLetter.transform.position;
            //print(letterPos);
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

        if (fingerDown && inTable && !isFinished)
        {
            if (isXmove)
                if (Input.mousePosition.x <= startPos.x - pD || Input.mousePosition.x >= startPos.x + pD)
                {
                    isYmove = false;
                    fingerDown = false;
                    //theLetter.transform.Translate(moveX * laneSpeed * laneSpeed * Time.deltaTime, 0f, 0f);
                    for (int k = 0; (k < n1); k++) // needs to be recoded
                    {
                        // to adjust the swipe speed
                        if (moveX * laneSpeed * Time.deltaTime <= 4.5f && moveX * laneSpeed * Time.deltaTime >= -4.5f)
                            lettersX[k].transform.Translate(moveX * laneSpeed * Time.deltaTime, 0f, 0f);
                        else if (moveX * laneSpeed * Time.deltaTime > 4.5f)
                            lettersX[k].transform.Translate(4.5f, 0f, 0f);
                        else
                            lettersX[k].transform.Translate(-4.5f, 0f, 0f);

                        //print(moveY * laneSpeed * Time.deltaTime);
                        //print(lettersX[k].ToString() + ": " + lettersX[k].transform.position.x.ToString());
                        //print(lettersX[k].ToString() + ": " + lettersX[k].transform.position.y.ToString());

                        // to adjust swipe borders (letter transfer to other end)
                        if (lettersX[k].transform.position.x > 2188.5f)
                            lettersX[k].transform.position = new Vector2(-958.5f, lettersX[k].transform.position.y);
                        else if (lettersX[k].transform.position.x < -1108.5f)
                            lettersX[k].transform.position = new Vector2(2038.5f, lettersX[k].transform.position.y);
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
                        // to adjust the swipe speed
                        if (moveY * laneSpeed * Time.deltaTime <= 4.5f && moveY * laneSpeed * Time.deltaTime >= -4.5f)
                            lettersY[k].transform.Translate(0f, moveY * laneSpeed * Time.deltaTime, 0f);
                        else if (moveY * laneSpeed * Time.deltaTime >= 4.5f)
                            lettersY[k].transform.Translate(0f, 4.5f, 0f);
                        else
                            lettersY[k].transform.Translate(0f, -4.5f, 0f);

                        //print(lettersY[k].ToString() +": "+lettersY[k].transform.position.y.ToString());

                        // to adjust swipe borders (letter transfer to other end)
                        if (lettersY[k].transform.position.y > 2790)
                            lettersY[k].transform.position = new Vector2(lettersY[k].transform.position.x, -930);
                        else if (lettersY[k].transform.position.y < -1085) // -955
                            lettersY[k].transform.position = new Vector2(lettersY[k].transform.position.x, 2635);
                    }
                }
            }
        }

        // this statement is for decreasing the point for every letter swiping from the score
        if (Input.GetMouseButton(0)) // nextLevel button error occurs here when presses
        {
            if (isXmove && Mathf.Abs(letterPos.x - theLetter.transform.position.x) >= 150)
            {
                distDiff = letterPos.x - theLetter.transform.position.x;
                pointDecrement = Mathf.Round(Mathf.Abs(distDiff) / 150) * 3;
                //print(pointDecrement);
                letterPos.x = theLetter.transform.position.x;
            }
            else if (isYmove && Mathf.Abs(letterPos.y - theLetter.transform.position.y) >= 155)
            {
                distDiff = letterPos.y - theLetter.transform.position.y;
                pointDecrement = Mathf.Round(Mathf.Abs(distDiff) / 155) * 3;
                //print(pointDecrement);
                letterPos.y = theLetter.transform.position.y;
            }
        }

        // statement that enables targeted axis motion and disables other
        if (Input.GetMouseButtonUp(0) && inTable && !isFinished)
        {
            // two statements below is to calculate point increment on x/y axis

            //print(Mathf.Abs(distDiff));

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
                surplusL = (lettersX[0].transform.position.x+58) % 150; // exactly 82.20661
                if (surplusL > 75)
                    surplusL = -(150 - surplusL);

                for (int k = 0; k < n1; k++)
                {
                    print(lettersX[k].transform.position.x.ToString() + " " + lettersX[k].ToString());
                    // it brokes the editor
                    //while (lettersX[k].transform.position.x < lettersX[k].transform.position.x - surplusL)
                    //    lettersX[k].transform.Translate(-surplusL * Time.deltaTime, 0f, 0f);

                    // specifies the end letters when mousebuttonup occurs if they're out of border
                    lettersX[k].transform.position = new Vector2(lettersX[k].transform.position.x - surplusL, lettersX[k].transform.position.y);
                    if (lettersX[k].transform.position.x > 2088.5f) // 2040
                        lettersX[k].transform.position = new Vector2(-958.5f, lettersX[k].transform.position.y);
                    if (lettersX[k].transform.position.x < -1008.45f) // -960
                        lettersX[k].transform.position = new Vector2(2038.5f, lettersX[k].transform.position.y);
                }
            }
            if (!isXmove)
            {
                surplusL = (lettersY[0].transform.position.y + 13) % 155; // 13.356 exactly
                if (surplusL > 77.5f)
                    surplusL = -(155 - surplusL);

                for (int k = 0; k < n2; k++)
                {
                    //print(lettersY[k].transform.position.y.ToString() + " " + lettersY[k].ToString());

                    // specifies the end letters when mousebuttonup occurs if they're out of border
                    lettersY[k].transform.position = new Vector2(lettersY[k].transform.position.x, lettersY[k].transform.position.y - surplusL);
                    if (lettersY[k].transform.position.y > 2712.5f) // 2640
                        lettersY[k].transform.position = new Vector2(lettersY[k].transform.position.x, -930);
                    else if (lettersY[k].transform.position.y < -1007.5f) // -925
                        lettersY[k].transform.position = new Vector2(lettersY[k].transform.position.x, 2635);
                }

                // to check 
                //print(lettersY[0].ToString() + ": "+ (lettersY[0].transform.position.y+5).ToString());
                //print(surplusL);
            }

            isXmove = true;
            isYmove = true;
            n1 = 0; n2 = 0;
        }

        inTable = false; // after loop finishes, inTable is false for the next loop
    }   */

    ///*
    void Update()
    {
        // -_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-
        //                  FOR Mobile
        // -_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-_-
        
        if (Input.GetTouch(0).phase == TouchPhase.Began) startPos = Input.touches[0].position;
        if (startPos.x > 20 && startPos.x < 1050 && startPos.y < 1650 && startPos.y > 450) inTable = true;

        if (Input.GetTouch(0).phase == TouchPhase.Began && inTable && !isFinished)
        {
            //n = 0;          
            startPos = Input.touches[0].position; // this and below variable may cause problem due to common use
            for (int i = 0; i < letters.Length; i++)
            {
                letterPos = letters[i].transform.position; // to determine the letter to move on x/y-axis
                if (startPos.x > letterPos.x - 75 && startPos.x < letterPos.x + 75 && startPos.y > letterPos.y - 77.5f && startPos.y < letterPos.y + 77.5f)
                    theLetter = letters[i];
            }

            //for (int i = 0; i < 8; i++)  // this for loop is the vertical determinant
            //    for (int j = 0; j < 7; j++) // this for loop is the horizontal determinant                
            //        if (startPos.x > (15+j*150) && startPos.x < (15+(j+1)*150) && startPos.y > (1485-(i+1)*150) && startPos.y < (1485-i*150))                 
            //            theLetter = letters[(i * 7) + j];                    

            //print(theLetter); // inspect the letter transition to understand the error
            letterPos = theLetter.transform.position;
            //print(letterPos);
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

        if (Input.touchCount > 0) // if statement is here; otherwise it throws an out of bounds error
        {
            moveX = Input.GetTouch(0).deltaPosition.x;
            moveY = Input.GetTouch(0).deltaPosition.y;
        }

        if (fingerDown == false && Input.touchCount > 0)
            fingerDown = true;

        if (Input.GetTouch(0).phase == TouchPhase.Began)
            startPos = Input.touches[0].position;

        if (fingerDown && inTable && !isFinished)
        {
            if (isXmove)
                if (Input.touches[0].position.x <= startPos.x - pD || Input.touches[0].position.x >= startPos.x + pD)
                {
                    isYmove = false;
                    fingerDown = false;
                    //theLetter.transform.Translate(moveX * laneSpeed * laneSpeed * Time.deltaTime, 0f, 0f);
                    for (int k = 0; (k < n1); k++) // needs to be recoded
                    {
                        // to adjust the swipe speed
                        if (moveX * laneSpeed * Time.deltaTime <= 4.5f && moveX * laneSpeed * Time.deltaTime >= -4.5f)
                            lettersX[k].transform.Translate(moveX * laneSpeed * Time.deltaTime, 0f, 0f);
                        else if (moveX * laneSpeed * Time.deltaTime > 4.5f)
                            lettersX[k].transform.Translate(4.5f, 0f, 0f);
                        else
                            lettersX[k].transform.Translate(-4.5f, 0f, 0f);

                        //print(moveY * laneSpeed * Time.deltaTime);
                        //print(lettersX[k].ToString() + ": " + lettersX[k].transform.position.x.ToString());
                        //print(lettersX[k].ToString() + ": " + lettersX[k].transform.position.y.ToString());

                        // to adjust swipe borders (letter transfer to other end)
                        if (lettersX[k].transform.position.x > 2188.5f)
                            lettersX[k].transform.position = new Vector2(-958.5f, lettersX[k].transform.position.y);
                        else if (lettersX[k].transform.position.x < -1108.5f)
                            lettersX[k].transform.position = new Vector2(2038.5f, lettersX[k].transform.position.y);
                    }
                }

            if (isYmove)
            {
                if (Input.touches[0].position.y >= startPos.y + pD || Input.touches[0].position.y <= startPos.y - pD)
                {
                    isXmove = false;
                    fingerDown = false;
                    //theLetter.transform.Translate(0f, moveY * laneSpeed * laneSpeed * Time.deltaTime, 0f);                   
                    for (int k = 0; (k < n2); k++) // needs to be recoded                  
                    {
                        // to adjust the swipe speed
                        if (moveY * laneSpeed * Time.deltaTime <= 4.5f && moveY * laneSpeed * Time.deltaTime >= -4.5f)
                            lettersY[k].transform.Translate(0f, moveY * laneSpeed * Time.deltaTime, 0f);
                        else if (moveY * laneSpeed * Time.deltaTime >= 4.5f)
                            lettersY[k].transform.Translate(0f, 4.5f, 0f);
                        else
                            lettersY[k].transform.Translate(0f, -4.5f, 0f);

                        //print(lettersY[k].ToString() +": "+lettersY[k].transform.position.y.ToString());

                        // to adjust swipe borders (letter transfer to other end)
                        if (lettersY[k].transform.position.y > 2790)
                            lettersY[k].transform.position = new Vector2(lettersY[k].transform.position.x, -930);
                        else if (lettersY[k].transform.position.y < -1085) // -955
                            lettersY[k].transform.position = new Vector2(lettersY[k].transform.position.x, 2635);
                    }
                }
            }
        }

        // this statement is for decreasing the point for every letter swiping from the score
        if (Input.touchCount > 0) // nextLevel button error occurs here when presses
        {
            if (isXmove && Mathf.Abs(letterPos.x - theLetter.transform.position.x) >= 150)
            {
                distDiff = letterPos.x - theLetter.transform.position.x;
                pointDecrement = Mathf.Round(Mathf.Abs(distDiff) / 150) * 3;
                //print(pointDecrement);
                letterPos.x = theLetter.transform.position.x;
            }
            else if (isYmove && Mathf.Abs(letterPos.y - theLetter.transform.position.y) >= 155)
            {
                distDiff = letterPos.y - theLetter.transform.position.y;
                pointDecrement = Mathf.Round(Mathf.Abs(distDiff) / 155) * 3;
                //print(pointDecrement);
                letterPos.y = theLetter.transform.position.y;
            }
        }

        // statement that enables targeted axis motion and disables other
        if (Input.GetTouch(0).phase == TouchPhase.Ended && inTable && !isFinished)
        {
            // two statements below is to calculate point increment on x/y axis

            //print(Mathf.Abs(distDiff));

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
                surplusL = (lettersX[0].transform.position.x + 58) % 150; // exactly 82.20661
                if (surplusL > 75)
                    surplusL = -(150 - surplusL);

                for (int k = 0; k < n1; k++)
                {
                    //print(lettersX[k].transform.position.x.ToString() + " " + lettersX[k].ToString());
                    // it brokes the editor
                    //while (lettersX[k].transform.position.x < lettersX[k].transform.position.x - surplusL)
                    //    lettersX[k].transform.Translate(-surplusL * Time.deltaTime, 0f, 0f);

                    // specifies the end letters when mousebuttonup occurs if they're out of border
                    lettersX[k].transform.position = new Vector2(lettersX[k].transform.position.x - surplusL, lettersX[k].transform.position.y);
                    if (lettersX[k].transform.position.x > 2088.5f) // 2040
                        lettersX[k].transform.position = new Vector2(-958.5f, lettersX[k].transform.position.y);
                    if (lettersX[k].transform.position.x < -1008.45f) // -960
                        lettersX[k].transform.position = new Vector2(2038.5f, lettersX[k].transform.position.y);
                }
            }
            if (!isXmove)
            {
                surplusL = (lettersY[0].transform.position.y) % 155; // 13.356 exactly
                if (surplusL > 77.5f)
                    surplusL = -(155 - surplusL);

                for (int k = 0; k < n2; k++)
                {
                    print(lettersY[k].transform.position.y.ToString() + " " + lettersY[k].ToString());

                    // specifies the end letters when mousebuttonup occurs if they're out of border
                    lettersY[k].transform.position = new Vector2(lettersY[k].transform.position.x, lettersY[k].transform.position.y - surplusL);
                    if (lettersY[k].transform.position.y > 2712.5f) // 2640
                        lettersY[k].transform.position = new Vector2(lettersY[k].transform.position.x, -930);
                    else if (lettersY[k].transform.position.y < -1007.5f) // -925
                        lettersY[k].transform.position = new Vector2(lettersY[k].transform.position.x, 2635);
                }

                // to check 
                //print(lettersY[0].ToString() + ": "+ (lettersY[0].transform.position.y+5).ToString());
                //print(surplusL);
            }

            isXmove = true;
            isYmove = true;
            n1 = 0; n2 = 0;
        }

        inTable = false; // after loop finishes, inTable is false for the next loop
    }   //*/
}
