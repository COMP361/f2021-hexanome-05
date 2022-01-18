using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownView : MonoBehaviour
{
    private List<Slot> townPieceSlots;
    private List<Slot> bootSlots;

    public GameObject townPiecePrefab;
    public GameObject bootPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //First, create the TownPiece slots.
        Vector3 initialSlot = gameObject.transform.position - new Vector3(0.2f, 0.5f, 0f);
        float colCount = 0;
        float rowCount = 0;
        townPieceSlots = new List<Slot>();
        for(int i = 0 ; i < 6 ; i++){
            townPieceSlots.Add(new Slot(initialSlot + new Vector3(0.2f * colCount, - (0.25f * rowCount), 0f)));
            Instantiate(townPiecePrefab, initialSlot + new Vector3(0.2f * colCount, - (0.25f * rowCount), 0f), Quaternion.identity);   //Remove later, just here now to help discern where the "slots" are.
            colCount = (colCount + 1) % 3;
            if(colCount == 0) rowCount++;
        }

        //Next, create the Boot slots.
        initialSlot = gameObject.transform.position + new Vector3(-0.3f, 0.5f, 0f);
        colCount = 0;
        rowCount = 0;
        townPieceSlots = new List<Slot>();
        for(int i = 0 ; i < 6 ; i++){
            townPieceSlots.Add(new Slot(initialSlot + new Vector3(0.35f * colCount, - (0.6f * rowCount), 0f)));
            Instantiate(bootPrefab, initialSlot + new Vector3(0.35f * colCount, - (0.6f * rowCount), 0f), Quaternion.identity);   //Remove later, just here now to help figure out where the "slots" are.
            colCount = (colCount + 1) % 3;
            if(colCount == 0) rowCount++;
        }

    }

}
