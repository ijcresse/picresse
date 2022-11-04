using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint {
    public int num {get; private set;}
    public bool solved {get; set;}
    public int earliestPosition;
    public int latestPosition;
    public Hint(int num, bool solved) {
        this.num = num;
        this.solved = solved;
    }
    
}
