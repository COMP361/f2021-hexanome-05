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
        public GameObject discardWindow;
        public GameObject endTurnButton;
        public RectTransform potentialDiscardLayoutGroup;
        public RectTransform toDiscardLayoutGroup;

        public GameObject dragonCardPrefab;
        public GameObject trollCardPrefab;
        public GameObject cloudCardPrefab;
        public GameObject cycleCardPrefab;
        public GameObject unicornCardPrefab;
        public GameObject pigCardPrefab;
        public GameObject raftCardPrefab;

        public List<GameObject> roadObjects;
        private List<RoadView> roadViews;
        public bool locked = true;

        private List<GameObject> playerCards;
        private List<GameObject> cardsToDiscard;
        

        void Start() {
            roadViews = new List<RoadView>();
            foreach (GameObject road in roadObjects) {
                roadViews.Add(road.GetComponent<RoadView>());
            }
            subscribeToRoadClickEvents();
            discardWindow.SetActive(false);
        }

        private void subscribeToRoadClickEvents() {
            foreach (RoadView roadView in roadViews) {
                roadView.RoadClicked += onRoadClicked;
            }
        }

        //Going to keep this for now - likely will be used for the "exchange" counter.
        private void onRoadClicked(object sender, EventArgs args) {
            if(!locked){
                //If we  made it here, then a road was clicked.
                Debug.Log("road clicked!");
            }
        }

        public void GUIClicked(GameObject cardClicked){
            Debug.Log("In the cardClicked function.");
            foreach(GameObject card in playerCards){
                if(card.GetComponent<GuidViewHelper>().getGuid() == cardClicked.GetComponent<GuidViewHelper>().getGuid()){
                    //Transfer this card from playerCards to ToDiscard
                    Debug.Log("Transferring from playerCards to ToDiscard!");
                    transferToDiscard(card);
                    return;
                }
            }
            foreach(GameObject card in cardsToDiscard){
                if(card.GetComponent<GuidViewHelper>().getGuid() == cardClicked.GetComponent<GuidViewHelper>().getGuid()){
                    //Transfer this card from playerCards to ToDiscard
                    Debug.Log("Transferring from playerCards to ToDiscard");
                    transferToCards(card);
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
            card.transform.SetParent(toDiscardLayoutGroup, false);
            LayoutRebuilder.ForceRebuildLayoutImmediate(toDiscardLayoutGroup);
            LayoutRebuilder.ForceRebuildLayoutImmediate(potentialDiscardLayoutGroup);
            playerCards.Remove(card);
            cardsToDiscard.Add(card);
            Debug.Log("Player cards: " + playerCards.Count);
            Debug.Log("Cards to discard: " + cardsToDiscard.Count);
            return;
        }

        private void transferToCards(GameObject card){
            Debug.Log("In transferToCards!");
            card.transform.SetParent(potentialDiscardLayoutGroup, false);
            LayoutRebuilder.ForceRebuildLayoutImmediate(toDiscardLayoutGroup);
            LayoutRebuilder.ForceRebuildLayoutImmediate(potentialDiscardLayoutGroup);
            cardsToDiscard.Remove(card);
            playerCards.Add(card);
            Debug.Log("Player cards: " + playerCards.Count);
            Debug.Log("Cards to discard: " + cardsToDiscard.Count);
            return;
        }

        public void endTurnValidation(){
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
            //This is callled if "endTurn" was pressed. If the player has less than or equal to 4 travelcards, simply call endTurn on ElfenroadsControl with an empty list.
            int numCards = Elfenroads.Control.getThisPlayer().inventory.cards.Count;
            if(numCards <= 4){
                List<Guid> emptyList = new List<Guid>();
                Elfenroads.Control.endTurn(emptyList);
                return;
            }else{
                //If not, we'll have to enable the window allowing players to discard cards. This means getting Guids of Player cards and putting the UI elements into the GridLayoutGroup.
                cardsToDiscard = new List<GameObject>();
                playerCards = new List<GameObject>();
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
                        case WitchCard wc:
                        {
                            Debug.Log("Elfengold - Do later");
                            break;
                        }
                        case GoldCard gc:
                        {
                            Debug.Log("Elfengold - Do later");
                            break;
                        }
                        default: Debug.Log("Card is of undefined type!") ; break;
                    }
                }

                //Now, we can enable the window.
                endTurnButton.SetActive(false);
                discardWindow.SetActive(true);
                Elfenroads.Control.LockCamera?.Invoke(null, EventArgs.Empty);
                Elfenroads.Control.LockDraggables?.Invoke(null, EventArgs.Empty);
            }
        }

        private void createAndAddToLayout(GameObject prefab, Card c){
            GameObject instantiatedCard = Instantiate(prefab, this.transform);
            instantiatedCard.GetComponent<GuidViewHelper>().setGuid(c.id);
            instantiatedCard.GetComponent<GuidViewHelper>().setContainer(this);
            instantiatedCard.transform.SetParent(potentialDiscardLayoutGroup, false);
            LayoutRebuilder.ForceRebuildLayoutImmediate(potentialDiscardLayoutGroup);
            playerCards.Add(instantiatedCard);
            return;
        }

        //Called by the "confirm" button. If there is the proper amount of cards in the "cardsToDiscard" array, their guids are passed to Control to emit to the server, and both arrays/GridLayoutGroups are cleared.
        public void confirmDiscardCards(){
            if(playerCards.Count != 4){
                invalidMessage("You must keep exactly 4 cards!");
                return;
            }else{
                List<Guid> discardList = new List<Guid>();
                foreach(GameObject card in cardsToDiscard){
                discardList.Add(card.GetComponent<GuidViewHelper>().getGuid());
            }
                clearDiscard();
                endTurnButton.SetActive(true);
                discardWindow.SetActive(false);
                Elfenroads.Control.endTurn(discardList);
                return;
            }
        }

        //Called by the "cancel" button. Simply closes the discard card window, clearing the arrays and GridLayoutGroups.
        public void cancelEndTurn(){
            clearDiscard();
            endTurnButton.SetActive(true);
            discardWindow.SetActive(false);
            Elfenroads.Control.UnlockCamera?.Invoke(null, EventArgs.Empty);
            Elfenroads.Control.UnlockDraggables?.Invoke(null, EventArgs.Empty);
        }

        private void clearDiscard(){
            playerCards.Clear();
            cardsToDiscard.Clear();

            foreach(Transform child in potentialDiscardLayoutGroup){
                child.SetParent(null);
                DestroyImmediate(child.gameObject);
            }
            potentialDiscardLayoutGroup.DetachChildren();

            foreach(Transform child in toDiscardLayoutGroup){
                child.SetParent(null);
                DestroyImmediate(child.gameObject);
            }
            toDiscardLayoutGroup.DetachChildren();
            
            return;
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

                //All good! We can send the command to the Server.
                if(road.start.boots.Contains(Elfenroads.Control.getThisPlayer().boot)){
                    Guid town = road.end.id;
                    Debug.Log("Moving to town " + road.end.name + " which has ID: " + road.end.id);
                    Elfenroads.Control.moveBoot(town, cardsToPass);
                }else{
                    Guid town = road.start.id;
                    Debug.Log("Moving to town " + road.start.name + " which has ID: " + road.start.id);
                    Elfenroads.Control.moveBoot(town, cardsToPass);
                }

            }else{
                Debug.Log("Double-check draggable names!");
            }
        }

        //Figures out the cost of travelling on the given road according to the given cardType. May return -1, which means the cardType is not applicable with that road.
        private int roadCost(TransportType? cardType, Road road, bool compatibleWithCounter){

            //First, figure out if there is an obstacle on the road. If so, cost is set to 1.
            int cost = 0;
            foreach(Counter c in road.counters){
                if(c is ObstacleCounter){
                    cost = 1;
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
                        //A little more involved here. Need to check where the player is. If he's on the start town, only costs 1. Otherwise costs 2. ***
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
    }
}