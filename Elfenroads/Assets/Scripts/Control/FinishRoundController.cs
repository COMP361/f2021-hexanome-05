using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firesplash.UnityAssets.SocketIO;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using Views;
using Models;

public class FinishRoundController : MonoBehaviour, GuidHelperContainer
{
    public GameObject FinishRoundCanvas;
    public GameObject mainWindow;
    public GameObject waitingWindow;
    public GameObject toggleMapButton;
    public GameObject invalidMovePrefab;
    public RectTransform potentialDiscardLayoutGroup;
    public RectTransform countersToDiscardLayoutGroup;
    public TMPro.TMP_Text topPrompt;

    public GameObject dragonCounterPrefab;
    public GameObject trollCounterPrefab;
    public GameObject cloudCounterPrefab;
    public GameObject cycleCounterPrefab;
    public GameObject unicornCounterPrefab;
    public GameObject pigCounterPrefab;
    public GameObject landCounterPrefab;
    public GameObject seaCounterPrefab;
    public GameObject doubleCounterPrefab;
    public GameObject exchangeCounterPrefab;
    public GameObject goldCounterPrefab;

    private List<GameObject> countersToKeep;
    private List<GameObject> countersToDiscard;

    public void initialSetup(Player thisPlayer){
        if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.Elfengold)){
            topPrompt.text = "Choose counters to discard until only two are left:";
        }

        //If this player has <= 1 counters, show the "waiting window". Otherwise, show the "mainWindow" and allow them to discard counters. In elfengold, player must have <= 2 counters.
        int countNotIncludeObstacle = 0;
        int countIncludeObstacle = 0;
        foreach(Counter c in thisPlayer.inventory.counters){
            countIncludeObstacle++;
            switch(c){
                case TransportationCounter tc:{
                    countNotIncludeObstacle++;
                    break;
                }
                default: break;
            }
        }
        Debug.Log("Counters including obstacles: " + countIncludeObstacle);
        Debug.Log("Counters not including obstacles: " + countNotIncludeObstacle);
        if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.Elfengold) && countIncludeObstacle <= 2){
            mainWindow.SetActive(false);
            toggleMapButton.SetActive(false);
            waitingWindow.SetActive(true);
            return;
        }else if(! (Elfenroads.Model.game.variant.HasFlag(Game.Variant.Elfengold)) && countNotIncludeObstacle <= 1){
            mainWindow.SetActive(false);
            toggleMapButton.SetActive(false);
            waitingWindow.SetActive(true);
            return;
        }else{
            countersToDiscard = new List<GameObject>();
            countersToKeep = new List<GameObject>();
            foreach(Counter c in thisPlayer.inventory.counters){
                switch(c){
                    case TransportationCounter tc:
                    {
                        switch(tc.transportType){
                            case TransportType.Dragon:
                            {  
                                createAndAddToLayout(dragonCounterPrefab, c);
                                break;
                            }
                            case TransportType.ElfCycle:
                            {
                                createAndAddToLayout(cycleCounterPrefab, c);
                                break;
                            }
                            case TransportType.MagicCloud:
                            {
                                createAndAddToLayout(cloudCounterPrefab, c);
                                break;
                            }
                            case TransportType.TrollWagon:
                            {
                                createAndAddToLayout(trollCounterPrefab, c);
                                break;
                            }
                            case TransportType.GiantPig:
                            {
                                createAndAddToLayout(pigCounterPrefab, c);
                                break;
                            }
                            case TransportType.Unicorn:
                            {
                                createAndAddToLayout(unicornCounterPrefab, c);
                                break;
                            }
                            default: Debug.Log("Error: TransportationCounter type invalid.") ; break;
                        }
                        break;
                    }
                    case MagicSpellCounter msc:
                    {
                        if(msc.spellType == SpellType.Exchange){
                            createAndAddToLayout(exchangeCounterPrefab,c);
                        }else if(msc.spellType == SpellType.Double){
                            createAndAddToLayout(doubleCounterPrefab,c);
                        }
                        break;
                    }
                    case GoldCounter gc:
                    {
                        createAndAddToLayout(goldCounterPrefab,c);
                        break;
                    }
                    case ObstacleCounter oc:{
                        if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.Elfengold)){
                            if(oc.obstacleType == ObstacleType.Land){
                                createAndAddToLayout(landCounterPrefab,c);
                            }else if(oc.obstacleType == ObstacleType.Sea){
                                createAndAddToLayout(seaCounterPrefab,c);
                            }
                            break;
                        }
                        break;
                    }
                    default: Debug.Log("Counter is of undefined type!") ; break;
                }
            }

            //Now, we can enable the window.
            mainWindow.SetActive(true);
            toggleMapButton.SetActive(true);
            waitingWindow.SetActive(false);
        }
    }

    public void validateConfimSelection(){
        if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.Elfengold)){ //***Bug here maybe, involving flags? -> Seemed like it when Elfengold was checked first.
            if(countersToKeep.Count != 2){
                invalidMessage("You must keep exactly two counters!");
                return;
            }else{
                //In this case, the move is valid. Inform the Server, clear the lists and layouts disable the window, and enable the "waiting" window. 
                mainWindow.SetActive(false);
                waitingWindow.SetActive(true);
                
                List<Guid> results = new List<Guid>();
                foreach(GameObject counter in countersToDiscard){
                    results.Add(counter.GetComponent<GuidViewHelper>().getGuid());
                }
                Elfenroads.Control.finishRound(results);
                clearDiscard();
            }
        }else if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.Elfenland)){
            if(countersToKeep.Count != 1){
                invalidMessage("You must keep exactly one counter!");
                return;
            }else{
                //In this case, the move is valid. Inform the Server, clear the lists and layouts disable the window, and enable the "waiting" window. 
                mainWindow.SetActive(false);
                waitingWindow.SetActive(true);
                
                List<Guid> results = new List<Guid>();
                foreach(GameObject counter in countersToDiscard){
                    results.Add(counter.GetComponent<GuidViewHelper>().getGuid());
                }
                Elfenroads.Control.finishRound(results);
                clearDiscard();
            }
        }else{
            invalidMessage("ERROR: BAD VARIANT - RESTART GAME");
        }
    }

    private void createAndAddToLayout(GameObject prefab, Counter c){
        GameObject instantiatedCounter = Instantiate(prefab, this.transform);
        instantiatedCounter.GetComponent<GuidViewHelper>().setGuid(c.id);
        instantiatedCounter.GetComponent<GuidViewHelper>().setContainer(this);
        if(!c.isFaceUp){
            instantiatedCounter.transform.GetChild(0).gameObject.GetComponent<TMPro.TMP_Text>().text = "Face down";
        }
        instantiatedCounter.transform.SetParent(potentialDiscardLayoutGroup, false);
        LayoutRebuilder.ForceRebuildLayoutImmediate(potentialDiscardLayoutGroup);
        countersToKeep.Add(instantiatedCounter);
        return;
    }

    public void GUIClicked(GameObject counterClicked){
        Debug.Log("In the cardClicked function.");
        foreach(GameObject counter in countersToKeep){
            if(counter.GetComponent<GuidViewHelper>().getGuid() == counterClicked.GetComponent<GuidViewHelper>().getGuid()){
                //Transfer this card from countersToKeep to ToDiscard
                Debug.Log("Transferring from countersToKeep to ToDiscard!");
                transferToDiscard(counter);
                return;
            }
        }
        foreach(GameObject counter in countersToDiscard){
            if(counter.GetComponent<GuidViewHelper>().getGuid() == counterClicked.GetComponent<GuidViewHelper>().getGuid()){
                //Transfer this card from countersToKeep to ToDiscard
                Debug.Log("Transferring from ToKeep to ToDiscard");
                transferToKeep(counter);
                return;
            }
        }

        
        //If we make it here, give an invalid message. ***Fatal error?
        invalidMessage("Could not find card in either layouts!");
        Debug.Log("Could not find card in either layouts!");
        return;
    }

    private void transferToDiscard(GameObject card){
        Debug.Log("In transferToDiscard!");
        card.transform.SetParent(countersToDiscardLayoutGroup, false);
        LayoutRebuilder.ForceRebuildLayoutImmediate(countersToDiscardLayoutGroup);
        LayoutRebuilder.ForceRebuildLayoutImmediate(potentialDiscardLayoutGroup);
        countersToKeep.Remove(card);
        countersToDiscard.Add(card);
        Debug.Log("Player cards: " + countersToKeep.Count);
        Debug.Log("Cards to discard: " + countersToDiscard.Count);
        return;
    }

    private void transferToKeep(GameObject card){
        Debug.Log("In transferToCards!");
        card.transform.SetParent(potentialDiscardLayoutGroup, false);
        LayoutRebuilder.ForceRebuildLayoutImmediate(countersToDiscardLayoutGroup);
        LayoutRebuilder.ForceRebuildLayoutImmediate(potentialDiscardLayoutGroup);
        countersToDiscard.Remove(card);
        countersToKeep.Add(card);
        Debug.Log("Player cards: " + countersToKeep.Count);
        Debug.Log("Cards to discard: " + countersToDiscard.Count);
        return;
    }

    private void clearDiscard(){
        countersToKeep.Clear();
        countersToDiscard.Clear();

        foreach(Transform child in potentialDiscardLayoutGroup){
            child.SetParent(null);
            DestroyImmediate(child.gameObject);
        }
        potentialDiscardLayoutGroup.DetachChildren();

        foreach(Transform child in countersToDiscardLayoutGroup){
            child.SetParent(null);
            DestroyImmediate(child.gameObject);
        }
        countersToDiscardLayoutGroup.DetachChildren();
        
        return;
    }

    private void invalidMessage(string message){
        Debug.Log("Invalid message: " + message);
        GameObject invalidBox = Instantiate(invalidMovePrefab, Input.mousePosition, Quaternion.identity, FinishRoundCanvas.transform);
        invalidBox.GetComponent<TMPro.TMP_Text>().text = message;
        Destroy(invalidBox, 2f);
    }

}
