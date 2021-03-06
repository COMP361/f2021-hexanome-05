using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Models;
using Views;

public class FaceUpCountersView : MonoBehaviour, GuidHelperContainer
{
    //Need public UI elements here, which will actually display the model (DrawCountersModel, below) to the screen.
    //Eg. public GridLayoutGroup layout; and several prefabs for the different types of counters.

    private DrawCountersController myController;
    private GameObject sessionInfo;

    public GameObject dragonCounterPrefab;
    public GameObject trollCounterPrefab;
    public GameObject cloudCounterPrefab;
    public GameObject cycleCounterPrefab;
    public GameObject unicornCounterPrefab;
    public GameObject pigCounterPrefab;
    public GameObject landObstaclePrefab;


    void Start(){
        myController = GameObject.Find("DrawCountersController").GetComponent<DrawCountersController>();
        sessionInfo = GameObject.Find("SessionInfo"); //Why?
    }

    //This should have been in the Controller. Don't do this for similar phases.
    public void updateFaceUpCounters(DrawCounters drawCountersModel) {
        //Here, needs to add counters to the GridLayoutGroup according to the model. Instantiated Counters must also have their "GuidViewHelper" component's "Guid" fields set appropriately.
        Debug.Log("Model was updated!");
        //First, destroy all children (mwahahah)
        foreach(Transform child in transform){
            child.SetParent(null);
            DestroyImmediate(child.gameObject);
        }
        transform.DetachChildren();

        //Now, loop through the counters of the model, instantiating appropriate counters each time.
        Debug.Log("Updated drawcounters!");
        foreach(Counter c in Elfenroads.Model.game.faceUpCounters){
            switch(c){
                case TransportationCounter tc:
                {
                    //Debug.Log("Transport type of " + c.id + " is: " + tc.transportType);
                    switch(tc.transportType){
                        case TransportType.Dragon:
                        {  
                           GameObject instantiatedCounter = Instantiate(dragonCounterPrefab, this.transform);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setGuid(c.id);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setContainer(this);
                            break;
                        }
                        case TransportType.ElfCycle:
                        {
                            GameObject instantiatedCounter = Instantiate(cycleCounterPrefab, this.transform);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setGuid(c.id);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setContainer(this);
                            break;
                        }
                        case TransportType.MagicCloud:
                        {
                            GameObject instantiatedCounter = Instantiate(cloudCounterPrefab, this.transform);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setGuid(c.id);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setContainer(this);
                            break;
                        }
                        case TransportType.TrollWagon:
                        {
                            GameObject instantiatedCounter = Instantiate(trollCounterPrefab, this.transform);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setGuid(c.id);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setContainer(this);
                            break;
                        }
                        case TransportType.GiantPig:
                        {
                            GameObject instantiatedCounter = Instantiate(pigCounterPrefab, this.transform);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setGuid(c.id);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setContainer(this);
                            break;
                        }
                        case TransportType.Unicorn:
                        {
                            GameObject instantiatedCounter = Instantiate(unicornCounterPrefab, this.transform);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setGuid(c.id);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setContainer(this);
                            break;
                        }
                        default: Debug.Log("Model transportation counter of type raft! This is not allowed!") ; break;
                    }
                    break;
                }
                case MagicSpellCounter msc:
                {
                    Debug.Log("Elfengold - This should never happen!");
                    break;
                }
                case GoldCounter gc:
                {
                    Debug.Log("Elfengold - This should never happen!");
                    break;
                }
                case ObstacleCounter obc:
                {
                    GameObject instantiatedCounter = Instantiate(landObstaclePrefab, this.transform);
                    instantiatedCounter.GetComponent<GuidViewHelper>().setGuid(c.id);
                    instantiatedCounter.GetComponent<GuidViewHelper>().setContainer(this);
                    break;
                }
                default: Debug.Log("Counter is of undefined type!") ; break;
            }
        }
    }

    //Called by the Counter GridElements (those instantiated by this script, and which will be made children of the GridLayoutGroup). Will be used to identify which counter was clicked.
    public void GUIClicked(GameObject clickedCounter){

        if(! (Elfenroads.Model.game.currentPhase is DrawCounters)){
            Debug.Log("Yay!");
            return;
        }
        
        if( (Elfenroads.Control.isCurrentPlayer()) && Elfenroads.Model.game.currentPhase is DrawCounters){
            myController.validateDrawCounter(clickedCounter);
        }else{
            Debug.Log("Click invalid. Is it your turn? -> " + (Elfenroads.Control.isCurrentPlayer()));
            Debug.Log("If so, then it is because the currentPhase is " + Elfenroads.Model.game.currentPhase);
        }
    }

    //Called by some other button or object representing a random draw, which has an Event trigger leading here.
    public void RandomCounterClicked(){
        if(! (Elfenroads.Model.game.currentPhase is DrawCounters)){
            Debug.Log("Model is null!");
            return;
        }
        if( (Elfenroads.Control.isCurrentPlayer()) && Elfenroads.Model.game.currentPhase is DrawCounters){
            Debug.Log("Random draw selected!");
            myController.validateDrawRandomCounter();

        }else{
            Debug.Log("Click invalid. Is it your turn? -> " + (Elfenroads.Control.isCurrentPlayer()));
            Debug.Log("If so, then it is because the currentPhase is " + Elfenroads.Model.game.currentPhase);
        }
    }

    void Update(){
        //*** If game slows this might be it.
        float width = this.gameObject.GetComponent<RectTransform>().rect.width;
        Vector2 newSize = new Vector2(width / 5.75f, width / 5.75f);
        this.gameObject.GetComponent<GridLayoutGroup>().cellSize = newSize;
    }
}
