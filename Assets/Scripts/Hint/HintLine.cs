using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintLine : MonoBehaviour
{
    public List<Hint> hints {get;}
    bool isCol {get;}
    bool active {get;} //true when cursor is on this line
    bool solved {get;}
    Vector2 position;
    private TextMeshProUGUI hintText;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
