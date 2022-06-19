using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockController : MonoBehaviour
{
    private int startTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Current();
    }

    // Update is called once per frame
    void Update()
    {
        // Debug.Log($"timer: {Current() - startTime}");
    }

    int Current()
    {
        DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        int currentEpochTime = (int)(DateTime.UtcNow - epoch).TotalSeconds;
        return currentEpochTime;
    }

}
