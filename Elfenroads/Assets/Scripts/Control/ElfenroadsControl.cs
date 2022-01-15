using System;
using UnityEngine;
using SocketIOClient;
using Models;
using Views;


namespace Controls {

    /// <summary>
    /// Manages user interactions and server communication.
    /// </summary>
    public class ElfenroadsControl : Elfenroads {
        private SocketIO socket;

        private void Awake() {
            Elfenroads.Control = this;
        }

        private void Start() {
        //When the game is launched, the Client first needs to ask the Server for Player information, so it can set up those Models/GameObjects. Do that here. (MAY BE ABLE TO TRANSFER PLAYER DATA FROM MENUSCENE INSTEAD ***)

        //Then, once all Player info is obtained, we must first show a screen which allows the host player ONLY to choose a gamemode + variants. (This screen will lead to a "chooseBoot" prompt for all Players after selection.)
        //For now, this should make a request to the Server for the initial game state (which at this point is the same every time, except for the # of players), and once the state is received it will update the Model via the Model Controller.
        //NOTE: This should likely not be the Unity Start() method - instead, change it later so that it is called by the GameModelManager when it is done setting up the Model, in order to avoid errors.      ***

            // socket = GameObject.Find("SocketIO").GetComponent<SocketIOSingleton>().Instance;

            // enable game updates
            // socket.On("UpdateState", updateState);
        }

        //Called once the Model has been initialized, spawns the required Boot prefabs over their currentTown (should be elfenhold)
        public void spawnPlayerBoots(){

        }

        private void moveBootDemo(Town town) {

            // we should implement town.serialize() and use it as the parameter
            socket.EmitAsync("MoveBootDemo", "serialized town");
        }
    }
}