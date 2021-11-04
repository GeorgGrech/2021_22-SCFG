using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class objectGenerator : MonoBehaviour
{

    //Reaction Time Game
    //------------------
    //Our reaction time is measured by the time taken for us to see something
    //and take action about it.

    //We will need to ask the user to input his/her name.

    //In this case, we are going to generate a square every RANDOM interval
    //for 15 times. We are going to measure the time FOR EACH square generated
    //between the square being generated, and us clicking on the square.

    //Each box will be a round, so we will inform the user before each box is going 
    //to be generated, measure the reaction time, and move to the next round, informing
    //the user of which round he/she is in.

    //After 15 rounds, the average reaction time of the user will be shown and the game
    //ends.


    //After the player enters his/her name and presses 'start', I want to show the following
    //instruction: 
    //A square will appear on a random location of the screen, click on it as quickly as you can
    //after 1second, I want that text to become a countdown from 3
    //when the countdown is 0, the countdown disappears
    //after a random interval, a box is spawned and the time is taken
    //I will then measure the time taken for the player to react to the box being spawned
    //that number is the score of round 1.


    //Each round of the game, will work as follows:
    //1. I will wait for a random interval, after which I will generate a random square 
    //   on screen.
    //2. I will save the time when the random square was generated
    //3. When the player click ON THE SQUARE only, the square will disappear and the round ends.
    //4. I will display the reaction time of that round and the round number
    //5. Countdown again, and back to 1.


    void playRound()
    {
        StartCoroutine(generateRandomSquare());
    }

    GameObject enemyBoxParent;

    IEnumerator generateRandomSquare()
    {
        //1. Wait a random interval
        yield return new WaitForSeconds(Random.Range(0f, 2f));
        //save the time when the box is generated
        timeToCompareTo = Time.time;
        float randomX = Random.Range(-4.5f, 4.5f);
        float randomY = Random.Range(-4.5f, 4.5f);

        GameObject enemySquare = makeOneSquare(randomX, randomY, enemyBoxParent);
        enemySquare.AddComponent<BoxCollider2D>();
        //coroutine ends here
        yield return null;

    }


    bool usingMouse, gameStarted;

    public float keyboardSpeed;

    float[] reactionTimes;

    string playerName;

    int squarecounter;

    Text inputSelectorText, roundTimerText;

    GameObject square, parentObject;

    //UI Variables
    GameObject hudPrefab, menuPrefab, countDownPrefab, hudInstance, menuInstance, countDownInstance;

    float timeToCompareTo;



    // Start is called before the first frame update
    void Start()
    {
        gameStarted = false;
        //the name of the prefab in the actual folder is CaseSenSiTive
        menuPrefab = Resources.Load<GameObject>("Prefabs/StartMenu");
        //hud prefab in the same way
        hudPrefab = Resources.Load<GameObject>("Prefabs/HUD");
        //countdown prefab in the same way
        countDownPrefab = Resources.Load<GameObject>("Prefabs/CountDown");

        //group all enemy boxes under one parent
        enemyBoxParent = new GameObject();
        //draw the menu in the middle of the screen
        setupMenu();
    }


    void setupMenu()
    {
        menuInstance = Instantiate(menuPrefab, Vector3.zero, Quaternion.identity);

        //PlayerName + StartButton
        GameObject.Find("StartButton").GetComponent<Button>().onClick.AddListener(
            () =>
            {
                playerName = GameObject.Find("PlayerName").GetComponent<InputField>().text;
                Debug.Log("Player name is: " + playerName);
                //destroy the menu
                Destroy(menuInstance);
                //start the round
                StartCoroutine(showInstructions());

            }
        );

    }

    //show instructions for one second
    IEnumerator showInstructions()
    {
        //show on screen
        countDownInstance = Instantiate(countDownPrefab, Vector3.zero, Quaternion.identity);
        countDownInstance.GetComponentInChildren<Text>().text =
        "A square will appear on a random location of the screen!\nClick on it as quickly as you can!";
        yield return new WaitForSeconds(1f);

        yield return countDown();
    }

    int countdownCounter = 3;
    //count down from 3
    IEnumerator countDown()
    {
        while (countdownCounter > 0)
        {
            countDownInstance.GetComponentInChildren<Text>().text = countdownCounter.ToString();
            yield return new WaitForSeconds(1f);
            countdownCounter--;
        }
        Destroy(countDownInstance);
        startGame();
        yield return null;
    }


    //when escape is pressed while the round is started, the hud and game should stop and I should go back
    //to the menu.

    void backToMenu()
    {
        Destroy(hudInstance);
        Destroy(parentObject);
        gameStarted = false;
        setupMenu();
    }

    int counter = 0;
    void showDurationBetweenClicks()
    {
        counter++;
        Debug.Log(counter + ":" + (Time.time - timeToCompareTo));
        timeToCompareTo = Time.time;
    }

    void startGame()
    {
        hudInstance = Instantiate(hudPrefab, Vector3.zero, Quaternion.identity);
        timeToCompareTo = Time.time;
        //get the input selector text
        inputSelectorText = GameObject.Find("InputSelector").GetComponent<Text>();
        //round timer text
        roundTimerText = GameObject.Find("RoundTimer").GetComponent<Text>();

        inputSelectorText.text = "M";

        usingMouse = true;

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

        gameStarted = true;
        playRound();
    }
    //generate N squares horizontally

    //modify this code to generate a full row, a full column, one diagonal going up
    //and the opposite as well (similar to the English flag)

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

    void mouseControl()
    {
        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;

        Vector3 asterixPosition = Camera.main.ScreenToWorldPoint(new Vector3(mouseX, mouseY, 0f));

        parentObject.transform.position = new Vector3(asterixPosition.x, asterixPosition.y);
        //this method will use raycast to shoot a ray in 
        //this means left click
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(asterixPosition, Vector3.forward);
            if (hit.collider != null)
            {
                Destroy(hit.collider.gameObject);
                float reactiontime = Time.time - timeToCompareTo;
                Debug.Log(reactiontime);
            }
        }

    }

    void keyboardControl(float keyspeed)
    {
        parentObject.transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * keyspeed * Time.deltaTime);
        parentObject.transform.Translate(Vector3.up * Input.GetAxis("Vertical") * keyspeed * Time.deltaTime);
    }


    // Update is called once per frame
    void Update()
    {
        //only happens if gameStarted is true
        if (gameStarted)
        {
            if (usingMouse)
            {
                if (inputSelectorText.text != "M")
                    inputSelectorText.text = "M";
                mouseControl();

            }
            else
            {
                if (inputSelectorText.text != "K")
                    inputSelectorText.text = "K";
                keyboardControl(keyboardSpeed);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                usingMouse = !usingMouse;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                backToMenu();
            }

            if (Input.GetMouseButtonDown(0))
            {
                showDurationBetweenClicks();
            }
        }

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
