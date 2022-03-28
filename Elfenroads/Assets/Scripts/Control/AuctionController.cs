using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;

public class AuctionController : MonoBehaviour
{

    public Auction auctionModel;

    public AuctionCounterView waitingCountersView;
    public CounterBeingAuctionedView currentCounterView;


    public GameObject AuctionCanvas;
    public GameObject ResultCanvas;

    public GameObject invalidMovePrefab;
    public GameObject messagePrefab;
    public RectTransform countersLeftLayoutGroup;
    public RectTransform currentCounterLayoutGroup;
    

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


    //Please review how 'currentPhase' works. This logic is flawed (the auction will never be the currentPhase at the start of any game)
    // void Start(){
    //     auctionModel = (Auction) Elfenroads.Model.game.currentPhase;
    // }

    //Called every time we get state and it is currently an auction.
    public void updateAuction(Auction auction){
        auctionModel = auction;
        thisPlayerBid = auctionModel.highestBid + 1;
        waitingCountersView.updateWaitingCounters(auction);
        currentCounterView.updateCurrentCounter(auction);
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
}
