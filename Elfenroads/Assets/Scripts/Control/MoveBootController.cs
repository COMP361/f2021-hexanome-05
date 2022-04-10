using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Views;
using Models;
using System;


namespace Controls
{
    public class MoveBootController : MonoBehaviour, GuidHelperContainer
    { 
        public GameObject invalidMovePrefab;
        public GameObject messagePrefab;
        public GameObject MoveBootCanvas;
        public GameObject helperWindow;
        public GameObject EGHelperWindow;
        public TMPro.TMP_Text topText;
        public TMPro.TMP_Text bottomText;
        public GameObject goldReminder;
        public GameObject endTurnButton;
        public GameObject witchButton;
        public RectTransform topLayoutGroup;
        public RectTransform bottomLayoutGroup;
        public PlayerInfoController playerInfoController;
        public InfoWindowController infoWindowController;

        public GameObject dragonCardPrefab;
        public GameObject trollCardPrefab;
        public GameObject cloudCardPrefab;
        public GameObject cycleCardPrefab;
        public GameObject unicornCardPrefab;
        public GameObject pigCardPrefab;
        public GameObject raftCardPrefab;
        public GameObject witchCardPrefab;

        public List<GameObject> roadObjects;
        private List<RoadView> roadViews;

        private List<GameObject> topCards;
        private List<GameObject> bottomCards;
        private bool caravanMode = false;
        private int targetRoadCost = 0;
        private Town targetTown;

        private int goldAccrued = 0;
        private bool witchInUse = false;
        private Guid currentWitch = Guid.Empty;

        private Road targetRoad;

        void Start() {
            roadViews = new List<RoadView>();
            foreach (GameObject road in roadObjects) {
                roadViews.Add(road.GetComponent<RoadView>());
            }
            subscribeToRoadClickEvents();
            helperWindow.SetActive(false);
        }

        public void updateEGStuff(){
            if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.Elfengold)){
                if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.ElfenWitch)){
                    witchButton.SetActive(true);
                }
                MoveBootCanvas.transform.GetChild(0).gameObject.SetActive(true);
                MoveBootCanvas.transform.GetChild(0).gameObject.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Accrued Gold: " + goldAccrued;
            }
        }

        private void subscribeToRoadClickEvents() {
            foreach (RoadView roadView in roadViews) {
                roadView.RoadClicked += onRoadClicked;
            }
        }

        private void onRoadClicked(object sender, EventArgs args) { 
                targetRoad = (Road) sender;
                if(! Elfenroads.Control.isCurrentPlayer()){
                    //Inform player they are not the current player.
                    invalidMessage("Not your turn!");
                    return;
                }
                if(targetRoad.roadType == TerrainType.Lake || targetRoad.roadType == TerrainType.Stream || targetRoad.counters.Count < 1){
                    invalidMessage("Caravan can't be used here!");
                    return;
                }
                targetTown = null;
                if(targetRoad.start.boots.Contains(Elfenroads.Control.getThisPlayer().boot)){
                    targetTown = targetRoad.end;
                }else if(targetRoad.end.boots.Contains(Elfenroads.Control.getThisPlayer().boot)){
                    targetTown = targetRoad.start;
                }else{
                    invalidMessage("Boot not adjacent to this road!");
                    return;
                }

                if(helperWindow.activeSelf || playerInfoController.windowOpen || infoWindowController.isOpen || EGHelperWindow.activeSelf){
                    invalidMessage("You already have an open window!");
                    return;
                }else{
                    //We want to set up the helper window for a "caravan" move instead.
                    caravanMode = true;
                    loadPlayerCards();
                    endTurnButton.SetActive(false);
                    helperWindow.SetActive(true);
                    //Don't disable the witch here - but make the button unclickable.
                    Elfenroads.Control.LockCamera?.Invoke(null, EventArgs.Empty);
                    Elfenroads.Control.LockDraggables?.Invoke(null, EventArgs.Empty);
                    targetRoadCost = 3;
                    if(!witchInUse){ //If there's no witch, add the cost of an obstacle (if there is one)
                        foreach(Counter c in targetRoad.counters){
                            if(c is ObstacleCounter){
                                targetRoadCost = 4;
                            }
                        }
                    }
                    topText.text = "Select the " + targetRoadCost + " cards you will use for this caravan:";
                    bottomText.text = "Cards to use for the caravan: ";
                }
        }

        public void GUIClicked(GameObject cardClicked){
            foreach(GameObject card in topCards){
                if(card.GetComponent<GuidViewHelper>().getGuid() == cardClicked.GetComponent<GuidViewHelper>().getGuid()){
                    //Transfer this card from topCards to ToDiscard
                    Debug.Log("Transferring from topCards to ToDiscard!");
                    transferToBottom(card);
                    if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.Elfengold)){
                        updateHelperWindowCardCount();
                    }
                    return;
                }
            }
            foreach(GameObject card in bottomCards){
                if(card.GetComponent<GuidViewHelper>().getGuid() == cardClicked.GetComponent<GuidViewHelper>().getGuid()){
                    //Transfer this card from topCards to ToDiscard
                    Debug.Log("Transferring from topCards to ToDiscard");
                    transferToTop(card);
                    if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.Elfengold)){
                        updateHelperWindowCardCount();
                    }
                    return;
                }
            }
            
            //If we make it here, give an invalid message. ***Fatal error?
            invalidMessage("Could not find card in either layouts!");
            Debug.Log("Could not find card in either layouts!");
            return;
        }

        private void transferToBottom(GameObject card){
            Debug.Log("In transferToBottom!");
            card.transform.SetParent(bottomLayoutGroup, false);
            LayoutRebuilder.ForceRebuildLayoutImmediate(bottomLayoutGroup);
            LayoutRebuilder.ForceRebuildLayoutImmediate(topLayoutGroup);
            topCards.Remove(card);
            bottomCards.Add(card);
            Debug.Log("Player cards: " + topCards.Count);
            Debug.Log("Cards to discard: " + bottomCards.Count);
            return;
        }

        private void transferToTop(GameObject card){
            Debug.Log("In transferToTop!");
            card.transform.SetParent(topLayoutGroup, false);
            LayoutRebuilder.ForceRebuildLayoutImmediate(bottomLayoutGroup);
            LayoutRebuilder.ForceRebuildLayoutImmediate(topLayoutGroup);
            bottomCards.Remove(card);
            topCards.Add(card);
            Debug.Log("Player cards: " + topCards.Count);
            Debug.Log("Cards to discard: " + bottomCards.Count);
            return;
        }

        public void endTurnValidation(){
            if(playerInfoController.windowOpen || infoWindowController.isOpen || helperWindow.activeSelf || EGHelperWindow.activeSelf){
            invalidMessage("Close any open windows first!");
            return;
            }

            //First, check that it is the player's turn and it is the right phase.
            if(! (Elfenroads.Model.game.currentPhase is MoveBoot)){
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

            if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.Elfengold)){
                if(Elfenroads.Control.getThisPlayer().inventory.cards.Count > 15){
                    //Open the "discard cards" window.
                    loadPlayerCards();
                    //Now, we can enable the window.
                    endTurnButton.SetActive(false);
                    helperWindow.SetActive(true);
                    witchInUse = false;
                    currentWitch = Guid.Empty;
                    caravanMode = false;
                    Elfenroads.Control.LockCamera?.Invoke(null, EventArgs.Empty);
                    Elfenroads.Control.LockDraggables?.Invoke(null, EventArgs.Empty);
                    topText.text = "Choose cards to discard until fifteen or less are left (currently contains " + Elfenroads.Control.getThisPlayer().inventory.cards.Count + " cards):";
                    goldReminder.SetActive(true);
                    goldReminder.GetComponent<TMPro.TMP_Text>().text = "Don't worry - you'll still get your " + goldAccrued + " accrued gold!";
                    bottomText.text = "Discarded Cards: ";
                    
                    return;
                }
                EGHelperWindow.SetActive(true);
                witchInUse = false;
                currentWitch = Guid.Empty;
                EGHelperWindow.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Would you like to end your turn by drawing two travel cards or by taking your " + goldAccrued + " accumulated gold?";
                EGHelperWindow.transform.GetChild(1).gameObject.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Take " + goldAccrued + " Gold";
                caravanMode = false;
                Elfenroads.Control.LockCamera?.Invoke(null, EventArgs.Empty);
                Elfenroads.Control.LockDraggables?.Invoke(null, EventArgs.Empty);
                endTurnButton.SetActive(false);
                return;
            }

            //This is callled if "endTurn" was pressed. If the player has less than or equal to 4 travelcards, simply call endTurn on ElfenroadsControl with an empty list. May need Elfenroad change here?***
            int numCards = Elfenroads.Control.getThisPlayer().inventory.cards.Count;
            if(numCards <= 4 || ((Elfenroads.Model.game.variant.HasFlag(Game.Variant.LongerGame) && Elfenroads.Model.game.roundNumber == 4)) 
                || ((!Elfenroads.Model.game.variant.HasFlag(Game.Variant.LongerGame) && Elfenroads.Model.game.roundNumber == 3))){
                List<Guid> emptyList = new List<Guid>();
                Elfenroads.Control.endTurn(emptyList);
                goldAccrued = 0;
                return;
            }else{
                //If not, we'll have to enable the window allowing players to discard cards. This means getting Guids of Player cards and putting the UI elements into the GridLayoutGroup.
                loadPlayerCards();
                //Now, we can enable the window.
                endTurnButton.SetActive(false);
                helperWindow.SetActive(true);
                witchInUse = false;
                currentWitch = Guid.Empty;
                caravanMode = false;
                Elfenroads.Control.LockCamera?.Invoke(null, EventArgs.Empty);
                Elfenroads.Control.LockDraggables?.Invoke(null, EventArgs.Empty);
                topText.text = "Choose cards to discard until only four are left:";
                bottomText.text = "Discarded Cards: ";
            }
        }

    private void updateHelperWindowCardCount(){
        topText.text = "Choose cards to discard until fifteen or less are left (currently contains " + topCards.Count + " cards):";
    }

    private void loadPlayerCards(){
        bottomCards = new List<GameObject>();
        topCards = new List<GameObject>();
        bool witchHasBeenSkipped = false;
        foreach(Card c in Elfenroads.Control.getThisPlayer().inventory.cards){
            switch(c){
                case TravelCard tc:
                {
                    switch(tc.transportType){
                        case TransportType.Dragon:
                        {  
                            createAndAddToLayout(dragonCardPrefab, c);
                            break;
                        }
                        case TransportType.ElfCycle:
                        {
                            createAndAddToLayout(cycleCardPrefab, c);
                            break;
                        }
                        case TransportType.MagicCloud:
                        {
                            createAndAddToLayout(cloudCardPrefab, c);
                            break;
                        }
                        case TransportType.TrollWagon:
                        {
                            createAndAddToLayout(trollCardPrefab, c);
                            break;
                        }
                        case TransportType.GiantPig:
                        {
                            createAndAddToLayout(pigCardPrefab, c);
                            break;
                        }
                        case TransportType.Unicorn:
                        {
                            createAndAddToLayout(unicornCardPrefab, c);
                            break;
                        }
                        case TransportType.Raft:
                        {
                            createAndAddToLayout(raftCardPrefab, c);
                            break;
                        }
                        default: Debug.Log("Error: Card type invalid.") ; break;
                    }
                    break;
                }
                case WitchCard wc:{
                    //If the cards are being loaded and it isn't caravanMode, we want to show the witch.
                    if(!caravanMode){
                        createAndAddToLayout(witchCardPrefab,c);
                        break;
                    }else{
                        //If it is caravan mode and a witch is in use, we want to show only the 'extra' witches (since one is in use to bypass the obstacle.)
                        if(witchInUse){
                            if(witchHasBeenSkipped){
                                createAndAddToLayout(witchCardPrefab,c);
                                break;
                            }else{
                                witchHasBeenSkipped = true;
                            }
                        }else{
                            createAndAddToLayout(witchCardPrefab,c);
                            break;
                        }
                    }
                    break;
                }
                default: Debug.Log("Card is of undefined type!") ; break; 
            }
        }
    }

        private void createAndAddToLayout(GameObject prefab, Card c){
            GameObject instantiatedCard = Instantiate(prefab, this.transform);
            instantiatedCard.GetComponent<GuidViewHelper>().setGuid(c.id);
            instantiatedCard.GetComponent<GuidViewHelper>().setContainer(this);
            instantiatedCard.transform.SetParent(topLayoutGroup, false);
            LayoutRebuilder.ForceRebuildLayoutImmediate(topLayoutGroup);
            topCards.Add(instantiatedCard);
            return;
        }

        public void confirmClicked(){
            if(caravanMode){
                confirmCaravan();
            }else{
                confirmDiscardCards();
            }
        }

        private void confirmCaravan(){
            if(bottomCards.Count != targetRoadCost){
                invalidMessage("Your caravan should include exactly " + targetRoadCost + " cards!");
                return;
            }else{
                List<Guid> discardList = new List<Guid>();
                foreach(GameObject card in bottomCards){ 
                    discardList.Add(card.GetComponent<GuidViewHelper>().getGuid());
                }
                if(witchInUse){ //If we were using a caravan with a witch, add it here.
                    discardList.Add(currentWitch);
                }
                clearDiscard();
                endTurnButton.SetActive(true);
                helperWindow.SetActive(false);
                caravanMode = false;
                Elfenroads.Control.moveBoot(targetTown.id, discardList);
                if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.Elfengold)){
                    bool hasGoldCounter = false;
                    foreach(Counter c in targetRoad.counters){
                        if(c is GoldCounter){
                            hasGoldCounter = true;
                            break;
                        }
                    }
                    if(hasGoldCounter){
                        goldAccrued += (2 * targetTown.goldValue);
                    }else{
                        goldAccrued += targetTown.goldValue;
                    }
                }
                witchInUse = false;
                currentWitch = Guid.Empty;
                targetRoad = null;
                return;
            }
        }

        //Called by the "confirm" button. If there is the proper amount of cards in the "bottomCards" array, their guids are passed to Control to emit to the server, and both arrays/GridLayoutGroups are cleared.
        private void confirmDiscardCards(){
            if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.Elfengold)){
                if(topCards.Count > 15){
                invalidMessage("You must keep 15 or less cards!");
                return;
            }else{
                List<Guid> discardList = new List<Guid>();
                foreach(GameObject card in bottomCards){ 
                    discardList.Add(card.GetComponent<GuidViewHelper>().getGuid());
                }
                clearDiscard();
                endTurnButton.SetActive(true);
                helperWindow.SetActive(false);
                caravanMode = false;
                Elfenroads.Control.endAndTakeGold(goldAccrued, discardList);
                goldAccrued = 0;
                return;
            }

            }else{
                if(topCards.Count != 4){
                    invalidMessage("You must keep exactly 4 cards!");
                    return;
                }else{
                    List<Guid> discardList = new List<Guid>();
                    foreach(GameObject card in bottomCards){ 
                        discardList.Add(card.GetComponent<GuidViewHelper>().getGuid());
                    }
                    clearDiscard();
                    endTurnButton.SetActive(true);
                    helperWindow.SetActive(false);
                    caravanMode = false;
                    Elfenroads.Control.endTurn(discardList);
                    goldAccrued = 0;
                    return;
                }
            }
        }

        //Called by the "cancel" button. Simply closes the discard card window, clearing the arrays and GridLayoutGroups.
        public void cancelWindow(){
            clearDiscard();
            endTurnButton.SetActive(true);
            helperWindow.SetActive(false);
            caravanMode = false;
            targetRoad = null;
            goldReminder.SetActive(false);
            Elfenroads.Control.UnlockCamera?.Invoke(null, EventArgs.Empty);
            Elfenroads.Control.UnlockDraggables?.Invoke(null, EventArgs.Empty);
        }


        private void clearDiscard(){
            topCards.Clear();
            bottomCards.Clear();

            foreach(Transform child in topLayoutGroup){
                child.SetParent(null);
                DestroyImmediate(child.gameObject);
            }
            topLayoutGroup.DetachChildren();

            foreach(Transform child in bottomLayoutGroup){
                child.SetParent(null);
                DestroyImmediate(child.gameObject);
            }
            bottomLayoutGroup.DetachChildren();
            
            return;
        }

        public void cancelElfenGold(){
            endTurnButton.SetActive(true);
            EGHelperWindow.SetActive(false);
            Elfenroads.Control.UnlockCamera?.Invoke(null, EventArgs.Empty);
            Elfenroads.Control.UnlockDraggables?.Invoke(null, EventArgs.Empty);
        }

        public void EGendAndTakeCards(){
            Elfenroads.Control.endAndDrawCards();
            cancelElfenGold();
            goldAccrued = 0;
        }

        public void EGendAndTakeGold(){
            Elfenroads.Control.endAndTakeGold(goldAccrued, new List<Guid>());
            cancelElfenGold();
            goldAccrued = 0;
        }

        public void toggleWitch(){
            if(! Elfenroads.Control.isCurrentPlayer()){
                //Inform player they are not the current player.
                invalidMessage("Not your turn!");
                return;
            }
            if(playerInfoController.windowOpen || infoWindowController.isOpen || helperWindow.activeSelf || EGHelperWindow.activeSelf){
                invalidMessage("Close any open windows first!");
                return;
            }

            if(witchInUse){
                witchInUse = false;
                return;
            }

            bool ownsIt = false;
            foreach(Card c in Elfenroads.Control.getThisPlayer().inventory.cards){
                if(c is WitchCard){
                    ownsIt = true;
                    currentWitch = c.id;
                }
            }
            if(! ownsIt){
                invalidMessage("No witch owned!");
                return;
            }else{
                if(Elfenroads.Control.getThisPlayer().inventory.gold < 1){
                    invalidMessage("No gold!");
                    return;
                }
                witchInUse = true;
            }
        }   

        public void attemptMagicFlight(Town targetTown){
            if(!witchInUse){
                invalidMessage("You must use a witch for a magic flight!");
                return;
            }
            if(Elfenroads.Control.getThisPlayer().inventory.gold < 3){
                invalidMessage("You need 3 gold for a magic flight!");
                return;
            }
            foreach(Boot b in targetTown.boots){
                if(b.color == Elfenroads.Control.getThisPlayer().boot.color){
                    invalidMessage("You are already in this town!");
                    return;
                }
            }
            //If we made it here, the magic flight is allowed.
            Elfenroads.Control.magicFlight(currentWitch, targetTown.id);
            witchInUse = false;
            currentWitch = Guid.Empty;

        }

        public void validateMoveBoot(string cardType, Road road){
            Debug.Log("Checking for boot of color: " + Elfenroads.Control.getThisPlayer().boot.color);
            foreach(Town t in Elfenroads.Model.game.board.towns){
                foreach(Boot b in t.boots){
                    if(b.color == Elfenroads.Control.getThisPlayer().boot.color){
                        Debug.Log("Player's boot is on town " + t.name + " which has ID " + t.id);
                    }
                }
            }
            if(Elfenroads.Model.game == null){
            invalidMessage("Testing! No game exists!");
            return;
            }
            if(! (Elfenroads.Model.game.currentPhase is MoveBoot)){
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


            //Here, we'll figure out which Card was passed in based on the cardType parameter.
            TransportType? passedCard = null;
            switch(cardType){
                case "Cloud":{
                    passedCard = TransportType.MagicCloud;
                    break;
                }
                case "Cycle":{
                    passedCard = TransportType.ElfCycle;
                    break;
                }
                case "Dragon":{
                    passedCard = TransportType.Dragon;
                    break;
                }
                case "Raft":{
                    passedCard = TransportType.Raft;
                    break;
                }
                case "Pig":{
                    passedCard = TransportType.GiantPig;
                    break;
                }
                case "Troll":{
                    passedCard = TransportType.TrollWagon;
                    break;
                }
                case "Unicorn":{
                    passedCard = TransportType.Unicorn;
                    break;
                }
                case "Witch":{
                    Debug.Log("Elfengold, not implemented!");
                    break;
                }
                default: Debug.Log("Illegal name on DragScript!"); break;
            }

            if(passedCard != null){ //Case where we found the cardType.
                //First, we need to check that the Player is on a town connected to the passed-in Road. ***NOTE: If not working, could be an issue with getThisPlayer() and references.
                Debug.Log("Start contains this boot?: " + road.start.boots.Contains(Elfenroads.Control.getThisPlayer().boot));
                Debug.Log("End contains this boot?: " + road.end.boots.Contains(Elfenroads.Control.getThisPlayer().boot));
                Debug.Log("Road has start = " + road.start.name + " with id " + road.start.id + " and end = " + road.end.name + " with id " + road.end.id + " and road is of type " + road.roadType);
                foreach(Boot b in road.start.boots){
                    Debug.Log(road.start.name + " has " + b.color + " and id " + b.id);
                }
                foreach(Boot b in road.end.boots){
                    Debug.Log(road.start.name + " has " + b.color + " and id " + b.id);
                }

                if( ! (road.start.boots.Contains(Elfenroads.Control.getThisPlayer().boot) || road.end.boots.Contains(Elfenroads.Control.getThisPlayer().boot)) ){
                    invalidMessage("Boot not adjacent to this road!");
                    return;
                }
                if(road.roadType != TerrainType.Stream && road.roadType != TerrainType.Lake && road.counters.Count <= 0){
                    invalidMessage("Road does not have a counter!");
                    return;
                }

                //Then, need to check that the cardType matches the counterType (if it isn't a lake or a stream);
                bool compatible = false;
                if(road.roadType != TerrainType.Stream || road.roadType != TerrainType.Lake){
                    List<TransportationCounter> tcs = new List<TransportationCounter>();
                    foreach(Counter c in road.counters){
                        if(c is TransportationCounter){
                            tcs.Add((TransportationCounter) c);
                        }
                    }
                    foreach(TransportationCounter tc in tcs){
                        if(tc.transportType == passedCard){
                            compatible = true;
                        }
                    }
                }

                //Next, we need to check if the cardType is compatible with the road, and what the card cost for that road is.
                int cost = roadCost(passedCard, road, compatible);
                if(cost == -1){
                    invalidMessage("Incompatible card!");
                    return;
                }

                //Now, we verify that the player has as many or more cards of that type as the cost. We save the cards' guids in an array to use later if we have enough.
                List<Guid> cardsToPass = new List<Guid>();
                foreach(Card c in Elfenroads.Control.getThisPlayer().inventory.cards){
                    if(c is TravelCard){
                        TravelCard cur = (TravelCard) c;
                        if(cur.transportType == passedCard){
                            if(cardsToPass.Count < cost){
                                cardsToPass.Add(cur.id);
                            }
                        }
                    }
                }
                if( ! (cardsToPass.Count == cost)){
                    invalidMessage("Not enough cards!");
                    return;
                }
                if(witchInUse){
                    cardsToPass.Add(currentWitch);
                }

                //All good! We can send the command to the Server.
                if(road.start.boots.Contains(Elfenroads.Control.getThisPlayer().boot)){
                    Guid town = road.end.id;
                    Debug.Log("Moving to town " + road.end.name + " which has ID: " + road.end.id);
                    if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.Elfengold)){
                        bool hasGoldCounter = false;
                        foreach(Counter c in road.counters){
                            if(c is GoldCounter){
                                hasGoldCounter = true;
                                break;
                            }
                        }
                        if(hasGoldCounter){
                            goldAccrued += (2 * road.end.goldValue);
                        }else{
                            goldAccrued += road.end.goldValue;
                        }
                    }
                    Elfenroads.Control.moveBoot(town, cardsToPass);
                    witchInUse = false;
                    currentWitch = Guid.Empty;
                    
                }else{
                    Guid town = road.start.id;
                    Debug.Log("Moving to town " + road.start.name + " which has ID: " + road.start.id);
                    if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.Elfengold)){
                        bool hasGoldCounter = false;
                        foreach(Counter c in road.counters){
                            if(c is GoldCounter){
                                hasGoldCounter = true;
                                break;
                            }
                        }
                        if(hasGoldCounter){
                            goldAccrued += (2 * road.start.goldValue);
                        }else{
                            goldAccrued += road.start.goldValue;
                        }
                    }
                    Elfenroads.Control.moveBoot(town, cardsToPass);
                    witchInUse = false;
                    currentWitch = Guid.Empty;
                }
            }else{
                Debug.Log("Double-check draggable names!");
            }
        }

        //Figures out the cost of travelling on the given road according to the given cardType. May return -1, which means the cardType is not applicable with that road.
        private int roadCost(TransportType? cardType, Road road, bool compatibleWithCounter){

            //First, figure out if there is an obstacle on the road. If so, cost is set to 1.
            int cost = 0;
            if(!witchInUse){
                foreach(Counter c in road.counters){
                if(c is ObstacleCounter){
                    cost = 1;
                    }
                }
            }
            
            if((! compatibleWithCounter) && (road.roadType != TerrainType.Lake && road.roadType != TerrainType.Stream)){
                //If the counter is not compatible, the player must be trying to use a caravan.
                cost = cost + 3;
                return cost;    
            }
            //If the cardType matches the road's terrainType, give the cost according to the travel chart. Otherwise, it costs 3 cards.
            switch(road.roadType){
                case TerrainType.Plain:{
                    if( cardType is TransportType.GiantPig || cardType is TransportType.ElfCycle || cardType is TransportType.TrollWagon || cardType is TransportType.Dragon ){
                        cost++;
                        return cost;
                    }else if(cardType is TransportType.MagicCloud){
                        cost = cost + 2;
                        return cost;
                    }
                    break;
                }
                case TerrainType.Forest:{
                    if( cardType is TransportType.GiantPig || cardType is TransportType.ElfCycle || cardType is TransportType.Unicorn){
                        cost++;
                        return cost;
                    }else if(cardType is TransportType.MagicCloud || cardType is TransportType.TrollWagon || cardType is TransportType.Dragon){
                        cost = cost + 2;
                        return cost;
                    }
                    break;
                }
                case TerrainType.Mountain:{
                    if( cardType is TransportType.MagicCloud || cardType is TransportType.Unicorn ||  cardType is TransportType.Dragon){
                        cost++;
                        return cost;
                    }else if(cardType is TransportType.ElfCycle || cardType is TransportType.TrollWagon){
                        cost = cost + 2;
                        return cost;
                    }
                    break;
                }
                case TerrainType.Desert:{
                    if( cardType is TransportType.Dragon){
                        cost++;
                        return cost;
                    }else if ( cardType is TransportType.TrollWagon || cardType is TransportType.Unicorn){
                        cost = cost + 2;
                        return cost;
                    }
                    break;
                }
                case TerrainType.Lake:{
                    if( cardType is TransportType.Raft ) {
                        cost = cost + 2;
                        return cost;
                    }else{
                        return -1;
                    }
                }
                case TerrainType.Stream:{
                    if( cardType is TransportType.Raft) {
                        //A little more involved here. Need to check where the player is. If he's on the start town, only costs 1. Otherwise costs 2.
                        if(road.start.boots.Contains(Elfenroads.Control.getThisPlayer().boot)){
                            cost++;
                            return cost;
                        }else{
                            cost = cost + 2;
                            return cost;
                        }
                    }else{
                        return -1;
                    }

                }
            }
            return -1; //Should never happen.    
        }

        private void invalidMessage(string message){
            Debug.Log("Invalid message: " + message);
            GameObject invalidBox = Instantiate(invalidMovePrefab, Input.mousePosition, Quaternion.identity, MoveBootCanvas.transform);
            invalidBox.GetComponent<TMPro.TMP_Text>().text = message;
            Destroy(invalidBox, 2f);
        }
        
        public void playerTurnMessage(string message){
            GameObject messageBox = Instantiate(messagePrefab, MoveBootCanvas.transform.position, Quaternion.identity, MoveBootCanvas.transform);
            messageBox.transform.GetChild(1).gameObject.GetComponent<TMPro.TMP_Text>().text = message;
            Destroy(messageBox, 1.9f);
        }

        Color32 normalColor = new Color32(255, 255, 255, 255);
        Color32 spellColor = new Color32(166, 64, 229, 255);
        UnityEngine.Color lerpedColorWitch = new Color32(255, 255, 255, 255);

        void Update(){
            if(witchInUse){
                lerpedColorWitch = UnityEngine.Color.Lerp(normalColor, spellColor, Mathf.PingPong(Time.time, 1));
                witchButton.GetComponent<Image>().color = lerpedColorWitch;
            }else{
                witchButton.GetComponent<Image>().color = normalColor;
            }
        }

    }
}