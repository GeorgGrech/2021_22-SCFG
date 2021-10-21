using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectGenerator : MonoBehaviour
{

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
}
