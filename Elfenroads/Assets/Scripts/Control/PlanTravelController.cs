using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Models;

public class PlanTravelController : MonoBehaviour
{
    //Text box or something here, to alert player of invalid moves.
    public GameObject invalidMovePrefab;
    public GameObject messagePrefab;
    public GameObject PlanTravelCanvas;
    public PlayerInfoController playerInfoController;
    public InfoWindowController infoWindowController;

    public GameObject doubleButton;
    public GameObject exchangeButton;
    private bool usingDouble = false;
    private Guid currentDouble = Guid.Empty;
    private bool usingExchange = false;
    private Guid currentExchange = Guid.Empty;
    private TransportationCounter firstCounter = null;
    private Road firstRoad = null;


    public void passTurn(){
        if(playerInfoController.windowOpen || infoWindowController.isOpen){
            invalidMessage("Close any open windows first!");
            return;
        }
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
        bool isGold = false;

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
                passedObstacle = ObstacleType.Sea;
                break;
            }
            // case "Exchange":{ //Won't be used in this scenario.
            //     Debug.Log("Elfengold, not implemented!");
            //     break;
            // }
            // case "Double":{
            //     Debug.Log("Elfengold, not implemented!");
            //     break;
            // }
            case "Gold":{
                isGold = true;
                break;
            }
            default: Debug.Log("Illegal name on DragScript!"); break;
        }

        //Now that we know what we have, we can go case by case.
        if((passedObstacle == null) && (passedCounter != null)){ //In this case, a normal TransportCounter was passed in.
            bool ownsIt = false;
            Guid guidToPass = Guid.Empty;

            //Check that the passed-in counter is compatible with the road.
            if(!compatibleWithRoad(passedCounter, road.roadType)){
                invalidMessage("Incompatible counter!");
                return;
            }
            //Verify that the road has no counters on it (with double case first)
            if(usingDouble && !hasDouble(road)){
                //Here, we're using a double spell so check that we have a second counter.
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
                Elfenroads.Control.playDoubleCounter(currentDouble, guidToPass, road.id);
                turnOffSpells();
                return;
            }
            if(road.counters.Count != 0){
                invalidMessage("Road occupied!");
                return;
            }

            //Finally, verify that the Player owns a counter of that type.
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
            turnOffSpells();

        }else if((passedObstacle != null) && (passedCounter == null)){ //In this case, an obstacle was passed in.
            //First, check if we're in elfengold or not.
            if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.Elfengold)){
                if(road.counters.Count == 0 && road.roadType != TerrainType.Stream && road.roadType != TerrainType.Lake){
                    invalidMessage("Road has no counter!");
                    return;
                }else if(hasObstacleOrGold(road)){
                    if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.Elfengold)){
                        invalidMessage("Obstacle or GoldCounter already exists!");
                        return;
                    }
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
                turnOffSpells();
            }else{
                if(road.counters.Count == 0){
                    invalidMessage("Road has no counter!");
                    return;
                }else if(hasObstacleOrGold(road)){
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
                turnOffSpells();
            }
        }else if(isGold == true){
            Debug.Log("Gold counter dragged!");
            if(road.counters.Count == 0){
                invalidMessage("Road has no counter!");
                return;
            }else if(hasObstacleOrGold(road)){
                invalidMessage("Obstacle or GoldCounter already exists!");
            }
            //Then, verify that the Player has a gold counter.
                bool ownsIt = false;
                Guid guidToPass = Guid.Empty;
                foreach(Counter c in Elfenroads.Control.getThisPlayer().inventory.counters){
                    if(c is GoldCounter){
                        ownsIt = true;
                        guidToPass = c.id;
                    }
                }
                if(! ownsIt){
                    invalidMessage("Missing counter!");
                    return;
                }
                //Hooray! Move is valid and we can send the command.
                Elfenroads.Control.placeCounter(guidToPass, road.id);
                turnOffSpells();
        }else{
            Debug.Log("Double-check draggable names!");
        }
    }

    private bool hasObstacleOrGold(Road road){
        bool result = false;
        foreach(Counter c in road.counters){
            if(c is ObstacleCounter || c is GoldCounter){
                result = true;
            }
        }
        return result;
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

    public void toggleDoubleSpell(){
        if(usingExchange){
            invalidMessage("You can only use one spell at a time!");
            return;
        }
        if(usingDouble){
            usingDouble = false;
            return;
        }

        bool ownsIt = false;
            foreach(Counter c in Elfenroads.Control.getThisPlayer().inventory.counters){
                if(c is MagicSpellCounter){
                    if(((MagicSpellCounter) c).spellType == SpellType.Double){
                        ownsIt = true;
                        currentDouble = c.id;
                    }
                }
            }
            if(! ownsIt){
                invalidMessage("No Double Spell owned!");
                return;
            }else{
                usingDouble = true;
            }
    }
    
    private void turnOffSpells(){
        usingDouble = false;
        usingExchange = false;
        currentDouble = Guid.Empty;
        currentExchange = Guid.Empty;
        firstCounter = null;
        firstRoad = null;
        //*** Also stop highlighting the spells here.
    }

    private bool hasDouble(Road road){
        int transpCounters = 0;
        foreach(Counter c in road.counters){
            if(c is TransportationCounter){
                transpCounters++;
            }
        }

        return transpCounters > 1;
    }

    public void toggleExchangeSpell(){
        if(usingDouble){
            invalidMessage("You can only use one spell at a time!");
            return;
        }
        if(usingExchange){
            usingExchange = false;
            currentExchange = Guid.Empty;
            firstCounter = null;
            firstRoad = null;
            return;
        }

        bool ownsIt = false;
            foreach(Counter c in Elfenroads.Control.getThisPlayer().inventory.counters){
                if(c is MagicSpellCounter){
                    if(((MagicSpellCounter) c).spellType == SpellType.Exchange){
                        ownsIt = true;
                        currentExchange = c.id;
                    }
                }
            }
            if(! ownsIt){
                invalidMessage("No Exchange Spell owned!");
                return;
            }else{
                usingExchange = true;
            }
    }

    public void counterClicked(Counter clickedCounter, Road counterRoad){
        if(!usingExchange){
            invalidMessage("Swapping requires an exchange spell!");
            return;
        }
        if(!(clickedCounter is TransportationCounter)){
            invalidMessage("Can only swap transportation counters!");
            return;
        }
        TransportationCounter tc = (TransportationCounter) clickedCounter;
        //Case where the first counter has been selected.
        if(firstCounter == null || firstRoad == null){
            firstCounter = tc;
            firstRoad = counterRoad;
            affirmMessage("Counter selected for swap!");
            return;
        }else{ //Case where the second counter has been selected.
            //Find out if the second counter is on the same road as the first.

            //Figure out if the first counter is compatible with the second counter's road.
            if(!compatibleWithRoad(firstCounter.transportType, counterRoad.roadType)){
                invalidMessage("Invalid swap!");
                return;
            }
            //Figure out if the second counter is compatible with the first counter's road.
            if(!compatibleWithRoad(tc.transportType, firstRoad.roadType)){
                invalidMessage("Invalid swap!");
                return;
            }
            //If we make it here, the swap is valid. *** Talk with server to figure out parameters) ***
            Elfenroads.Control.playExchangeCounter(firstRoad.id, firstCounter.id, counterRoad.id, clickedCounter.id);
            turnOffSpells();
        }
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

    private void affirmMessage(string message){
        GameObject validBox = Instantiate(invalidMovePrefab, Input.mousePosition, Quaternion.identity, PlanTravelCanvas.transform);
        validBox.GetComponent<TMPro.TMP_Text>().text = message;
        validBox.GetComponent<TMPro.TMP_Text>().color = new Color32(77, 255, 0, 255);
        Destroy(validBox, 2f);
    }

    //Just here to test functionality.
    public void buttonTesterDouble(){
        if(usingDouble){
            usingDouble = false;
        }else{
            usingDouble = true;
        }
    }

    public void buttonTesterExchange(){
        if(usingExchange){
            usingExchange = false;
        }else{
            usingExchange = true;
        }
    }

    Color32 normalColor = new Color32(255, 255, 255, 255);
    Color32 spellColor = new Color32(166, 64, 229, 255);
    UnityEngine.Color lerpedColorDouble = new Color32(255, 255, 255, 255);
    UnityEngine.Color lerpedColorExchange = new Color32(255, 255, 255, 255);
    void Update(){
        if(usingDouble){
            lerpedColorDouble = UnityEngine.Color.Lerp(normalColor, spellColor, Mathf.PingPong(Time.time, 1));
            doubleButton.GetComponent<Image>().color = lerpedColorDouble;
        }else{
            doubleButton.GetComponent<Image>().color = normalColor;
        }
        if(usingExchange){
            lerpedColorExchange = UnityEngine.Color.Lerp(normalColor, spellColor, Mathf.PingPong(Time.time, 1));
            exchangeButton.GetComponent<Image>().color = lerpedColorExchange;
        }else{
            exchangeButton.GetComponent<Image>().color = normalColor;
        }
    }
}
