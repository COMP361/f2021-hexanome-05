using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Views;
using Models;

public class DrawCardsController : MonoBehaviour, GuidHelperContainer
{
    public RectTransform cardsLayout;
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
        Debug.Log("Model was updated!");
        //First, destroy all children (mwahahah)
        foreach(Transform child in cardsLayout){
            child.SetParent(null);
            DestroyImmediate(child.gameObject);
        }
        cardsLayout.DetachChildren();

        //Now, loop through the cards of the model, instantiating appropriate cards each time.
        foreach(Card c in Elfenroads.Model.game.faceUpCards){
            switch(c){
                case TravelCard tc:
                {
                    //Debug.Log("Transport type of " + c.id + " is: " + tc.transportType);
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
    }

    //Handles the (very little) needed validation for drawing a card.
    public void GUIClicked(GameObject clickedCard){
        // if(! (Elfenroads.Model.game.currentPhase is DrawCards)){
        //     Debug.Log("Yay!");
        //     return;
        // }
        if( Elfenroads.Control.isCurrentPlayer() ){
            Elfenroads.Control.drawCard(clickedCard);
        }else{
            Debug.Log("Click invalid. Is it your turn? -> " + (Elfenroads.Control.isCurrentPlayer()));
        }
    }



    public void takeGoldCards(){
        // if(! (Elfenroads.Model.game.currentPhase is DrawCards)){
        //     Debug.Log("Yay!");
        //     return;
        // }
        if( Elfenroads.Control.isCurrentPlayer() ){
            Elfenroads.Control.takeGoldCards();
        }else{
            Debug.Log("Click invalid. Is it your turn? -> " + (Elfenroads.Control.isCurrentPlayer()));
        }
    }

    public void takeRandomCard(){
        // if(! (Elfenroads.Model.game.currentPhase is DrawCards)){
        //     Debug.Log("Yay!");
        //     return;
        // }
        if( Elfenroads.Control.isCurrentPlayer() ){
            Elfenroads.Control.drawRandomCard();
        }else{
            Debug.Log("Click invalid. Is it your turn? -> " + (Elfenroads.Control.isCurrentPlayer()));
        }
    }
}
