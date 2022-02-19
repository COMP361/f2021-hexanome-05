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
        public GameObject mainCamera;
        public GameObject DrawCounterCanvas;

        //Making these singletons might be better?
        public GameObject MoveBootsManager;
        public GameObject ChooseBootController;

        private void Awake() {
            Elfenroads.Control = this;
            
        }

        private void Start() {

        //First, if we are the host, we need to set up a screen where the host can choose variants, and then they send a "InitializeGame" message to the Server.
        //(While this happens, other clients simply get a "waiting for host to decide the gamemode" message)
        if(GameObject.Find("Listener") == null) return;
        socket = GameObject.Find("Listener").GetComponent<SocketIOCommunicator>();
        Debug.Log("Socket ID in game scene: " + socket.Instance.SocketID);
        Debug.Log("Socket status in game scene  : " + socket.Instance.Status);
        GameObject obj = GameObject.Find("SessionInfo");
        string playerName = obj.GetComponent<SessionInfo>().getClient().clientCredentials.username;
        Debug.Log("Session info player name: " + playerName + ", Host player name: " + obj.GetComponent<SessionInfo>().getClient().getSessionByID(obj.GetComponent<SessionInfo>().getSessionID()).hostPlayerName);
        if(playerName == obj.GetComponent<SessionInfo>().getClient().getSessionByID(obj.GetComponent<SessionInfo>().getSessionID()).hostPlayerName){
            Debug.Log("In the if statement");
            socket.Instance.Emit("InitializeGame", obj.GetComponent<SessionInfo>().getSessionID(), true); // Only the host should be doing this! ***
        }

        // try{ 
        //    string playerName = obj.GetComponent<SessionInfo>().getClient().clientCredentials.username;
        //    Debug.Log("Session info player name: " + playerName + ", Host player name: " + obj.GetComponent<SessionInfo>().getClient().getSessionByID(obj.GetComponent<SessionInfo>().getSessionID()).hostPlayerName);
        //    if(playerName == obj.GetComponent<SessionInfo>().getClient().getSessionByID(obj.GetComponent<SessionInfo>().getSessionID()).hostPlayerName){
        //        Debug.Log("In the if statement");
        //         socket.Instance.Emit("InitializeGame", obj.GetComponent<SessionInfo>().getSessionID(), true); // Only the host should be doing this! ***
        //    }
        // }catch (Exception e){
        //    Debug.Log("Null exception? " + e);
        // }

        //Once that's done, all Players will need to choose their boots. So, call the "ChooseBootController"'s start choosing function. *** SHOULD MAYBE BE MOVED OUTSIDE OF THIS START() FUNCTION? ***
        ChooseBootController.GetComponent<ChooseBootController>().beginChooseColors(obj.GetComponent<SessionInfo>(), socket);

        //Once the Server recieves all colors, it can send the initial game state to the Clients and the game begins. *** REMEMBER TO UN-LOCK THE CAMERA + CLICKING!
        }

        //Called after an update has been integrated to the Model. Reads the current phase, and presents the appropriate canvas to the Player. (***Depending on the phase, should lock/unlock the camera as well***)
        public void prepareScreen(){
            disableCanvases();

            switch(Elfenroads.Model.game.currentPhase){
                case DrawCounters dc:{
                    DrawCounterCanvas.SetActive(true);
                    break;
                }
                default:{
                    Debug.Log("Phase not implemented!");
                    break;
                }
            }
        }

        private void disableCanvases(){
            DrawCounterCanvas.SetActive(false);
        }



        //Called after validation from "DrawCounters" phase, sends a command to the Server for the currentPlayer to add the specified counter to their inventory.
        public void drawCounter(){ //Parameters to be decided, see "DrawCountersController"

        }

        //Called after validation from "DrawCounters" phase, sends a command to the Server for the currentPlayer to draw a random counter.
        public void drawRandomCounter(){

        }
    }
}