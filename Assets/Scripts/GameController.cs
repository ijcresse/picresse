using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject cursor;
    private Vector2 gridStart = new Vector2(0, 0);
    
    void Start()
    {
        Instantiate(cursor, gridStart, cursor.transform.rotation);
    }

    void Update()
    {
        
    }
}
