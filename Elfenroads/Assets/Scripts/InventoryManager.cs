using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public GameObject CounterManagerObserver;
    private int intTownPieceCounter = 1;
    private GameObject TMPTownPieceCounter;
    // Start is called before the first frame update
    void Start()
    {
        TMPTownPieceCounter.GetComponent<TextMeshProUGUI>().SetText("1");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void incrementTownPiece(){
        this.intTownPieceCounter = this.intTownPieceCounter + 1;
        TMPTownPieceCounter.GetComponent<TextMeshProUGUI>().SetText(intTownPieceCounter.ToString());
    }

    public void setTownPieceCounter(GameObject tpCounter){
        this.TMPTownPieceCounter = tpCounter;
    }
}
