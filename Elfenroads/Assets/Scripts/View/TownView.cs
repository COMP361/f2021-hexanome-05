using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Models;


public class TownView : MonoBehaviour {
    [HideInInspector]
    public Town modelTown { private set; get; }
    public string townName;
    public bool isStartingTown;

    //private List<Slot> townPieceSlots;
    private Slots townPieces;
    //private List<Slot> bootSlots;
    private Slots boots;

    public GameObject redTownPiecePrefab;
    public GameObject redBootPrefab;
    public GameObject blueTownPiecePrefab;
    public GameObject blueBootPrefab;
    public GameObject greenTownPiecePrefab;
    public GameObject greenBootPrefab;
    public GameObject yellowTownPiecePrefab;
    public GameObject yellowBootPrefab;
    public GameObject purpleTownPiecePrefab;
    public GameObject purpleBootPrefab;
    public GameObject blackTownPiecePrefab;
    public GameObject blackBootPrefab;

    // Start is called before the first frame update
    void Start()
    {
        //First, create the TownPiece slots.
        // Vector3 initialSlot = gameObject.transform.position - new Vector3(0.2f, 0.5f, 0f);
        // float colCount = 0;
        // float rowCount = 0;
        // townPieceSlots = new List<Slot>();
        // for(int i = 0 ; i < 6 ; i++){
        //     townPieceSlots.Add(new Slot(initialSlot + new Vector3(0.2f * colCount, - (0.25f * rowCount), 0f)));
        //     //Instantiate(townPiecePrefab, initialSlot + new Vector3(0.2f * colCount, - (0.25f * rowCount), 0f), Quaternion.identity);   //Remove later, just here now to help discern where the "slots" are.
        //     colCount = (colCount + 1) % 3;
        //     if(colCount == 0) rowCount++;
        // }
        townPieces = new Slots(6, 3, gameObject.transform.position - new Vector3(0.2f, 0.5f, 0f), 0.2f, 0.25f);

        //Next, create the Boot slots.
        // initialSlot = gameObject.transform.position + new Vector3(-0.3f, 0.5f, 0f);
        // colCount = 0;
        // rowCount = 0;
        // bootSlots = new List<Slot>();
        // for(int i = 0 ; i < 6 ; i++){
        //     bootSlots.Add(new Slot(initialSlot + new Vector3(0.35f * colCount, - (0.6f * rowCount), 0f)));
        //     //Instantiate(bootPrefab, initialSlot + new Vector3(0.35f * colCount, - (0.6f * rowCount), 0f), Quaternion.identity);   //Remove later, just here now to help figure out where the "slots" are.
        //     colCount = (colCount + 1) % 3;
        //     if(colCount == 0) rowCount++;
        // }
        boots = new Slots(6, 3, gameObject.transform.position + new Vector3(-0.3f, 0.5f, 0f), 0.35f, 0.6f);


    }

    public void setAndSubscribeToModel(Town t){
            modelTown = t;
            modelTown.Updated += onModelUpdated;
            this.onModelUpdated(null, null); //When subscribing initially, want the view to take effect. 
            
    }

    void onModelUpdated(object sender, EventArgs e) {
        //Remove all townpieces and boots from slots (can just do difference later but this is easiest)
        townPieces.removeAllFromSlots();
        boots.removeAllFromSlots();
    
        //Next, look at the townPieces in the Model, and add the appropriate TownPiece prefabs to the slots.
        foreach(TownPiece piece in modelTown.townPieces){
            //Switch case for colors needed here.
            switch(piece.color){
                case Models.Color.RED:
                {
                    townPieces.addToSlot(redTownPiecePrefab, this.gameObject);
                    break;
                }
                case Models.Color.BLUE:
                {
                    townPieces.addToSlot(blueTownPiecePrefab, this.gameObject);
                    break;
                }
                case Models.Color.GREEN:
                {
                    townPieces.addToSlot(greenTownPiecePrefab, this.gameObject);
                    break;
                }
                case Models.Color.YELLOW:
                {
                    townPieces.addToSlot(yellowTownPiecePrefab, this.gameObject);
                    break;
                }
                case Models.Color.PURPLE:
                {
                    townPieces.addToSlot(purpleTownPiecePrefab, this.gameObject);
                    break;
                }
                case Models.Color.BLACK:
                {
                    townPieces.addToSlot(blackTownPiecePrefab, this.gameObject);
                    break;
                }
                default: Debug.Log("Piece with no color!!") ; break;
            }
        }
        //Then, do the same thing but for the ElfenBoots.
        foreach(Boot boot in modelTown.boots){
            //Switch case also needed here.
            switch(boot.color){
                case Models.Color.RED:
                {
                    boots.addToSlot(redBootPrefab,this.gameObject);
                    break;
                }
                case Models.Color.BLUE:
                {
                    boots.addToSlot(blueBootPrefab,this.gameObject);
                    break;
                }
                case Models.Color.GREEN:
                {
                    boots.addToSlot(greenBootPrefab,this.gameObject);
                    break;
                }
                case Models.Color.YELLOW:
                {
                    boots.addToSlot(yellowBootPrefab, this.gameObject);
                    break;
                }
                case Models.Color.PURPLE:
                {
                    boots.addToSlot(purpleBootPrefab, this.gameObject);
                    break;
                }
                case Models.Color.BLACK:
                {
                    boots.addToSlot(blackBootPrefab, this.gameObject);
                    break;
                }
                default: Debug.Log("Boot with no color!!") ; break;
            }
        }
    }

    //Need to make prefabs for each color of boot and townpiece, and use the color enum to figure out which color to add/remove (parameters change from GameObject obj -> Color color)

    // public void addTownPieceToSlot(Models.Color color){
    //     foreach(Slot s in townPieceSlots){
    //         if(s.obj == null){
    //             GameObject newPiece = Instantiate(townPiecePrefab, new Vector3(s.xCoord, s.yCoord, gameObject.transform.position.z + 0.5f), Quaternion.identity);
    //             s.obj = newPiece;
    //             return;
    //         }
    //     }
    //     Debug.Log("No slot found!");
    // }

    // public void removeTownPieceFromSlot(GameObject obj){ 
    //     foreach(Slot s in townPieceSlots){
    //         if(s.obj == obj){
    //             s.obj = null;
    //             return;
    //         }else{
    //             Debug.Log("Nothing to remove!");
    //         }
    //     }
    // }

    // public void addBootToSlot(Models.Color color){

    //     foreach(Slot s in bootSlots){
    //         if(s.obj == null){
    //             GameObject newBoot = Instantiate(bootPrefab, new Vector3(s.xCoord, s.yCoord, gameObject.transform.position.z + 0.5f), Quaternion.identity);
    //             s.obj = newBoot;
    //             return;
    //         }
    //     }
    //     Debug.Log("No slot found!");
    // }

    // public void removeBootFromSlot(GameObject obj){ 
    //     foreach(Slot s in bootSlots){
    //         if(s.obj == obj){
    //             s.obj = null;
    //             return;
    //         }else{
    //             Debug.Log("Nothing to remove!");
    //         }
    //     }
    // }

    // public void removeAllFromSlots(){
    //     foreach (Slot s in bootSlots){
    //         if(s.obj != null){
    //             Destroy(s.obj);
    //             s.obj = null;
    //         }
    //     }
    //     foreach (Slot s in townPieceSlots){
    //         if(s.obj != null){
    //             Destroy(s.obj);
    //             s.obj = null;
    //         }
    //     }
    // }
}