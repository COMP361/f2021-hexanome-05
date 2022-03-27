using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Models;
using Controls;


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

    public Sprite goldValueTwo;
    public Sprite goldValueThree;
    public Sprite goldValueFour;
    public Sprite goldValueFive;
    public Sprite goldValueSix;
    public Sprite goldValueSeven;

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

            if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.Elfengold)){
                switch(modelTown.goldValue){
                    case (2):{
                        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = goldValueTwo;
                        break;
                    }
                    case (3):{
                        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = goldValueThree;
                        break;
                    }
                    case (4):{
                        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = goldValueFour;
                        break;
                    }
                    case (5):{
                        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = goldValueFive;
                        break;
                    }
                    case (6):{
                        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = goldValueSix;
                        break;
                    }
                    case (7):{
                        transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().sprite = goldValueSeven;
                        break;
                    }
                    default:{
                        Debug.Log("This must be elfenhold!");
                        transform.GetChild(0).gameObject.SetActive(false);
                        break;
                    } 
                }
            }

            this.onModelUpdated(null, null); //When subscribing initially, want the view to take effect. 
            
    }

    void removeAllFromSlots(Slots target){ //Needs to be added to all "Views" using slots, since Unity is goofy like that and doesn't allow deletion if it isn't in a Monobehavior.
        List<Slot> tSlots = target.getSlots();
        foreach(Slot s in tSlots){
            if(s.obj != null){
                Destroy(s.obj);
                s.obj = null;
            }
        }
    }

    void onModelUpdated(object sender, EventArgs e) {
        //Remove all townpieces and boots from slots (can just do difference later but this is easiest)
        removeAllFromSlots(townPieces);
        removeAllFromSlots(boots);
    
        //Next, look at the townPieces in the Model, and add the appropriate TownPiece prefabs to the slots.
        foreach(TownPiece piece in modelTown.townPieces){
            //Switch case for colors needed here.
            switch(piece.color){
                case Models.Color.RED:
                {
                    GameObject parameter = Instantiate(redTownPiecePrefab, Vector3.zero, Quaternion.identity);
                    townPieces.addToSlot(parameter, this.gameObject);
                    break;
                }
                case Models.Color.BLUE:
                {
                    GameObject parameter = Instantiate(blueTownPiecePrefab, Vector3.zero, Quaternion.identity);
                    townPieces.addToSlot(parameter, this.gameObject);
                    break;
                }
                case Models.Color.GREEN:
                {
                    GameObject parameter = Instantiate(greenTownPiecePrefab, Vector3.zero, Quaternion.identity);
                    townPieces.addToSlot(parameter, this.gameObject);
                    break;
                }
                case Models.Color.YELLOW:
                {
                    GameObject parameter = Instantiate(yellowTownPiecePrefab, Vector3.zero, Quaternion.identity);
                    townPieces.addToSlot(parameter, this.gameObject);
                    break;
                }
                case Models.Color.PURPLE:
                {
                    GameObject parameter = Instantiate(purpleTownPiecePrefab, Vector3.zero, Quaternion.identity);
                    townPieces.addToSlot(parameter, this.gameObject);
                    break;
                }
                case Models.Color.BLACK:
                {
                    GameObject parameter = Instantiate(blackTownPiecePrefab, Vector3.zero, Quaternion.identity);
                    townPieces.addToSlot(parameter, this.gameObject);
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
                    GameObject parameter = Instantiate(redBootPrefab, Vector3.zero, Quaternion.identity);
                    boots.addToSlot(parameter, this.gameObject);
                    break;
                }
                case Models.Color.BLUE:
                {
                    GameObject parameter = Instantiate(blueBootPrefab, Vector3.zero, Quaternion.identity);
                    boots.addToSlot(parameter, this.gameObject);
                    break;
                }
                case Models.Color.GREEN:
                {
                    GameObject parameter = Instantiate(greenBootPrefab, Vector3.zero, Quaternion.identity);
                    boots.addToSlot(parameter, this.gameObject);
                    break;
                }
                case Models.Color.YELLOW:
                {
                    GameObject parameter = Instantiate(yellowBootPrefab, Vector3.zero, Quaternion.identity);
                    boots.addToSlot(parameter, this.gameObject);
                    break;
                }
                case Models.Color.PURPLE:
                {
                    GameObject parameter = Instantiate(purpleBootPrefab, Vector3.zero, Quaternion.identity);
                    boots.addToSlot(parameter, this.gameObject);
                    break;
                }
                case Models.Color.BLACK:
                {
                    GameObject parameter = Instantiate(blackBootPrefab, Vector3.zero, Quaternion.identity);
                    boots.addToSlot(parameter, this.gameObject);
                    break;
                }
                default: Debug.Log("Boot with no color!!") ; break;
            }
        }
    }

    //Towns can be clicked on the MoveBoot phase during Elfengold, when a witch card is used.
    public void OnClick(){
        Debug.Log("Town clicked!");
        if(Elfenroads.Model.game == null){
            return;
        }

        if(Elfenroads.Model.game.currentPhase is MoveBoot && Elfenroads.Model.game.variant.HasFlag(Game.Variant.ElfenWitch)){
            GameObject.Find("MoveBootController").GetComponent<MoveBootController>().attemptMagicFlight(modelTown);
        }

    }
}