using System;
using UnityEngine;
using Firesplash.UnityAssets.SocketIO;
using Models;
using UnityEngine.UI;
using Views;


namespace Controls {

    /// <summary>
    /// Manages user interactions and server communication.
    /// </summary>
    public class ElfenroadsControl : Elfenroads {
        private SocketIOCommunicator socket;
        public Button redButton;
        public Button blueButton;
        public Button greenButton;
        public Button blackButton;
        public Button yellowButton;
        public Button purpleButton;

        private void Awake() {
            Elfenroads.Control = this;
        }

        private void Start() {
        //When the game is launched, the Client first needs to ask the Server for Player information, so it can set up those Models/GameObjects. Do that here. (MAY BE ABLE TO TRANSFER PLAYER DATA FROM MENUSCENE INSTEAD ***)

        //Then, once all Player info is obtained, we must first show a screen which allows the host player ONLY to choose a gamemode + variants. (This screen will lead to a "chooseBoot" screen for all Players after selection.)
        //For now, this should make a request to the Server for the initial game state (which at this point is the same every time, except for the # of players), and once the state is received it will update the Model via the Model Controller.
        //NOTE: This should likely not be the Unity Start() method - instead, change it later so that it is called by the GameModelManager when it is done setting up the Model, in order to avoid errors.      ***
            socket = GameObject.Find("Listener").GetComponent<SocketIOCommunicator>();
            GameObject obj = GameObject.Find("SessionInfo");
            //socket = obj.GetComponent<SessionInfo>().getClient().socket;
            Debug.Log("Socket ID in game scene: " + socket.Instance.SocketID);
            Debug.Log("Socket status in game scene  : " + socket.Instance.Status);

            socket.Instance.Emit("InitializeGame", obj.GetComponent<SessionInfo>().getSessionID(), true); // Only the host should be doing this! ***
            socket.Instance.On("ChooseColor", updateColors); 
        }

        //Called once the Model has been initialized, spawns the required Boot prefabs over their currentTown (should be elfenhold)
        public void spawnPlayerBoots(){

        }

        public void updateColors(string input){
            //Input will be a list of colors. Turn this into a list object, and turn off buttons which are of the colors that are present in the list.
            // ["Blue", "Green", "Red"]
            Debug.Log("reached updateColors"); 
            Debug.Log(input);
        }

        public void chooseRed(){
            Debug.Log("red chosen!");
            socket.Instance.Emit("ChooseColor", "Red", true);
        }

        public void chooseBlue(){  
            Debug.Log("blue chosen!");
            socket.Instance.Emit("ChooseColor", "Blue", true);
        }

        public void chooseGreen(){
            Debug.Log("green chosen!");
            socket.Instance.Emit("ChooseColor", "Green", true);
        }

        public void chooseYellow(){
            Debug.Log("yellow chosen!");
            socket.Instance.Emit("ChooseColor", "Yellow", true);
        }

        public void choosePurple(){
            Debug.Log("purple chosen!");
            socket.Instance.Emit("ChooseColor", "Purple", true);
        }

        public void chooseBlack(){
            Debug.Log("black chosen!");
            socket.Instance.Emit("ChooseColor", "Black", true);
        }
    }
}