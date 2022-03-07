using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;

public class PlanTravelController : MonoBehaviour
{
    //Text box or something here, to alert player of invalid moves.
    public GameObject invalidMovePrefab;
    public GameObject messagePrefab;
    public GameObject PlanTravelCanvas;

    public void passTurn(){
        //If a Player chose to pass, we need only verify we're in the right phase, and that we are the current player.
        if(! (Elfenroads.Model.game.currentPhase is PlanTravelRoutes)){
            //Inform player move is invalid.
            invalidMessage("Wrong phase!");
            Debug.Log("Invalid, not in the correct phase!");
            return;
        }else if(! Elfenroads.Control.isCurrentPlayer()){
            //Inform player they are not the current player.
            invalidMessage("Not your turn!");
            return;
        }else{
            //We're done, and can call Control.
            Elfenroads.Control.passTurn();
        }
    }

    public void validatePlaceCounter(string counterType, Road road){
        if(Elfenroads.Model.game == null){
            invalidMessage("Testing! No game exists!");
            return;
        }
        if(! (Elfenroads.Model.game.currentPhase is PlanTravelRoutes)){
            //Inform player move is invalid.
            invalidMessage("Wrong phase!");
            Debug.Log("Invalid, not in the correct phase!");
            return;
        }

        if(! Elfenroads.Control.isCurrentPlayer()){
            //Inform player they are not the current player.
            invalidMessage("Not your turn!");
            return;
        }


        //Here, we'll figure out which Counter was passed in based on the counterType parameter.
        TransportType? passedCounter = null;
        ObstacleType? passedObstacle = null;

        //First, we need to figure out which counter we're dealing with.
        switch(counterType){
            case "Cloud":{
                passedCounter = TransportType.MagicCloud;
                break;
            }
            case "Cycle":{
                passedCounter = TransportType.ElfCycle;
                break;
            }
            case "Dragon":{
                passedCounter = TransportType.Dragon;
                break;
            }
            case "LandObstacle":{
                passedObstacle = ObstacleType.Land;
                break;
            }
            case "Pig":{
                passedCounter = TransportType.GiantPig;
                break;
            }
            case "Troll":{
                passedCounter = TransportType.TrollWagon;
                break;
            }
            case "Unicorn":{
                passedCounter = TransportType.Unicorn;
                break;
            }
            case "SeaObstacle":{
                Debug.Log("Elfengold, not implemented!");
                break;
            }
            case "Exchange":{
                Debug.Log("Elfengold, not implemented!");
                break;
            }
            case "Double":{
                Debug.Log("Elfengold, not implemented!");
                break;
            }
            case "Gold":{
                Debug.Log("Elfengold, not implemented!");
                break;
            }
            default: Debug.Log("Illegal name on DragScript!"); break;
        }

        //Now that we know what we have, we can go case by case.
        if((passedObstacle == null) && (passedCounter != null)){ //In this case, a normal TransportCounter was passed in.
            //First, verify that the road has no counters on it.
            if(road.counters.Count != 0){
                invalidMessage("Road occupied!");
                return;
            }
            //Next, check that the passed-in counter is compatible with the road.
            if(!compatibleWithRoad(passedCounter, road.roadType)){
                invalidMessage("Incompatible counter!");
                return;
            }

            //Finally, verify that the Player owns a counter of that type.
            bool ownsIt = false;
            Guid guidToPass = Guid.Empty;
            foreach(Counter c in Elfenroads.Control.getThisPlayer().inventory.counters){
                if(c is TransportationCounter){
                    if(((TransportationCounter) c).transportType == passedCounter){
                        ownsIt = true;
                        guidToPass = c.id;
                    }
                }
            }
            if(! ownsIt){
                invalidMessage("Missing counter!");
                return;
            }

            //Yahoo! Move is valid and we can send the command.
            Elfenroads.Control.placeCounter(guidToPass, road.id);

        }else if((passedObstacle != null) && (passedCounter == null)){ //In this case, an obstacle was passed in.
            //Verification here is simple. First, check that the road has a counter on it.
            if(road.counters.Count == 0){
                invalidMessage("Road has no counter!");
                return;
            }else if(road.counters.Count == 2){
                invalidMessage("Obstacle already exists!");
                return;
            }

            //Then, verify that the Player has an obstacle counter.
            bool ownsIt = false;
            Guid guidToPass = Guid.Empty;
            foreach(Counter c in Elfenroads.Control.getThisPlayer().inventory.counters){
                if(c is ObstacleCounter){
                    if(((ObstacleCounter) c).obstacleType == passedObstacle){
                        ownsIt = true;
                        guidToPass = c.id;
                    }
                }
            }
            if(! ownsIt){
                invalidMessage("Missing counter!");
                return;
            }

            //Hooray! Move is valid and we can send the command.
            Elfenroads.Control.placeCounter(guidToPass, road.id);
        }else{
            Debug.Log("Double-check draggable names!");
        }
    }

    private bool compatibleWithRoad(TransportType? counterType, TerrainType roadType){
        //A counter will be compatible with a road according to the transportation chart.
        switch(roadType){
            case TerrainType.Plain:{
                return ((counterType is TransportType.GiantPig) || (counterType is TransportType.Dragon) || (counterType is TransportType.ElfCycle) || (counterType is TransportType.MagicCloud) || (counterType is TransportType.TrollWagon));
            }
            case TerrainType.Forest:{
                return ((counterType is TransportType.GiantPig) || (counterType is TransportType.Dragon) || (counterType is TransportType.ElfCycle) || (counterType is TransportType.MagicCloud) || (counterType is TransportType.TrollWagon) || (counterType is TransportType.Unicorn));
            }
            case TerrainType.Mountain:{
                return ((counterType is TransportType.Unicorn) || (counterType is TransportType.Dragon) || (counterType is TransportType.ElfCycle) || (counterType is TransportType.MagicCloud) || (counterType is TransportType.TrollWagon));
            }
            case TerrainType.Desert:{
                return ((counterType is TransportType.Unicorn) || (counterType is TransportType.Dragon) || (counterType is TransportType.TrollWagon));
            }
            case TerrainType.Lake:{
                //For now, nothing can be placed on lakes/streams. Return false for both.
                return false;
            }
            case TerrainType.Stream:{
                return false;
            }
        }   
        return false; //If we somehow make it here, just return false;
    }

    public void playerTurnMessage(string message){
        GameObject messageBox = Instantiate(messagePrefab, PlanTravelCanvas.transform.position, Quaternion.identity, PlanTravelCanvas.transform);
        messageBox.transform.GetChild(1).gameObject.GetComponent<TMPro.TMP_Text>().text = message;
        Destroy(messageBox, 1.9f);
    }

    private void invalidMessage(string message){
        Debug.Log("Invalid message: " + message);
        GameObject invalidBox = Instantiate(invalidMovePrefab, Input.mousePosition, Quaternion.identity, PlanTravelCanvas.transform);
        invalidBox.GetComponent<TMPro.TMP_Text>().text = message;
        Destroy(invalidBox, 2f);
    }

}
