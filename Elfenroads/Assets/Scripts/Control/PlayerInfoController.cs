using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Models;
using System;

public class PlayerInfoController : MonoBehaviour
{
    //This controller serves to show player inventory information.
    public GameObject opponentTabPrefab;
    public GameObject inventoryWindow;
    public RectTransform opponentsLayout;
    public RectTransform playerCounters;
    public RectTransform playerCards;
    public TMPro.TMP_Text playerName;
    public TMPro.TMP_Text playerStats;
    public InfoWindowController infoWindowController;

    [Header("CounterPrefabs")]
    public GameObject cloudCounterPrefab;
    public GameObject cycleCounterPrefab;
    public GameObject dragonCounterPrefab;
    public GameObject landCounterPrefab;
    public GameObject pigCounterPrefab;
    public GameObject trollCounterPrefab;
    public GameObject unicornCounterPrefab;
    public GameObject backOfCounterPrefab;
    [Header("CardPrefabs")]
    public GameObject cloudCardPrefab;
    public GameObject cycleCardPrefab;
    public GameObject dragonCardPrefab;
    public GameObject pigCardPrefab;
    public GameObject raftCardPrefab;
    public GameObject trollCardPrefab;
    public GameObject unicornCardPrefab;
    public GameObject backOfCardPrefab;

    [HideInInspector]
    public bool windowOpen = false;

    private bool onOpenCameraLock;
    private bool onOpenDraggableLock;


    //Called on initial game setup.
    public void createOpponentPrefabs(List<Player> players){
        foreach(Player p in players){
            if(p.name == GameObject.Find("SessionInfo").GetComponent<SessionInfo>().getClient().clientCredentials.username){
                continue;
            }
            GameObject instantiatedTab = Instantiate(opponentTabPrefab, opponentsLayout);
            instantiatedTab.GetComponent<OpponentPlayerView>().setAndSubscribeToModel(p);

            switch(p.boot.color){
                case Models.Color.RED:{
                    instantiatedTab.GetComponent<Image>().color = new Color32(221, 76, 76, 255);
                    break;
                }
                case Models.Color.BLUE:{
                    instantiatedTab.GetComponent<Image>().color = new Color32(50, 127, 210, 255);
                    break;
                }
                case Models.Color.GREEN:{
                    instantiatedTab.GetComponent<Image>().color = new Color32(46, 154, 71, 255);
                    break;
                }
                case Models.Color.PURPLE:{
                    instantiatedTab.GetComponent<Image>().color = new Color32(160, 45, 193, 255);
                    break;
                }
                case Models.Color.YELLOW:{
                    instantiatedTab.GetComponent<Image>().color = new Color32(253, 223, 20, 255);
                    break;
                }
                case Models.Color.BLACK:{
                    //Black is a little more involved, since we need to make the text white.
                    instantiatedTab.GetComponent<Image>().color = new Color32(42, 42, 44, 255);
                    instantiatedTab.transform.GetChild(0).gameObject.GetComponent<TMPro.TMP_Text>().color = new Color32(243, 243, 243, 255);
                    instantiatedTab.transform.GetChild(1).gameObject.GetComponent<TMPro.TMP_Text>().color = new Color32(243, 243, 243, 255);
                    break;
                }
                default: Debug.Log("Invalid color!"); break;
            }
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(opponentsLayout);
    }
    

    public void openAndSetupWindow(Player targetPlayer){
        if(inventoryWindow.activeSelf || infoWindowController.isOpen){
            //Do nothing if the window is already active or if the info window is open.
            return;
        }
        //We'll need to save the previous lock states for use when closing the window.
        onOpenCameraLock = Elfenroads.Control.cameraLocked;
        onOpenDraggableLock = Elfenroads.Control.draggablesLocked;
        Elfenroads.Control.LockCamera?.Invoke(null, EventArgs.Empty);
        Elfenroads.Control.LockDraggables?.Invoke(null, EventArgs.Empty);

        //Now, we can set the window to active and load up the GridLayoutGroups depending on whether or not we are the current player or not.
        inventoryWindow.SetActive(true);
        
        playerName.text = targetPlayer.name + "'s Inventory!";
        string toPresent = "Points: " + targetPlayer.inventory.townPieces.Count;

        if(targetPlayer.id == Elfenroads.Model.game.startingPlayer.id){
            toPresent = toPresent + "   -   StartingPlayer: Yes";
        }else{
            toPresent = toPresent + "   -   StartingPlayer: No";
        }

        if((targetPlayer.destinationTown != null) && (targetPlayer.id == Elfenroads.Control.getThisPlayer().id)){
            toPresent = toPresent + "   -   DestinationTown: " + targetPlayer.destinationTown.name;
        }
        playerStats.text = toPresent;

        if(targetPlayer.id == Elfenroads.Control.getThisPlayer().id){
            loadCounters(targetPlayer, true);
            loadCards(targetPlayer,true);
        }else{
            loadCounters(targetPlayer,false);
            loadCards(targetPlayer,false);
        }
        setWindowTint(targetPlayer.boot.color);
        windowOpen = true;
    }

    //Loads counters into the layoutGroup, with counters face up or down depending on input.
    public void loadCounters(Player targetPlayer, bool isThisPlayer){
        foreach(Counter c in targetPlayer.inventory.counters){

            //If we are not "thisPlayer" and the counter is not face up, create a face down counter.
            if((!c.isFaceUp) && !isThisPlayer){
                createAndAddToLayout(backOfCounterPrefab, playerCounters);
                continue;
            }

            switch(c){
                case TransportationCounter tc:
                {
                    switch(tc.transportType){
                        case TransportType.Dragon:
                        {  
                            createAndAddToLayout(dragonCounterPrefab, playerCounters);
                            break;
                        }
                        case TransportType.ElfCycle:
                        {
                            createAndAddToLayout(cycleCounterPrefab, playerCounters);
                            break;
                        }
                        case TransportType.MagicCloud:
                        {
                            createAndAddToLayout(cloudCounterPrefab, playerCounters);
                            break;
                        }
                        case TransportType.TrollWagon:
                        {
                            createAndAddToLayout(trollCounterPrefab, playerCounters);
                            break;
                        }
                        case TransportType.GiantPig:
                        {
                            createAndAddToLayout(pigCounterPrefab, playerCounters);
                            break;
                        }
                        case TransportType.Unicorn:
                        {
                            createAndAddToLayout(unicornCounterPrefab, playerCounters);
                            break;
                        }
                        default: Debug.Log("Error: TransportationCounter type invalid.") ; break;
                    }
                    break;
                }
                case MagicSpellCounter msc:
                {
                    Debug.Log("Not implemented!");
                    break;
                }
                case GoldCounter gc:
                {
                    Debug.Log("Not implemented!");
                    break;
                }
                case ObstacleCounter oc:{
                    if(oc.obstacleType == ObstacleType.Land){
                        createAndAddToLayout(landCounterPrefab, playerCounters);
                    }
                    break;
                }
                default: Debug.Log("Counter is of undefined type!") ; break;
            }
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(playerCounters);
    }

    public void loadCards(Player targetPlayer, bool isThisPlayer){
        foreach(Card c in targetPlayer.inventory.cards){

            //If we are not "thisPlayer", just create a facedown card.
            if(!isThisPlayer){
                createAndAddToLayout(backOfCardPrefab, playerCards);
                continue;
            }

            switch(c){
                case TravelCard tc:
                {
                    switch(tc.transportType){
                        case TransportType.Dragon:
                        {  
                            createAndAddToLayout(dragonCardPrefab, playerCards);
                            break;
                        }
                        case TransportType.ElfCycle:
                        {
                            createAndAddToLayout(cycleCardPrefab, playerCards);
                            break;
                        }
                        case TransportType.MagicCloud:
                        {
                            createAndAddToLayout(cloudCardPrefab, playerCards);
                            break;
                        }
                        case TransportType.TrollWagon:
                        {
                            createAndAddToLayout(trollCardPrefab, playerCards);
                            break;
                        }
                        case TransportType.GiantPig:
                        {
                            createAndAddToLayout(pigCardPrefab, playerCards);
                            break;
                        }
                        case TransportType.Unicorn:
                        {
                            createAndAddToLayout(unicornCardPrefab, playerCards);
                            break;
                        }
                        case TransportType.Raft:{
                            createAndAddToLayout(raftCardPrefab, playerCards);
                            break;
                        }
                        default: Debug.Log("Error: TransportationCard type invalid.") ; break;
                    }
                    break;
                }
                case WitchCard wc:
                {
                    Debug.Log("Not implemented!");
                    break;
                }
                case GoldCard gc:
                {
                    Debug.Log("Not implemented!");
                    break;
                }
                default: Debug.Log("Card is of undefined type!") ; break;
            }
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(playerCards);
    }

    private void createAndAddToLayout(GameObject prefab, RectTransform targetLayout){
        GameObject instantiatedCounter = Instantiate(prefab, targetLayout);
        instantiatedCounter.transform.SetParent(targetLayout, false);
        return;
    }

    public void closeWindow(){
        //First, set the locks to what they were.
        if(!onOpenCameraLock){
            Elfenroads.Control.UnlockCamera?.Invoke(null, EventArgs.Empty);
        }
        if(!onOpenDraggableLock){
            Elfenroads.Control.UnlockDraggables?.Invoke(null, EventArgs.Empty);
        }

        //Next, clear the layouts and disable the window.
        clearLayouts();
        inventoryWindow.SetActive(false);
        windowOpen = false;
    }

    private void clearLayouts(){
        foreach(Transform child in playerCards){
            child.SetParent(null);
            DestroyImmediate(child.gameObject);
        }
        playerCards.DetachChildren();

        foreach(Transform child in playerCounters){
            child.SetParent(null);
            DestroyImmediate(child.gameObject);
        }
        playerCounters.DetachChildren();
    }

    private void setWindowTint(Models.Color inputColor){
        switch(inputColor){
                case Models.Color.RED:{
                    inventoryWindow.GetComponent<Image>().color = new Color32(221, 76, 76, 255);
                    foreach(Transform child in inventoryWindow.transform){
                        if(child.GetComponent<TMPro.TMP_Text>() != null){
                            child.GetComponent<TMPro.TMP_Text>().color = new Color32(1, 1, 1, 255);
                        }
                    }
                    playerCards.GetComponent<Image>().color = new Color32(192, 110, 110, 255);
                    playerCounters.GetComponent<Image>().color = new Color32(192, 110, 110, 255);
                    break;
                }
                case Models.Color.BLUE:{
                    inventoryWindow.GetComponent<Image>().color = new Color32(50, 127, 210, 255);
                    foreach(Transform child in inventoryWindow.transform){
                        if(child.GetComponent<TMPro.TMP_Text>() != null){
                            child.GetComponent<TMPro.TMP_Text>().color = new Color32(1, 1, 1, 255);
                        }
                    }
                    playerCards.GetComponent<Image>().color = new Color32(131, 159, 197, 255);
                    playerCounters.GetComponent<Image>().color = new Color32(131, 159, 197, 255);
                    break;
                }
                case Models.Color.GREEN:{
                    inventoryWindow.GetComponent<Image>().color = new Color32(46, 154, 71, 255);
                    foreach(Transform child in inventoryWindow.transform){
                        if(child.GetComponent<TMPro.TMP_Text>() != null){
                            child.GetComponent<TMPro.TMP_Text>().color = new Color32(1, 1, 1, 255);
                        }
                    }
                    playerCards.GetComponent<Image>().color = new Color32(112, 207, 157, 255);
                    playerCounters.GetComponent<Image>().color = new Color32(112, 207, 157, 255);
                    break;
                }
                case Models.Color.PURPLE:{
                    inventoryWindow.GetComponent<Image>().color = new Color32(160, 45, 193, 255);
                    foreach(Transform child in inventoryWindow.transform){
                        if(child.GetComponent<TMPro.TMP_Text>() != null){
                            child.GetComponent<TMPro.TMP_Text>().color = new Color32(1, 1, 1, 255);
                        }
                    }
                    playerCards.GetComponent<Image>().color = new Color32(219, 111, 203, 255);
                    playerCounters.GetComponent<Image>().color = new Color32(219, 111, 203, 255);
                    break;
                }
                case Models.Color.YELLOW:{
                    inventoryWindow.GetComponent<Image>().color = new Color32(253, 223, 20, 255);
                    foreach(Transform child in inventoryWindow.transform){
                        if(child.GetComponent<TMPro.TMP_Text>() != null){
                            child.GetComponent<TMPro.TMP_Text>().color = new Color32(1, 1, 1, 255);
                        }
                    }
                    playerCards.GetComponent<Image>().color = new Color32(255, 158, 59, 255);
                    playerCounters.GetComponent<Image>().color = new Color32(255, 158, 59, 255);
                    break;
                }
                case Models.Color.BLACK:{
                    //Black is a little more involved, since we need to make the text white.
                    inventoryWindow.GetComponent<Image>().color = new Color32(42, 42, 44, 255);
                    foreach(Transform child in inventoryWindow.transform){
                        if(child.GetComponent<TMPro.TMP_Text>() != null){
                            child.GetComponent<TMPro.TMP_Text>().color = new Color32(243, 243, 243, 255);
                        }
                    }
                    playerCards.GetComponent<Image>().color = new Color32(152, 152, 152, 255);
                    playerCounters.GetComponent<Image>().color = new Color32(152, 152, 152, 255);
                    break;
                }
                default: Debug.Log("Invalid color!"); break;
            }
    }
}
