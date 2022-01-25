using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot{
    public float xCoord { set; get; }
    public float yCoord { set; get; }
    public GameObject obj { set; get; }

    public Slot(Vector3 inputPos){
        xCoord = inputPos.x;
        yCoord = inputPos.y;
    }

}
