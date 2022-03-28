using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;

public class AuctionController : MonoBehaviour
{

    public Auction auctionModel;
    public GameObject AuctionCanvas;
    public GameObject ResultCanvas;

    public GameObject invalidMovePrefab;
    public GameObject messagePrefab;
    public RectTransform countersLeftLayoutGroup;
    public RectTransform currentCounterLayoutGroup;
    public RectTransform soldCounterLayoutGroup;
    public TMPro.TMP_Text currentHighestBidText;
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

    //Called every time we get state and it is currently an auction.
    public void updateAuction(Auction auction){
        auctionModel = auction;

        thisPlayerBid = auctionModel.highestBid + 1;

        updateLayoutGroup(countersLeftLayoutGroup, auctionModel.countersForAuction);
        List<Counter> currentCounter = new List<Counter>();
        currentCounter.Add(auctionModel.getCurrentAuctioningCounter());
        updateLayoutGroup(currentCounterLayoutGroup, currentCounter);

        currentHighestBidText.text = "The current highest bidder is " + auctionModel.highestBidder + " with a bid of " + auctionModel.highestBid + " gold. Place your bid:";

        if( (counterUpForAuction != null) && (auctionModel.getCurrentAuctioningCounter().id != counterUpForAuction.id)){
            //Enable popup window here ***
            List<Counter> soldCounter = new List<Counter>();
            soldCounter.Add(counterUpForAuction);
            updateLayoutGroup(soldCounterLayoutGroup, soldCounter);
        }

        counterUpForAuction = auctionModel.getCurrentAuctioningCounter();
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
        
        Elfenroads.Control.passAuction();
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
            Elfenroads.Control.placeBid(thisPlayerBid);
        }
        else{
            invalidMessage("You can only bid higher!");
            return;
        }
    }

    public void minusClicked(){
        if (thisPlayerBid - 1 < auctionModel.highestBid){
            invalidMessage("You can only bid higher!");
            return;
        }
        thisPlayerBid = thisPlayerBid - 1;
        updateBid();
    }

    public void plusClicked(){
        thisPlayerBid = thisPlayerBid + 1;
        updateBid();
    }

    public void updateBid(){
        // update bid value displayed 
    }

    public void iSeeClicked(){
        // 
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

    public void updateLayoutGroup(RectTransform targetLayoutGroup, List<Counter> countersToShow) { 
        Debug.Log("Model was updated!");
        //First, destroy all children
        foreach(Transform child in targetLayoutGroup){
            child.SetParent(null);
            DestroyImmediate(child.gameObject);
        }
        targetLayoutGroup.DetachChildren();

        //Now, loop through the cards of the model, instantiating appropriate counter each time.
        foreach(Counter c in countersToShow){
            switch(c){
                case TransportationCounter tc:
                {
                    //Debug.Log("Transport type of " + c.id + " is: " + tc.transportType);
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
