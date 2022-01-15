using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Models;

namespace Models {
    public interface INotifyModelUpdated {
        event EventHandler ModelUpdated;
    }

    public interface IUpdatable<T> {
        void update(T updated);
    }

    public class ElfenroadsModel : Elfenroads {
        public List<GameObject> roads;
        public List<GameObject> towns;

        private Game game;

        private void Awake() {
            Elfenroads.Model = this;

            //Once the session is launched, this is where all required model objects are created and assigned to their appropriate GameObjects (though their other attributes will contain default or null values if they are subject to change, since they would need
            // to be updated via the Server once it has set up the initial state). (See ElfenroadsControl)
        
            //So, create the Game model, which in its constuctor will create the Board and assign all roads and towns.
            game = new Game(roads,towns);
            //For testing purposes right now, we'll instantiate a single player in our model. Later, we'd have to get the list of Players either from the Server or from the MenuScene.
            Debug.Log("Game created!");
        }

        //Called by the GameController once the amount of players (and their names) have been received from the Server (passwords in model not needed)   *** Grab from menuscene? But then, how to assign players correctly?
        public void createPlayers(List<string> playerNames){ 
            game.createPlayers(playerNames);
        }  

        // public Player getCurrentPlayer() {
        
        // }

    }

}