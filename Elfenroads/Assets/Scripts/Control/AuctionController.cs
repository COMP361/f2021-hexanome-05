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


    void Start(){
        auctionModel = (Auction) Elfenroads.Model.game.currentPhase;
    }

    void Update(){
        thisPlayerBid = auctionModel.highestBid;
    }

    
    public void updateWaitingCounters(Auction au){
        waitingCountersView.updateWaitingCounters(au);
    }

    
    public void updateCurrentCounter(Auction au){
        currentCounterView.updateCurrentCounter(au);
    }


    public void passTurn(){
        if(! (Elfenroads.Model.game.currentPhase is Auction)){
            invalidMessage("Incorrect phase!");
            return;
        }

        if(! Elfenroads.Control.isCurrentPlayer()){
            invalidMessage("Not your turn!");
            return;
        }
        
        Elfenroads.Control.passTurn();
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
        // Add checks here ***

        Elfenroads.Control.placeBid(thisPlayerBid);
    }

    public void minusClicked(){
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
