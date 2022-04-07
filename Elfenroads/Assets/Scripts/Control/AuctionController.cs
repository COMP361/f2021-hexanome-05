using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;

public class AuctionController : MonoBehaviour
{

    public Auction auctionModel;
    public GameObject AuctionCanvas;
    public GameObject PlayerInventoryCanvas;

    public GameObject invalidMovePrefab;
    public GameObject messagePrefab;
    public GameObject auctionResultWindowPrefab;
    public RectTransform countersLeftLayoutGroup;
    public RectTransform currentCounterLayoutGroup;
    public TMPro.TMP_Text currentHighestBidText;
    public TMPro.TMP_Text turnText;
    public TMPro.TMP_Text passedPlayersText;
    public TMPro.TMP_Text currentBidText;

    
    [Header("Counter Prefabs")]
    public GameObject dragonCounterPrefab;
    public GameObject trollCounterPrefab;
    public GameObject cloudCounterPrefab;
    public GameObject cycleCounterPrefab;
    public GameObject unicornCounterPrefab;
    public GameObject pigCounterPrefab;
    public GameObject landObstaclePrefab;
    public GameObject seaObstaclePrefab;
    public GameObject goldCounterPrefab;
    public GameObject doubleCounterPrefab;
    public GameObject exchangeCounterPrefab;

    private Counter counterUpForAuction;
    private int thisPlayerBid;

    private string previousHighestBidder;

    //Called every time we get state and it is currently an auction.
    public void updateAuction(Auction auction){
        auctionModel = auction;
        if( (counterUpForAuction != null) && (auctionModel.getCurrentAuctioningCounter().id != counterUpForAuction.id)){
            List<Counter> soldCounter = new List<Counter>();
            soldCounter.Add(counterUpForAuction);
            if (previousHighestBidder == null){
                previousHighestBidder = "Nobody obtained:";
            }
            else {
                previousHighestBidder = previousHighestBidder + " obtained:";
            }
            string message = previousHighestBidder;
            spawnAuctionResultWindow(message, soldCounter);

            previousHighestBidder = null;
        }

        thisPlayerBid = auctionModel.highestBid + 1;
        counterUpForAuction = auctionModel.getCurrentAuctioningCounter();
        updateLayoutGroup(countersLeftLayoutGroup, auctionModel.countersForAuction, true);
        List<Counter> currentCounter = new List<Counter>();
        currentCounter.Add(auctionModel.getCurrentAuctioningCounter());
        updateLayoutGroup(currentCounterLayoutGroup, currentCounter, false);

        if(auctionModel.highestBidder == null){
            currentHighestBidText.text = "There are no bids on this counter yet.";
        }else{
            currentHighestBidText.text = "The current highest bidder is " + auctionModel.highestBidder.name + " with a bid of " + auctionModel.highestBid + " gold.";
        }

        if(! Elfenroads.Control.isCurrentPlayer()){
            turnText.text = "Please wait, " + auctionModel.currentPlayer.name + " is placing a bid...";
        }else{
            turnText.text = "Your turn! Place a bid or pass:";
        }
        currentBidText.text = thisPlayerBid + "";
    }

    public void passAuction(){
        if(! (Elfenroads.Model.game.currentPhase is Auction)){
            invalidMessage("Incorrect phase!");
            return;
        }

        if(! Elfenroads.Control.isCurrentPlayer()){
            invalidMessage("Not your turn!");
            return;
        }

        if(auctionModel.highestBidder != null){
            previousHighestBidder = auctionModel.highestBidder.name;
        }
        
        Elfenroads.Control.placeBid(0);
    }


    public void placeBid(){
        if(! (Elfenroads.Model.game.currentPhase is Auction)){
            invalidMessage("Incorrect phase!");
            return;
        }

        if(! Elfenroads.Control.isCurrentPlayer()){
            invalidMessage("Not your turn!");
            return;
        }
        
        if (thisPlayerBid > auctionModel.highestBid){
            previousHighestBidder = "You";
            Elfenroads.Control.placeBid(thisPlayerBid);
        }
        else{
            invalidMessage("You can only bid higher!");
            return;
        }
    }

    public void minusClicked(){
        if(! Elfenroads.Control.isCurrentPlayer()){
            invalidMessage("Not your turn!");
            return;
        }
        if (thisPlayerBid - 1 <= auctionModel.highestBid || thisPlayerBid - 1 == 0){
            invalidMessage("You can only bid higher than the current price!");
            return;
        }
        thisPlayerBid = thisPlayerBid - 1;
        updateBid(thisPlayerBid);
    }

    public void plusClicked(){
        if(! Elfenroads.Control.isCurrentPlayer()){
            invalidMessage("Not your turn!");
            return;
        }
        if(thisPlayerBid + 1 > auctionModel.currentPlayer.inventory.gold){
            invalidMessage("Not enough gold!");
            return;
        }
        thisPlayerBid = thisPlayerBid + 1;
        updateBid(thisPlayerBid);
    }

    public void updateBid(int thisPlayerBid){
        currentBidText.text = "" + thisPlayerBid;
    }

    public void playerTurnMessage(string message){
        GameObject messageBox = Instantiate(messagePrefab, AuctionCanvas.transform.position, Quaternion.identity, AuctionCanvas.transform);
        messageBox.transform.GetChild(1).gameObject.GetComponent<TMPro.TMP_Text>().text = message;
        Destroy(messageBox, 1.9f);
    }

    private void invalidMessage(string message){
        Debug.Log("Invalid message: " + message);
        GameObject invalidBox = Instantiate(invalidMovePrefab, Input.mousePosition, Quaternion.identity, AuctionCanvas.transform);
        invalidBox.GetComponent<TMPro.TMP_Text>().text = message;
        Destroy(invalidBox, 2f);
    }

    public void spawnAuctionResultWindow(string message, List<Counter> soldCounter){
        GameObject window = Instantiate(auctionResultWindowPrefab, PlayerInventoryCanvas.transform.position, Quaternion.identity, PlayerInventoryCanvas.transform);
        window.GetComponentInChildren<TMPro.TMP_Text>().text = message;
        GameObject auctionedCounter = window.transform.Find("AuctionedCounter").gameObject;
        RectTransform soldCounterLayoutGroup = auctionedCounter.GetComponentInChildren<RectTransform>();
        updateLayoutGroup(soldCounterLayoutGroup, soldCounter, false);
        Destroy(window, 1.9f);
    }

    public void updateLayoutGroup(RectTransform targetLayoutGroup, List<Counter> countersToShow, bool scheduled) { 
        Debug.Log("Model was updated!");
        //First, destroy all children
        foreach(Transform child in targetLayoutGroup){
            child.SetParent(null);
            DestroyImmediate(child.gameObject);
        }
        targetLayoutGroup.DetachChildren();
        if(scheduled && countersToShow.Count <= 1){
            return;
        }
        int first = 0;
        //Now, loop through the cards of the model, instantiating appropriate counter each time.
        foreach(Counter c in countersToShow){
            if(first == 0 && scheduled){
                first++;
                continue;
            }
            switch(c){
                case TransportationCounter tc:
                {
                    switch(tc.transportType){
                        case TransportType.Dragon:
                        {  
                           GameObject instantiatedCounter = Instantiate(dragonCounterPrefab, targetLayoutGroup);
                            break;
                        }
                        case TransportType.ElfCycle:
                        {
                            GameObject instantiatedCounter = Instantiate(cycleCounterPrefab, targetLayoutGroup);
                            break;
                        }
                        case TransportType.MagicCloud:
                        {
                            GameObject instantiatedCounter = Instantiate(cloudCounterPrefab, targetLayoutGroup);
                            break;
                        }
                        case TransportType.TrollWagon:
                        {
                            GameObject instantiatedCounter = Instantiate(trollCounterPrefab, targetLayoutGroup);
                            break;
                        }
                        case TransportType.GiantPig:
                        {
                            GameObject instantiatedCounter = Instantiate(pigCounterPrefab, targetLayoutGroup);
                            break;
                        }
                        case TransportType.Unicorn:
                        {
                            GameObject instantiatedCounter = Instantiate(unicornCounterPrefab, targetLayoutGroup);
                            break;
                        }
                        
                        default: Debug.Log("Model transportation counter of type raft! This is not allowed!") ; break;
                    }
                    break;
                }
                case MagicSpellCounter msc:
                {
                    switch(msc.spellType){
                        case SpellType.Exchange:
                        {
                            GameObject instantiatedCounter = Instantiate(exchangeCounterPrefab, targetLayoutGroup);
                            break;
                        }
                        case SpellType.Double:
                        {
                            GameObject instantiatedCounter = Instantiate(doubleCounterPrefab, targetLayoutGroup);
                            break;
                        }
                    }
                    break;
                }
                case GoldCounter gc:
                {
                    GameObject instantiatedCounter = Instantiate(goldCounterPrefab, targetLayoutGroup);
                    break;
                }
                case ObstacleCounter obc:
                {
                    switch(obc.obstacleType){
                        case ObstacleType.Land:
                        {
                            GameObject instantiatedCounter = Instantiate(landObstaclePrefab, targetLayoutGroup);
                            break;
                        }
                        case ObstacleType.Sea:
                        {
                            GameObject instantiatedCounter = Instantiate(seaObstaclePrefab, targetLayoutGroup);
                            break;
                        }
                    }
                    break;
                }
                default: Debug.Log("Counter is of undefined type!") ; break;
            }
        }
    }
}
