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
        public List<GameObject> playerObjects; //Represents the player's inventories, which will be on the UI.
        public GameObject faceUpCounters;
        //Save these two for Elfengold, once we get to that.
        //public GameObject faceUpCards;
        //public GameObject auctionCounters;
        //public GameObject goldCardPile;

        [HideInInspector]
        private Game game;

        private void Awake() {
            Elfenroads.Model = this;
        }

        //Called when the initial GameState is recieved. Should attach Models to their Views, and put any GUIDs in the Store.
        public void initialGame(Game g){
            //First, we set the game.
            game = g;
            //Second, loop through TownObjects, calling "setAndSubscribeToModel" on each one with the Model-counterpart as a parameter.
            foreach(GameObject t in townObjects){
                t.GetComponent<TownView>().setAndSubscribeToModel(game.board.GetTown(t.name));
            }

            //Then, do the same with roadObjects.
            foreach(GameObject r in roadObjects){
                RoadView rView = r.GetComponent<RoadView>();
                Town startTown = game.board.GetTown(rView.startTown.name);
                Town endTown = game.board.GetTown(rView.endTown.name);
                rView.setAndSubscribeToModel(game.board.GetRoad(startTown, endTown, rView.roadType));
            }
            
            //Now, playerObjects.
            foreach(GameObject p in playerObjects){

            }
            //Finally, the faceUpCounters object.
            

            //Next, we need to store all GuidModels into the store. That is, Townpieces, Towns, Roads, Players, Boots, Counters and Cards (I think that's it but feel free to double-check).

            //Now that the Model is fully integrated, we can tell the main Game controller that we're ready to begin the first Phase.
            //Call here
        }

        //Called when a new, non-initial GameState is received.
        public void updatedGame(Game g){

        }

        //Don't think we need this anymore? Alternative way if we want to I guess.
        private void ready(){
            ModelReady?.Invoke(game, EventArgs.Empty); // Calls all of the Game's views
        }

        // public Player getCurrentPlayer() {
        
        // }

    }

}