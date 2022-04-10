using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Views;

namespace Models {
    public class ElfenroadsModel : Elfenroads {
        public event EventHandler ModelReady;
        public List<GameObject> roadObjects;
        public List<GameObject> townObjects;
        public GameObject faceUpCounters;
        public GameObject mainPlayerObject;
        public PlayerInfoController playerInfoController;
        //Save these for Elfengold, once we get to that.
        //public GameObject faceUpCards;
        //public GameObject auctionCounters;
        //public GameObject goldCardPile;

        [HideInInspector]
        public Game game;

        private void Awake() {
            Elfenroads.Model = this;
        }

        //Called when the initial GameState is recieved. Should attach Models to their Views, and put any GUIDs in the Store.
        public void initialGame(Game g){
            //First, we set the game.
            game = g;
            foreach (Road road in game.board.roads) {
            Town startTown = game.board.GetTown(road.start.id);
            Town endTown = game.board.GetTown(road.end.id);
            road.setStart(startTown);
            road.endStart(endTown);
        }
            //Second, loop through TownObjects, calling "setAndSubscribeToModel" on each one with the Model-counterpart as a parameter.
            foreach(GameObject t in townObjects){
                t.GetComponent<TownView>().setAndSubscribeToModel(game.board.GetTown(t.name));
            }

            //Then, do the same with roadObjects.
            foreach(GameObject r in roadObjects){
                RoadView rView = r.GetComponent<RoadView>();
                Town startTown = game.board.GetTown(rView.startTown.name);
                Town endTown = game.board.GetTown(rView.endTown.name);
                //Debug.Log("In Model, start town is: " + startTown.name + ", end town is: " + endTown.name + " and type is " + rView.roadType);
                rView.setAndSubscribeToModel(game.board.GetRoad(startTown, endTown, rView.roadType));
            }

            //Next, add the main player object.
            foreach(Player p in game.players){
                if(p.name == SessionInfo.Instance().getClient().clientCredentials.username){
                    mainPlayerObject.GetComponent<ThisPlayerInventoryView>().setAndSubscribeToModel(p); 
                    Elfenroads.Control.setThisPlayer(p);
                    playerInfoController.setThisPlayer(mainPlayerObject);
                }
            }
            
            playerInfoController.createOpponentPrefabs(game.players);

            //Next, we need to store all GuidModels into the store. That is, Townpieces, Towns, Roads, Players, Boots, Counters and Cards (I think that's it but feel free to double-check).
            foreach(Town t in game.board.towns){
                ModelStore.Add(t);
                foreach(TownPiece tp in t.townPieces){
                    ModelStore.Add(tp);
                }
            }
            foreach(Player p in game.players){
                ModelStore.Add(p);
                ModelStore.Add(p.boot);
                foreach(Counter c in p.inventory.counters){
                    ModelStore.Add(c);
                }
                foreach(Card c in p.inventory.cards){
                    ModelStore.Add(c);
                }
                foreach(TownPiece tp in p.inventory.townPieces){
                    ModelStore.Add(tp);
                }
            }

            foreach(Road r in game.board.roads){
                ModelStore.Add(r);
                foreach(Counter c in r.counters){
                    ModelStore.Add(c);
                }
            }
            foreach(Counter c in game.counterPile.counters){
                ModelStore.Add(c);
            }
            foreach(Card c in game.cards.cards){
                ModelStore.Add(c);
            }
            foreach(Card c in game.discardPile.cards){
                ModelStore.Add(c);
            }

            foreach(Card c in game.faceUpCards){
                ModelStore.Add(c);
            }

            foreach(Card c in game.goldCardDeck){
                ModelStore.Add(c);
            }

            foreach(Counter c in game.faceUpCounters){
                ModelStore.Add(c);
            }

            //Add items to store depending on which phase we're in:
            switch(game.currentPhase){
                case Auction a:{
                    foreach(Counter c in a.countersForAuction){
                        ModelStore.Add(c);
                    }
                    break;
                }
                case SelectCounter sc:{
                    foreach(Counter c in sc.counters){
                        ModelStore.Add(c);
                    }
                    break;
                }
            }

            //Now that the Model is fully integrated, we can tell the main Game controller to prepare the screen accordingly, and begin listening for GameState updates.
            Elfenroads.Control.prepareScreen();
            Elfenroads.Control.beginListening();
            Elfenroads.Control.hasBeenSetup = true;
        }

        //Called when a new, non-initial GameState is received.
        public void updatedGame(Game g){
                game.Update(g);
                Elfenroads.Control.prepareScreen();
        }

        //Don't think we need this anymore? Alternative way if we want to I guess.
        private void ready(){
            ModelReady?.Invoke(game, EventArgs.Empty); // Calls all of the Game's views
        }

        public Player getCurrentPlayer() {

            switch(game.currentPhase){
                case DrawCounters dc:
                    return dc.currentPlayer;
                case PlanTravelRoutes pt:
                    return pt.currentPlayer;
                case MoveBoot mb:
                    return  mb.currentPlayer;
                case FinishRound fr: //Operating under the assumption this is called ONCE PER ROUND, due to how it works.
                    return null;
                case GameOver go:
                    return null;
                case DrawCards dCa:
                    return dCa.currentPlayer;
                case SelectCounter sc:
                    return sc.currentPlayer;
                case Auction a:
                    return a.currentPlayer;
                default:
                    Debug.Log("Phase not implemented!");
                    break;
            }
            return null;
        }

    }

}