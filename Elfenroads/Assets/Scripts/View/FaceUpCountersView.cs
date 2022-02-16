using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Models;

public class FaceUpCountersView : MonoBehaviour
{
    //Need public UI elements here, which will actually display the model (DrawCountersModel, below) to the screen.
    //Eg. public GridLayoutGroup layout; and several prefabs for the different types of counters.

    private DrawCountersController myController;
    private GameObject sessionInfo;
    private DrawCounters drawCountersModel;


    void Start(){
        myController = GameObject.Find("DrawCountersController").GetComponent<DrawCountersController>();
        sessionInfo = GameObject.Find("SessionInfo");
    }

    public void setAndSubscribeToModel(DrawCounters inputDrawCounters){
         drawCountersModel = inputDrawCounters;
         drawCountersModel.Updated += onModelUpdated;
         //onModelUpdated(null, null);
     }

    void onModelUpdated(object sender, EventArgs e) {

    }

    //Called by the Counter GridElements (those instantiated by this script, and which will be made children of the GridLayoutGroup). Will be used to identify which counter was clicked.
    public void CounterClicked(GameObject clickedCounter){
        
        if( (drawCountersModel.currentPlayer.name == sessionInfo.GetComponent<SessionInfo>().getClient().clientCredentials.username) && Elfenroads.Model.game.currentPhase is DrawCounters){
            Debug.Log(clickedCounter);

            //Find the counter here

            myController.validateDrawCounter();

        }else{
            Debug.Log("Click invalid. Is it your turn? -> " + (drawCountersModel.currentPlayer.name == sessionInfo.GetComponent<SessionInfo>().getClient().clientCredentials.username));
            Debug.Log("If so, then it is because the currentPhase is " + Elfenroads.Model.game.currentPhase);
        }
    }

    //Called by some other button or object representing a random draw, which has an Event trigger leading here.
    public void RandomCounterClicked(){
        if( (drawCountersModel.currentPlayer.name == sessionInfo.GetComponent<SessionInfo>().getClient().clientCredentials.username) && Elfenroads.Model.game.currentPhase is DrawCounters){

            myController.validateDrawRandomCounter();

        }else{
            Debug.Log("Click invalid. Is it your turn? -> " + (drawCountersModel.currentPlayer.name == sessionInfo.GetComponent<SessionInfo>().getClient().clientCredentials.username));
            Debug.Log("If so, then it is because the currentPhase is " + Elfenroads.Model.game.currentPhase);
        }
    }
}
