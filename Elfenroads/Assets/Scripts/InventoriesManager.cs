using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoriesManager : MonoBehaviour
{
    private int townPiecesCount = 1;
    private GameObject boot;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void incrementTownPieceCount(){
        this.townPiecesCount = this.townPiecesCount + 1;
    }

    public int countTownPiece(){
        return this.townPiecesCount;
    }

    public void setBoot(GameObject pBoot){
        this.boot = pBoot;
    }
}
