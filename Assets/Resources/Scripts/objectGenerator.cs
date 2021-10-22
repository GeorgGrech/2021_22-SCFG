using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Reaction Time Game
/// --------------------
/// Our reaction time is measured by the time taken for use to see something
/// and take action about it.
/// 
/// We will need to ask the user to input his/her name.
/// 
/// In this case, we are going to generate a square every random interval
/// for 15 times. We are going to measure the time FOR EACH square generated
/// between the square being generated, and us clicking on the square.
/// 
/// Each box will be a round, so we will inform the user before each box is going
/// to be generated. measure the reaction time, and move to the next round, informing
/// the user of which round he/she is in.
/// 
/// After 15 rounds, the average reaction time of the user will be shown and the game
/// ends.
/// </summary>

public class objectGenerator : MonoBehaviour
{
    /// <summary>
    /// Lecturer's code
    /// </summary>
    GameObject square, parentObject;
    private bool mouseEnable;
    float keyboardSpeed;
    float[] reactionTime;
    string playerName;
    int squareCounter;

    Text inputSelectorText, roundTimerText;
    // Start is called before the first frame update
    void Start()
    {
        //get the input selector text
        inputSelectorText = GameObject.Find("InputSelector").GetComponent<Text>();
        roundTimerText = GameObject.Find("RoundTimer").GetComponent<Text>();
        inputSelectorText.text = "M";
        mouseEnable = true;

        squarecounter = 0;
        //1. Load square template from resources
        square = Resources.Load<GameObject>("Prefabs/Square");
        //2. Instantiate a square in the MIDDLE of the screen at 0 degree rotation
        // GameObject tempSquare = Instantiate(square,new Vector3(0f,0f),Quaternion.identity); 
        //3. Set a random colour for the square
        //  tempSquare.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        //4. Find the coordinates of the edges of the square
        generateNSquares(5);

        parentObject.transform.localScale = new Vector3(0.25f, 0.25f);
    }

    //generate N squares horizontally

    //modify this code to generate a full row, a full column, one diagonal going up
    //and the opposite as well (similar to the English flag)
    int squarecounter;
    GameObject makeOneSquare(float x, float y, GameObject myparentobject)
    {
        GameObject tempSquare = Instantiate(square, new Vector3(x, y), Quaternion.identity);
        squarecounter++;
        tempSquare.name = "Square-" + squarecounter;
        tempSquare.transform.parent = myparentobject.transform;
        tempSquare.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        return tempSquare;
    }
    void generateNSquares(int numberOfSquares)
    {
        parentObject = new GameObject();
        parentObject.name = "allSquares";
        //for loop to generate N squares horizontally
        for (int counter = -numberOfSquares; counter <= numberOfSquares; counter++)
        {
            makeOneSquare(counter, 0f, parentObject);
            makeOneSquare(0f, counter, parentObject);
            makeOneSquare(counter, counter, parentObject);
            makeOneSquare(counter, -counter, parentObject);
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (mouseEnable)
                mouseEnable = false;
                inputSelectorText.text = "M";
            else
                mouseEnable = true;
            
        }

        if (mouseEnable == true)
        {
            MouseMovement();
        }
        else
        {
            KeyboardMovement(keyboardSpeed);
        }

    }

    void MouseMovement()
    {
        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;

        Vector3 asterixPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseX, mouseY, 0f));

        parentObject.transform.position = new Vector3(asterixPosition.x, asterixPosition.y);

        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(Time.time);
        }
    }

    void KeyboardMovement(float keyspeed)
    {
        parentObject.transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * keyspeed * Time.deltaTime);
        parentObject.transform.Translate(Vector3.up * Input.GetAxis("Vertical") * keyspeed * Time.deltaTime);


        /*

        if(Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(new Vector3(10f, 0f, 0f) * Time.deltaTime, Space.World);
        }
        else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(-10f, 0f, 0f) * Time.deltaTime, Space.World);
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(new Vector3(0f, -10f, 0f) * Time.deltaTime, Space.World);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector3(0f, 10f, 0f) * Time.deltaTime, Space.World);
        }
        */
    }



    /// <summary>
    /// Code used previously for square star creation
    /// </summary>
    /*
    GameObject square,parentObjectAsterisk;
    // Start is called before the first frame update
    void Start()
    {
        parentObjectAsterisk = new GameObject();
        parentObjectAsterisk.name = "ParentObject";

        generateNSquares(11, 0).transform.parent = parentObjectAsterisk.transform;
        generateNSquares(11, 1).transform.parent = parentObjectAsterisk.transform;
        generateNSquares(11, 2).transform.parent = parentObjectAsterisk.transform;
        generateNSquares(11, 3).transform.parent = parentObjectAsterisk.transform;

    }

    //generate N Squares horizontally
    GameObject generateNSquares(int numberOfSquares,int orientation)
    {
        GameObject parentObject = new GameObject();
        parentObject.name = "allSquares";
        //1. load square template from resources
        square = Resources.Load<GameObject>("Prefabs/Square");

        //for loop to generate squares horizontally
        for (int x = 0; x < numberOfSquares; x++)
        {
            float xvalue = 0f;
            float yvalue = 0f;

            switch (orientation)
            {
                case 0: //horizontal
                    xvalue = -(numberOfSquares / 2) + x;
                    break;
                case 1: //vertical
                    yvalue = -(numberOfSquares / 2) + x;
                    break;
                case 2: //diagonal
                    xvalue = -(numberOfSquares / 2) + x;
                    yvalue = -(numberOfSquares / 2) + x;
                    break;
                case 3: //inverse diagonal
                    xvalue = -(numberOfSquares / 2) + x;
                    yvalue = -(-(numberOfSquares / 2) + x);
                    break;

            } 

            //2. Instantiate a square in the MIDDLE of the screen at 0 degree rotation
            GameObject tempSquare = Instantiate(square, new Vector3(xvalue, yvalue), Quaternion.identity);
            tempSquare.name = "Square-" + x;
            tempSquare.transform.parent = parentObject.transform;
            
            //3. Set a random color for the square
            tempSquare.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
            //4. Find the coordinates of the edges of the square
        }
        return parentObject;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;

        Vector3 asteriskPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseX, mouseY, 0f));

        parentObjectAsterisk.transform.position = asteriskPosition;
    }
    */
}
