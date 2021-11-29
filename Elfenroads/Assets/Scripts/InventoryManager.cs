using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public int townPieceCounter = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void incrementTownPiece(){
        this.townPieceCounter = this.townPieceCounter + 1;
    }

    public int countTownPiece(){
        return townPieceCounter;
    }
}
