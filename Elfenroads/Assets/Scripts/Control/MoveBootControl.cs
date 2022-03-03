using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Views;
using Models;
using System;


namespace Controls
{
    public class MoveBootControl : MonoBehaviour
    {
        //Text box or something here, to alert player of invalid moves.
        public GameObject invalidMovePrefab;
        public GameObject MoveBootCanvas;
        public List<GameObject> roadObjects;
        private List<RoadView> roadViews;
        public bool locked = true;

        void Start() {
            roadViews = new List<RoadView>();
            foreach (GameObject road in roadObjects) {
                roadViews.Add(road.GetComponent<RoadView>());
            }
            subscribeToRoadClickEvents();
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


        private void validateMoveBoot(string cardType, Road road){
            if(Elfenroads.Model.game == null){
            invalidMessage("Testing! No game exists!");
            return;
            }
            if(! (Elfenroads.Model.game.currentPhase is MoveBoots)){
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
                //First, we need to check that the Player is on a town connected to the passed-in Road.
                Debug.Log("Reminder: if this does not work, likely an issue with 'getThisPlayer()' and references!");
                if( !(road.start.boots.Contains(Elfenroads.Control.getThisPlayer().boot) || road.end.boots.Contains(Elfenroads.Control.getThisPlayer().boot)) ){
                    invalidMessage("Not adjacent to this road!");
                }
                //Then, check that the Road actually has at least one counter on it.
                if(road.counters.Count <= 0){
                    invalidMessage("Road does not have a counter!");
                }

                //Next, we need to check if the cardType is compatible with the road, and what the card cost for that road is.
                int cost = roadCost(passedCard, road);
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
                }

                //All good! We can send the command to the Server.
                if(road.start.boots.Contains(Elfenroads.Control.getThisPlayer().boot)){
                    Guid town = road.end.id;
                    Elfenroads.Control.moveBoot(town, cardsToPass);
                }else{
                    Guid town = road.start.id;
                    Elfenroads.Control.moveBoot(town, cardsToPass);
                }

            }else{
                Debug.Log("Double-check draggable names!");
            }
        }

        //Figures out the cost of travelling on the given road according to the given cardType. May return -1, which means the cardType is not applicable with that road.
        private int roadCost(TransportType? cardType, Road road){

            //First, figure out if there is an obstacle on the road. If so, cost is set to 1.
            int cost = 0;
            foreach(Counter c in road.counters){
                if(c is ObstacleCounter){
                    cost = 1;
                }
            }

            //If the cardType matches the road's terrainType, give the cost according to the travel chart. Otherwise, it costs 3 cards.
            switch(road.roadType){
                case TerrainType.Plain:{
                    if( cardType is TransportType.GiantPig || cardType is TransportType.ElfCycle || cardType is TransportType.TrollWagon || cardType is TransportType.Dragon){
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

            //If we've made it here, the Player must be trying to use a caravan. So, increment cost by 3);
            cost = cost + 3;
            return cost;            
        }

        private void invalidMessage(string message){
            Debug.Log("Invalid message: " + message);
            GameObject invalidBox = Instantiate(invalidMovePrefab, Input.mousePosition, Quaternion.identity, MoveBootCanvas.transform);
            invalidBox.GetComponent<TMPro.TMP_Text>().text = message;
            Destroy(invalidBox, 2f);
        }
        
    }
}