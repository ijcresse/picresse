using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : MonoBehaviour
{

    private Vector2 gridStart = new Vector2(0, 0);
    // public GameObject cursor;
    private (float x, float y) gridSize;
    //unused right now, but should be the master of this property eventually
    public float boxSize { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        //y is negative to make cursor go down only
        gridSize = (x: 5, y: -5);
        boxSize = 0.5f;
        // Instantiate(cursor, gridStart, cursor.transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool CanMove(Vector2 position)
    {
        // if (position.x >= 0 && position.x <= gridSize.x && position.y <= 0 && position.y >= gridSize.y)
        // {
        //     return true;
        // }
        // return false;
        return true;
    }
}
