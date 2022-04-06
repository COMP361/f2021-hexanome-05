using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Views;
using Models;

public class DrawCardsController : MonoBehaviour, GuidHelperContainer
{
    public RectTransform cardsLayout;
    public TMPro.TMP_Text turnPrompt;
    public TMPro.TMP_Text goldCardAmt;
    public GameObject invalidMovePrefab;
    public GameObject DrawCardsCanvas;
    private int totalAmt = 0;
    [Header("CardPrefabs")]
    public GameObject dragonCardPrefab;
    public GameObject trollCardPrefab;
    public GameObject cloudCardPrefab;
    public GameObject cycleCardPrefab;
    public GameObject unicornCardPrefab;
    public GameObject pigCardPrefab;
    public GameObject raftCardPrefab;
    public GameObject witchCardPrefab;

    public void updateFaceUpCards() { //Later needs to take DrawCards Model to set GoldCardDeck attributes ***
        //Here, needs to add cards to the GridLayoutGroup according to the model. Instantiated Cards must also have their "GuidViewHelper" component's "Guid" fields set appropriately.
        //First, destroy all children (mwahahah)
        foreach(Transform child in cardsLayout){
            child.SetParent(null);
            DestroyImmediate(child.gameObject);
        }
        cardsLayout.DetachChildren();
        if(Elfenroads.Control.isCurrentPlayer()){
            turnPrompt.text = "Your turn! Draw a card!";
        }else{
            turnPrompt.text = Elfenroads.Control.currentPlayer.name + " is drawing a card...";
        }

        //Now, loop through the cards of the model, instantiating appropriate cards each time.
        foreach(Card c in Elfenroads.Model.game.faceUpCards){
            switch(c){
                case TravelCard tc:
                {
                    switch(tc.transportType){
                        case TransportType.Dragon:
                        {  
                            GameObject instantiatedCard = Instantiate(dragonCardPrefab, cardsLayout);
                            instantiatedCard.GetComponent<GuidViewHelper>().setGuid(c.id);
                            instantiatedCard.GetComponent<GuidViewHelper>().setContainer(this);
                            break;
                        }
                        case TransportType.ElfCycle:
                        {
                            GameObject instantiatedCard = Instantiate(cycleCardPrefab, cardsLayout);
                            instantiatedCard.GetComponent<GuidViewHelper>().setGuid(c.id);
                            instantiatedCard.GetComponent<GuidViewHelper>().setContainer(this);
                            break;
                        }
                        case TransportType.MagicCloud:
                        {
                            GameObject instantiatedCard = Instantiate(cloudCardPrefab, cardsLayout);
                            instantiatedCard.GetComponent<GuidViewHelper>().setGuid(c.id);
                            instantiatedCard.GetComponent<GuidViewHelper>().setContainer(this);
                            break;
                        }
                        case TransportType.TrollWagon:
                        {
                            GameObject instantiatedCard = Instantiate(trollCardPrefab, cardsLayout);
                            instantiatedCard.GetComponent<GuidViewHelper>().setGuid(c.id);
                            instantiatedCard.GetComponent<GuidViewHelper>().setContainer(this);
                            break;
                        }
                        case TransportType.GiantPig:
                        {
                            GameObject instantiatedCard = Instantiate(pigCardPrefab, cardsLayout);
                            instantiatedCard.GetComponent<GuidViewHelper>().setGuid(c.id);
                            instantiatedCard.GetComponent<GuidViewHelper>().setContainer(this);
                            break;
                        }
                        case TransportType.Unicorn:
                        {
                            GameObject instantiatedCard = Instantiate(unicornCardPrefab, cardsLayout);
                            instantiatedCard.GetComponent<GuidViewHelper>().setGuid(c.id);
                            instantiatedCard.GetComponent<GuidViewHelper>().setContainer(this);
                            break;
                        }
                        case TransportType.Raft:
                        {
                            GameObject instantiatedCard = Instantiate(raftCardPrefab, cardsLayout);
                            instantiatedCard.GetComponent<GuidViewHelper>().setGuid(c.id);
                            instantiatedCard.GetComponent<GuidViewHelper>().setContainer(this);
                            break;
                        }
                        default: Debug.Log("Invalid Card type!") ; break;
                    }
                    break;
                }
                case GoldCard gc:
                {
                    Debug.Log("Hey! There should never be a gold card here!");
                    break;
                }
                case WitchCard wc:
                {
                    GameObject instantiatedCard = Instantiate(witchCardPrefab, cardsLayout);
                    instantiatedCard.GetComponent<GuidViewHelper>().setGuid(c.id);
                    instantiatedCard.GetComponent<GuidViewHelper>().setContainer(this);
                    break;
                }
                default: Debug.Log("Card is of undefined type!") ; break;
            }
        }

        totalAmt = 0;
        foreach(GoldCard gc in Elfenroads.Model.game.goldCardDeck){
            totalAmt = totalAmt + 3;
        }
        goldCardAmt.text = "Gold in deck: " + totalAmt;
    }

    //Handles the (very little) needed validation for drawing a card.
    public void GUIClicked(GameObject clickedCard){
        if(! (Elfenroads.Model.game.currentPhase is DrawCards)){
            Debug.Log("Hey! How are you clicking this? It isn't DrawCards!");
            return;
        }
        if( Elfenroads.Control.isCurrentPlayer() ){
            Elfenroads.Control.drawCard(clickedCard);
        }else{
            Debug.Log("Click invalid. Is it your turn? -> " + (Elfenroads.Control.isCurrentPlayer()));
        }
    }



    public void takeGoldCards(){
        if(! (Elfenroads.Model.game.currentPhase is DrawCards)){
            Debug.Log("Hey! How are you clicking this? It isn't DrawCards!");
            return;
        }

        if(totalAmt == 0){
            invalidMessage("No cards in the deck!");
            return;
        }

        if( Elfenroads.Control.isCurrentPlayer() ){
            Elfenroads.Control.takeGoldCards();
        }else{
            Debug.Log("Click invalid. Is it your turn? -> " + (Elfenroads.Control.isCurrentPlayer()));
        }
    }

    public void takeRandomCard(){
        if(! (Elfenroads.Model.game.currentPhase is DrawCards)){
            Debug.Log("Hey! How are you clicking this? It isn't DrawCards!");
            return;
        }
        if( Elfenroads.Control.isCurrentPlayer() ){
            Elfenroads.Control.drawRandomCard();
        }else{
            Debug.Log("Click invalid. Is it your turn? -> " + (Elfenroads.Control.isCurrentPlayer()));
        }
    }

    private void invalidMessage(string message){
        Debug.Log("Invalid message: " + message);
        GameObject invalidBox = Instantiate(invalidMovePrefab, Input.mousePosition, Quaternion.identity, DrawCardsCanvas.transform);
        invalidBox.GetComponent<TMPro.TMP_Text>().text = message;
        Destroy(invalidBox, 2f);
    }
}
