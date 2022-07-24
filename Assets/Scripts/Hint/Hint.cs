using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hint {
    private int num {get; set;}
    private bool solved {get; set;}
    public Hint(int num, bool solved) {
        this.num = num;
        this.solved = solved;
    }
    
}
