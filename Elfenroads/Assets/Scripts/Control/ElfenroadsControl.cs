using System;
using System.Collections.Generic;
using UnityEngine;
using Firesplash.UnityAssets.SocketIO;
using Models;
using UnityEngine.UI;
using Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;



namespace Controls {

    /// <summary>
    /// Manages user interactions and server communication.
    /// </summary>
    public class ElfenroadsControl : Elfenroads {
        private SocketIOCommunicator socket;
        private SessionInfo sessionInfo;
        public GameObject mainCamera;
        public GameObject DrawCounterCanvas;
        public GameObject PlanTravelCanvas;
        public GameObject MoveBootCanvas;

        public GameObject PlayerCounters;
        public GameObject PlayerCards;

        //Making these singletons might be better?
        public GameObject MoveBootsManager;
        public GameObject ChooseBootController;

        [HideInInspector]
        public EventHandler LockCamera;
        public EventHandler UnlockCamera;
        public EventHandler LockDraggables;
        public EventHandler UnlockDraggables;
        private Player thisPlayer;
        private Player currentPlayer;

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
        sessionInfo = obj.GetComponent<SessionInfo>();
        string playerName = sessionInfo.getClient().clientCredentials.username;
        Debug.Log("Session info player name: " + playerName + ", Host player name: " + sessionInfo.getClient().getSessionByID(obj.GetComponent<SessionInfo>().getSessionID()).hostPlayerName);
        if(playerName == sessionInfo.getClient().getSessionByID(sessionInfo.getSessionID()).hostPlayerName){
            Debug.Log("In the if statement");
            socket.Instance.Emit("InitializeGame", sessionInfo.getSessionID(), true); // Only the host should be doing this! ***
        }

        //Once that's done, all Players will need to choose their boots. So, call the "ChooseBootController"'s start choosing function. *** SHOULD MAYBE BE MOVED OUTSIDE OF THIS START() FUNCTION? ***
        ChooseBootController.GetComponent<ChooseBootController>().beginChooseColors(sessionInfo, socket);
        Elfenroads.Control.LockDraggables?.Invoke(null, EventArgs.Empty); //**May need verification.
        //Once the Server recieves all colors, it can send the initial game state to the Clients and the game begins. *** REMEMBER TO UN-LOCK THE CAMERA + CLICKING!
        }

        public void beginListening(){
            socket.Instance.On("GameState", stateRecieved); 
        }

        public void stateRecieved(string input){
            Debug.Log(input);
            var jset = new Newtonsoft.Json.JsonSerializerSettings() { TypeNameHandling = TypeNameHandling.Objects, MetadataPropertyHandling = MetadataPropertyHandling.ReadAhead, ReferenceLoopHandling = ReferenceLoopHandling.Serialize };
            Game newGame = Newtonsoft.Json.JsonConvert.DeserializeObject<Game>(input, jset);
            Elfenroads.Model.updatedGame(newGame);
        }

        //Called after an update has been integrated to the Model. Reads the current phase, and presents the appropriate canvas to the Player. (***Depending on the phase, should lock/unlock the camera as well***)
        public void prepareScreen(){
            disableCanvases();

            switch(Elfenroads.Model.game.currentPhase){
                case DrawCounters dc:{
                    DrawCounterCanvas.SetActive(true);
                    LockCamera?.Invoke(null, EventArgs.Empty);
                    currentPlayer = dc.currentPlayer;
                    TMPro.TMP_Text prompt = GameObject.Find("DrawCountersPrompt").GetComponent<TMPro.TMP_Text>(); 
                    if(currentPlayer.id == thisPlayer.id){
                        prompt.text = "Your turn! Draw a counter!";
                    }else{
                        prompt.text = currentPlayer.name + " is drawing a counter...";
                    }
                    break;
                }
                case PlanTravelRoutes pt:{
                    Debug.Log("Phase is PlanTravelRoutes!");
                    PlanTravelCanvas.SetActive(true);
                    PlayerCounters.SetActive(true);
                    PlayerCards.SetActive(false);
                    UnlockCamera?.Invoke(null, EventArgs.Empty);
                    UnlockDraggables?.Invoke(null, EventArgs.Empty);
                    currentPlayer = pt.currentPlayer;
                    break;
                }
                case MoveBoot mb:{
                    Debug.Log("Phase is MoveBoot!");
                    MoveBootCanvas.SetActive(true);
                    PlayerCounters.SetActive(false);
                    PlayerCards.SetActive(true);
                    UnlockCamera?.Invoke(null, EventArgs.Empty);
                    UnlockDraggables?.Invoke(null, EventArgs.Empty);
                    currentPlayer = mb.currentPlayer;
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
            PlanTravelCanvas.SetActive(false);
        }



        //Called after validation from "DrawCounters" phase, sends a command to the Server for the currentPlayer to add the specified counter to their inventory.
        public void drawCounter(GameObject clickedCounter){ //Parameters to be decided, see "DrawCountersController"
            //Send the asking player name and counter GUID.
            Debug.Log("About to send drawCounter to the server!");
            JObject json = new JObject();
            json.Add("game_id", sessionInfo.getSessionID());
            json.Add("player_id", Elfenroads.Model.game.GetPlayer(sessionInfo.getClient().clientCredentials.username).id);
            json.Add("counter_id", clickedCounter.GetComponent<GuidViewHelper>().getGuid());
            socket.Instance.Emit("PickCounter", json.ToString(), false);
        }

        //Called after validation from "DrawCounters" phase, sends a command to the Server for the currentPlayer to draw a random counter.
        public void drawRandomCounter(){
            JObject json = new JObject();
            json.Add("game_id", sessionInfo.getSessionID());
            json.Add("player_id", Elfenroads.Model.game.GetPlayer(sessionInfo.getClient().clientCredentials.username).id);
            socket.Instance.Emit("DrawRandomCounter", json.ToString(), false);
        }

        public void placeCounter(Guid counterGuid, Guid roadGuid){
            JObject json = new JObject();
            json.Add("game_id", sessionInfo.getSessionID());
            json.Add("player_id", Elfenroads.Model.game.GetPlayer(sessionInfo.getClient().clientCredentials.username).id);
            json.Add("road_id", roadGuid);
            json.Add("counter_id", counterGuid);
            socket.Instance.Emit("PlaceCounter", json.ToString(), false);
        }

        public void passTurn(){
            Debug.Log("About to emit PassTurn!");
            JObject json = new JObject();
            json.Add("game_id", sessionInfo.getSessionID());
            json.Add("player_id", Elfenroads.Model.game.GetPlayer(sessionInfo.getClient().clientCredentials.username).id);
            socket.Instance.Emit("PassTurn", json.ToString(), false);
        }

        // May need to pass in a road if the Town Guid can't be ascertained.
        public async void moveBoot(Guid townGuid, List<Guid> cardsToUse){
            String[] stringArray = new String[cardsToUse.Count];
            for(int i = 0 ; i < cardsToUse.Count ; i++){
                stringArray[i] = cardsToUse[i].ToString();
            }

            JObject json = new JObject();
            json.Add("game_id", sessionInfo.getSessionID());
            json.Add("player_id", Elfenroads.Model.game.GetPlayer(sessionInfo.getClient().clientCredentials.username).id);
            json.Add("town_id", townGuid);
            //JArray array = JArray.FromObject(cardsToUse);
            //Debug.Log(array.ToString());
            foreach(String s in stringArray){
                Debug.Log(s);
            }
            json.Add("card_ids", stringArray.ToString());
            socket.Instance.Emit("MoveBoot", json.ToString(), false);
        }

        public void endTurn(List<Guid> cardsToDiscard){
            JObject json = new JObject();
            json.Add("game_id", sessionInfo.getSessionID());
            json.Add("player_id", Elfenroads.Model.game.GetPlayer(sessionInfo.getClient().clientCredentials.username).id);
            JArray array = JArray.FromObject(cardsToDiscard);
            json.Add("card_ids", array);
            socket.Instance.Emit("DiscardTravelCards", json.ToString(), false);
        }

        public void setThisPlayer(Player input){
            thisPlayer = input;
        }

        public Player getThisPlayer(){
            return thisPlayer;
        }

        public bool isCurrentPlayer(){
            return thisPlayer.Equals(currentPlayer);
        }

       
    }
}