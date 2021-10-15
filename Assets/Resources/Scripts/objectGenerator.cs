using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class objectGenerator : MonoBehaviour
{

    GameObject square;
    // Start is called before the first frame update
    void Start()
    {
        //1. load square template from resources
        square = Resources.Load<GameObject>("Prefabs/Square");
        //2. Instantiate a square in the MIDDLE of the screen at 0 degree rotation
        GameObject tempSquare = Instantiate(square, new Vector3(0f, 0f), Quaternion.identity);
        //3. Set a random color for the square
        tempSquare.GetComponent<SpriteRenderer>().color = Random.ColorHSV();
        //4. Find the coordinates of the edges of the square
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
