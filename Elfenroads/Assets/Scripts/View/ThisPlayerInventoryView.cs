using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Models;
public class ThisPlayerInventoryView : MonoBehaviour
{
    private Player playerModel;

    public PlayerInfoController playerInfoController;
    public GameObject playerCounters;
    public GameObject playerCards;
    public GameObject playerStats;

    [Header("Counter TMPs")]
    public TMPro.TMP_Text numCloudCounters;
    public TMPro.TMP_Text numCycleCounters;
    public TMPro.TMP_Text numDragonCounters;
    public TMPro.TMP_Text numLandObstacles;
    public TMPro.TMP_Text numPigCounters;
    public TMPro.TMP_Text numTrollCounters;
    public TMPro.TMP_Text numUnicornCounters;
    public TMPro.TMP_Text numSeaObstacles;
    public TMPro.TMP_Text numGoldCounters;

    [Header("Card TMPs")]
    public TMPro.TMP_Text numCloudCards;
    public TMPro.TMP_Text numCycleCards;
    public TMPro.TMP_Text numDragonCards;
    public TMPro.TMP_Text numRaftCards;
    public TMPro.TMP_Text numPigCards;
    public TMPro.TMP_Text numTrollCards;
    public TMPro.TMP_Text numUnicornCards;

    public void expandMyInventory(){
        playerInfoController.openAndSetupWindow(playerModel);
    }

    public void setAndSubscribeToModel(Player inputPlayer){
         playerModel = inputPlayer;
         setWindowTint(inputPlayer.boot.color);
         playerModel.Updated += onModelUpdated;
         if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.Elfengold)){
             playerCounters.GetComponent<GridLayoutGroup>().spacing = new Vector2(85f, 10f);
             foreach(Transform child in playerCounters.transform){
                child.gameObject.GetComponent<DragScript>().resetStartingPositions();
             }
         }
         onModelUpdated(null, null);
     }

    void onModelUpdated(object sender, EventArgs e) {
        //Here, needs to add counters to the GridLayoutGroup according to the model. Instantiated Counters must also have their "CounterViewHelper" component's "Guid" fields set appropriately.
        if(Elfenroads.Model.game.currentPhase is DrawCounters || Elfenroads.Model.game.currentPhase is PlanTravelRoutes || Elfenroads.Model.game.currentPhase is Auction){
            playerCounters.SetActive(true);
            playerCards.SetActive(false);
        }else{
            playerCounters.SetActive(false);
            playerCards.SetActive(true);
        }

        //First, get the CounterGridLayoutGroup from this object. 
        setCounters();

        //Now, we do the same for the cards.
        setCards();

        //Finally, we'll set the "Stats":
        playerStats.transform.GetChild(0).gameObject.GetComponent<TMPro.TMP_Text>().text = playerModel.name;
        updateTexts();
    }

    public void resetDraggablePositions(){

        if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.Elfengold)){
             playerCounters.GetComponent<GridLayoutGroup>().spacing = new Vector2(85f, 10f);
         }
         if(Elfenroads.Model.game.currentPhase is DrawCounters || Elfenroads.Model.game.currentPhase is PlanTravelRoutes || Elfenroads.Model.game.currentPhase is SelectCounter || Elfenroads.Model.game.currentPhase is FinishRound){
            //LayoutRebuilder.ForceRebuildLayoutImmediate(playerCounters.GetComponent<RectTransform>());
            foreach(Transform child in playerCounters.transform){
                child.gameObject.GetComponent<DragScript>().resetStartingPositions();
            }
         }
         if(Elfenroads.Model.game.currentPhase is DrawCards || Elfenroads.Model.game.currentPhase is MoveBoot || Elfenroads.Model.game.currentPhase is Auction)
            //LayoutRebuilder.ForceRebuildLayoutImmediate(playerCards.GetComponent<RectTransform>());
            foreach(Transform child in playerCards.transform){
                child.gameObject.GetComponent<DragScript>().resetStartingPositions();
             }
    }

    public void updateTexts(){
        Player curPlayer = Elfenroads.Model.getCurrentPlayer();
        if(curPlayer == null){
            playerStats.transform.GetChild(1).gameObject.GetComponent<TMPro.TMP_Text>().text = "P: " + playerModel.inventory.townPieces.Count;
            if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.Elfengold)){
                playerStats.transform.GetChild(1).gameObject.GetComponent<TMPro.TMP_Text>().text += " G: " + playerModel.inventory.gold;
            }
        }else if(curPlayer.id == playerModel.id){
            if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.Elfengold)){
                playerStats.transform.GetChild(1).gameObject.GetComponent<TMPro.TMP_Text>().text = "P: " + playerModel.inventory.townPieces.Count + " G: " + playerModel.inventory.gold  + "\nCurrentPlayer: Y";
            }else{
                playerStats.transform.GetChild(1).gameObject.GetComponent<TMPro.TMP_Text>().text = "P: " + playerModel.inventory.townPieces.Count + "\nCurrentPlayer: Y";
            }
        }else{
            if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.Elfengold)){
                playerStats.transform.GetChild(1).gameObject.GetComponent<TMPro.TMP_Text>().text = "P: " + playerModel.inventory.townPieces.Count + " G: " + playerModel.inventory.gold  + "\nCurrentPlayer: N";
            }else{
                playerStats.transform.GetChild(1).gameObject.GetComponent<TMPro.TMP_Text>().text = "P: " + playerModel.inventory.townPieces.Count + "\nCurrentPlayer: N";
            }
        } 
    }

    //Adjusts the Counter TMPs according to the model.
    private void setCounters(){
        //GameObject countersLayoutGroup = GameObject.Find("PlayerCounters");
        int dragonCounters = 0;
        int trollCounters = 0;
        int cloudCounters = 0;
        int cycleCounters = 0;
        int unicornCounters = 0;
        int pigCounters = 0;
        int landObstacles = 0;
        int seaObstacles = 0;
        int goldCounters = 0;

        //Then, assign appropriate counters as in RoadView.
         foreach(Counter c in playerModel.inventory.counters){
            switch(c){
                case TransportationCounter tc:
                {
                    switch(tc.transportType){
                        case TransportType.Dragon:
                        {  
                           dragonCounters++;
                            break;
                        }
                        case TransportType.ElfCycle:
                        {
                            cycleCounters++;
                            break;
                        }
                        case TransportType.MagicCloud:
                        {
                            cloudCounters++;
                            break;
                        }
                        case TransportType.TrollWagon:
                        {
                            trollCounters++;
                            break;
                        }
                        case TransportType.GiantPig:
                        {
                            pigCounters++;
                            break;
                        }
                        case TransportType.Unicorn:
                        {
                            unicornCounters++;
                            break;
                        }
                        default: Debug.Log("Model transportation counter of type raft! This is not allowed!") ; break;
                    }
                    break;
                }
                // case MagicSpellCounter msc: //Won't be shown in the bar below the player.
                // {
                //     Debug.Log("Elfengold - Do later");
                //     break;
                // }
                case GoldCounter gc:
                {
                    goldCounters++;
                    break;
                }
                case ObstacleCounter obc:
                {
                    //*** Add sea obstacle later, during elfengold.
                    if(obc.obstacleType is ObstacleType.Land){
                        landObstacles++;
                    }else{
                        seaObstacles++;
                    }
                    break;
                }
                default: Debug.Log("Counter is of undefined type!") ; break;
            }
        }

        setAmount(numCloudCounters, cloudCounters);
        setAmount(numCycleCounters, cycleCounters);
        setAmount(numDragonCounters, dragonCounters);
        setAmount(numLandObstacles, landObstacles);
        setAmount(numPigCounters, pigCounters);
        setAmount(numTrollCounters, trollCounters);
        setAmount(numUnicornCounters, unicornCounters);
        setAmount(numSeaObstacles, seaObstacles);
        setAmount(numGoldCounters, goldCounters);
    }

    //Does the same as setCounters, but for the cards.
    private void setCards(){
        //GameObject cardsLayoutGroup = GameObject.Find("PlayerCards");
        int dragonCards = 0;
        int trollCards = 0;
        int cloudCards = 0;
        int cycleCards = 0;
        int unicornCards = 0;
        int pigCards = 0;
        int raftCards = 0;

        //Then, assign appropriate counters as in RoadView.
         foreach(Card c in playerModel.inventory.cards){
            switch(c){
                case TravelCard tc:
                {
                    switch(tc.transportType){
                        case TransportType.Dragon:
                        {  
                           dragonCards++;
                            break;
                        }
                        case TransportType.ElfCycle:
                        {
                            cycleCards++;
                            break;
                        }
                        case TransportType.MagicCloud:
                        {
                            cloudCards++;
                            break;
                        }
                        case TransportType.TrollWagon:
                        {
                            trollCards++;
                            break;
                        }
                        case TransportType.GiantPig:
                        {
                            pigCards++;
                            break;
                        }
                        case TransportType.Unicorn:
                        {
                            unicornCards++;
                            break;
                        }
                        case TransportType.Raft:
                        {
                            raftCards++;
                            break;
                        }
                        default: Debug.Log("Error: Card type invalid.") ; break;
                    }
                    break;
                }
                case WitchCard wc:
                {
                    Debug.Log("Witch cards don't show up on the player hand!");
                    break;
                }
                case GoldCard gc:
                {
                    Debug.Log("GoldCards don't show up on the player hand!");
                    break;
                }
                default: Debug.Log("Card is of undefined type!") ; break;
            }
        }

        setAmount(numCloudCards, cloudCards);
        setAmount(numCycleCards, cycleCards);
        setAmount(numDragonCards, dragonCards);
        setAmount(numRaftCards, raftCards);
        setAmount(numPigCards, pigCards);
        setAmount(numTrollCards, trollCards);
        setAmount(numUnicornCards, unicornCards);
    }

    private static void setAmount(TMPro.TMP_Text text, int amount){
            text.text = amount.ToString() + "x";
        }

    private void setWindowTint(Models.Color inputColor){
        switch(inputColor){
                case Models.Color.RED:{
                    this.gameObject.GetComponent<Image>().color = new Color32(221, 76, 76, 255);
                    foreach(Transform child in playerStats.transform){
                        if(child.GetComponent<TMPro.TMP_Text>() != null){
                            child.GetComponent<TMPro.TMP_Text>().color = new Color32(1, 1, 1, 255);
                        }
                    }
                    break;
                }
                case Models.Color.BLUE:{
                    this.gameObject.GetComponent<Image>().color = new Color32(50, 127, 210, 255);
                    foreach(Transform child in playerStats.transform){
                        if(child.GetComponent<TMPro.TMP_Text>() != null){
                            child.GetComponent<TMPro.TMP_Text>().color = new Color32(1, 1, 1, 255);
                        }
                    }
                    break;
                }
                case Models.Color.GREEN:{
                    this.gameObject.GetComponent<Image>().color = new Color32(46, 154, 71, 255);
                    foreach(Transform child in playerStats.transform){
                        if(child.GetComponent<TMPro.TMP_Text>() != null){
                            child.GetComponent<TMPro.TMP_Text>().color = new Color32(1, 1, 1, 255);
                        }
                    }
                    break;
                }
                case Models.Color.PURPLE:{
                    this.gameObject.GetComponent<Image>().color = new Color32(160, 45, 193, 255);
                    foreach(Transform child in playerStats.transform){
                        if(child.GetComponent<TMPro.TMP_Text>() != null){
                            child.GetComponent<TMPro.TMP_Text>().color = new Color32(1, 1, 1, 255);
                        }
                    }
                    break;
                }
                case Models.Color.YELLOW:{
                    this.gameObject.GetComponent<Image>().color = new Color32(253, 223, 20, 255);
                    foreach(Transform child in playerStats.transform){
                        if(child.GetComponent<TMPro.TMP_Text>() != null){
                            child.GetComponent<TMPro.TMP_Text>().color = new Color32(1, 1, 1, 255);
                        }
                    }
                    break;
                }
                case Models.Color.BLACK:{
                    //Black is a little more involved, since we need to make the text white for the cards and counters as well.
                    this.gameObject.GetComponent<Image>().color = new Color32(42, 42, 44, 255);
                    foreach(Transform child in playerStats.transform){
                        if(child.GetComponent<TMPro.TMP_Text>() != null){
                            child.GetComponent<TMPro.TMP_Text>().color = new Color32(243, 243, 243, 255);
                        }
                    }
                    foreach(Transform child in playerCounters.transform){
                        foreach(Transform count in child){
                            count.GetComponent<TMPro.TMP_Text>().color = new Color32(243, 243, 243, 255);
                        }
                    }
                    foreach(Transform child in playerCards.transform){
                        foreach(Transform count in child){
                            count.GetComponent<TMPro.TMP_Text>().color = new Color32(243, 243, 243, 255);
                        }
                    }
                    break;
                }
                default: Debug.Log("Invalid color!"); break;
            }
    }
}
