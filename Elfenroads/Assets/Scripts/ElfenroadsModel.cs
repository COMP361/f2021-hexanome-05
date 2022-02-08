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
        //public GameObject goldCardPile;

        [HideInInspector]
        private Game game;

        private void Awake() {
            Elfenroads.Model = this;
        }

        public void modelFinished(Game g){
            //Called when the initial GameState is recieved and put into a Game object.

            //First, loop through TownObjects, calling "setAndSubscribeToModel" on each one with the Model-counterpart as a parameter.

            //Then, do the same with roadObjects.

            //Now, playerObjects.

            //Finally, faceUpCounters.
        }

        //Don't think we need this anymore? Alternative way if we want to I guess.
        private void ready(){
            ModelReady?.Invoke(game, EventArgs.Empty); // Calls all of the Game's views
        }

        // public Player getCurrentPlayer() {
        
        // }

    }

}