using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Models;

public class TownView : MonoBehaviour
{
    private Town modelTown;
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
            //Instantiate(townPiecePrefab, initialSlot + new Vector3(0.2f * colCount, - (0.25f * rowCount), 0f), Quaternion.identity);   //Remove later, just here now to help discern where the "slots" are.
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
            //Instantiate(bootPrefab, initialSlot + new Vector3(0.35f * colCount, - (0.6f * rowCount), 0f), Quaternion.identity);   //Remove later, just here now to help figure out where the "slots" are.
            colCount = (colCount + 1) % 3;
            if(colCount == 0) rowCount++;
        }

        Elfenroads.Model.ModelReady += getAndSubscribeToModel;
    }

    public void getAndSubscribeToModel(object sender, EventArgs e){
            this.modelTown = ModelHelper.StoreInstance().getTown(gameObject.GetComponent<TownScript>().townName);
            modelTown.ModelUpdated += onModelUpdated;
    }

    void onModelUpdated(object sender, EventArgs e) {
        //First, look at the townPieces in the Model, and add the appropriate TownPiece prefabs to the slots.

        //Then, do the same thing but for the ElfenBoots.
    }

    //Need to make prefabs for each color of boot and townpiece, and use the color enum to figure out which color to add/remove (parameters change from GameObject obj -> Color color)

    public void addTownPieceToSlot(GameObject obj){
        foreach(Slot s in townPieceSlots){
            if(s.obj == null){
                s.obj = obj;
                obj.transform.position = (new Vector3(s.xCoord, s.yCoord, gameObject.transform.position.z + 0.5f));
                return;
            }else{

            }
        }
    }

    public void removeTownPieceFromSlot(GameObject obj){ 
        foreach(Slot s in townPieceSlots){
            if(s.obj == obj){
                s.obj = null;
                return;
            }else{
                Debug.Log("Nothing to remove!");
            }
        }
    }

    public void addBootToSlot(GameObject obj){
        foreach(Slot s in bootSlots){
            if(s.obj == obj){
                Debug.Log("Boot is already in a slot!");
                return;
            }
        }

        foreach(Slot s in bootSlots){
            if(s.obj == null){
                s.obj = obj;
                obj.transform.position = (new Vector3(s.xCoord, s.yCoord, gameObject.transform.position.z + 0.5f));
                return;
            }else{
            }
        }
    }

    public void removeBootFromSlot(GameObject obj){ 
        foreach(Slot s in bootSlots){
            if(s.obj == obj){
                s.obj = null;
                return;
            }else{
                Debug.Log("Nothing to remove!");
            }
        }
    }


}
