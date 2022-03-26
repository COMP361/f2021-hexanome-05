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
        public GameObject FinishRoundCanvas;
        public GameObject EndOfGameCanvas;
        public GameObject DrawCardsCanvas;
        public DrawCountersController drawCountersController;
        public DrawCardsController drawCardsController;
        public PlanTravelController planTravelController;
        public MoveBootController moveBootController;
        public FinishRoundController finishRoundController;
        public PlayerInfoController playerInfoController;
        public InfoWindowController infoWindowController;
        public GameOverController gameOverController;

        public GameObject PlayerCounters;
        public GameObject PlayerCards;

        //Making these singletons might be better?
        public GameObject MoveBootsManager;
        public GameObject ChooseBootController;

        [HideInInspector]
        public bool cameraLocked = true;
        public bool draggablesLocked = true;
        public EventHandler LockCamera;
        public EventHandler UnlockCamera;
        public EventHandler LockDraggables;
        public EventHandler UnlockDraggables;
        private Player thisPlayer;
        public Player currentPlayer;

        private void cameraLock(object sender, EventArgs e){
            cameraLocked = true;
        }

        private void cameraUnlock(object sender, EventArgs e){
            cameraLocked = false;
        }

        private void draggableLock(object sender, EventArgs e){
            draggablesLocked = true;
        }

        private void draggableUnlock(object sender, EventArgs e){
            draggablesLocked = false;
        }

        private void Awake() {
            Elfenroads.Control = this;
            LockCamera += cameraLock;
            UnlockCamera += cameraUnlock;
            LockDraggables += draggableLock;
            UnlockDraggables += draggableUnlock;
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
            // socket.Instance.Emit("InitializeGame", sessionInfo.getSessionID(), true); // Only the host should be doing this! 
            JObject json = new JObject();
            json.Add("game_id", sessionInfo.getClient().thisSessionID);
            Debug.Log(sessionInfo.getVariant());
            json.Add("variant", JsonConvert.SerializeObject(sessionInfo.getVariant()));
            Debug.Log(json.ToString());
            socket.Instance.Emit("ChooseVariant", json.ToString(), false); // Variant choices
        }

        //Once that's done, all Players will need to choose their boots. So, call the "ChooseBootController"'s start choosing function.
        ChooseBootController.GetComponent<ChooseBootController>().beginChooseColors(sessionInfo, socket);
        Elfenroads.Control.LockDraggables?.Invoke(null, EventArgs.Empty); //**May need verification.
        //Once the Server recieves all colors, it can send the initial game state to the Clients and the game begins. 
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

        //Called after an update has been integrated to the Model. Reads the current phase, and presents the appropriate canvas to the Player. (Depending on the phase, should lock/unlock the camera as well)
        public void prepareScreen(){
            disableCanvases();
            playerInfoController.closeWindow();
            infoWindowController.CloseHelpWindow();
            playerInfoController.updateViews();

            switch(Elfenroads.Model.game.currentPhase){
                case DrawCounters dc:{
                    DrawCounterCanvas.SetActive(true);
                    PlayerCounters.SetActive(true);
                    PlayerCards.SetActive(false);
                    LockCamera?.Invoke(null, EventArgs.Empty);
                    currentPlayer = dc.currentPlayer;
                    if(!DrawCounterCanvas.transform.GetChild(0).gameObject.activeSelf){
                        DrawCounterCanvas.transform.GetChild(0).gameObject.SetActive(true);
                    }
                    drawCountersController.updateFaceUpCounters(dc);
                    TMPro.TMP_Text prompt = GameObject.Find("DrawCountersPrompt").GetComponent<TMPro.TMP_Text>(); 
                    if(currentPlayer.id == thisPlayer.id){
                        prompt.text = "Your turn! Draw a counter!";
                    }else{
                        prompt.text = currentPlayer.name + " is drawing a counter...";
                    }
                    infoWindowController.UpdateDrawCounterHelp();
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
                    if(isCurrentPlayer()){
                        planTravelController.playerTurnMessage("It is your turn!");
                    }else{
                        planTravelController.playerTurnMessage("It is " + currentPlayer.name + "'s  turn!" );
                    }
                    infoWindowController.UpdatePlanTravelRoutesHelp();
                    if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.Elfengold)){
                        PlanTravelCanvas.transform.GetChild(0).gameObject.SetActive(true);
                        PlanTravelCanvas.transform.GetChild(1).gameObject.SetActive(true);
                    }
                    break;
                }
                case MoveBoot mb:{
                    Debug.Log("Phase is MoveBoot!");
                    MoveBootCanvas.SetActive(true);
                    PlayerCounters.SetActive(false);
                    PlayerCards.SetActive(true);
                    UnlockCamera?.Invoke(null, EventArgs.Empty);
                    UnlockDraggables?.Invoke(null, EventArgs.Empty);
                    
                    if(currentPlayer != mb.currentPlayer){
                        currentPlayer = mb.currentPlayer;
                        if(isCurrentPlayer()){
                            moveBootController.playerTurnMessage("It is your turn!");
                        }else{
                            moveBootController.playerTurnMessage("It is " + currentPlayer.name + "'s  turn!" );
                        }
                    }
                    infoWindowController.UpdateMoveBootHelp();
                    if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.Elfengold)){
                        MoveBootCanvas.transform.GetChild(0).gameObject.SetActive(true);
                    }
                    if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.ElfenWitch)){
                        MoveBootCanvas.transform.GetChild(1).gameObject.SetActive(true);
                    }
                    moveBootController.updateEGStuff();
                    break;
                }
                case FinishRound fr:{ //Operating under the assumption this is called ONCE PER ROUND, due to how it works.
                    Debug.Log("Phase is FinishRound!");
                    FinishRoundCanvas.SetActive(true);
                    PlayerCounters.SetActive(true);
                    PlayerCards.SetActive(false);
                    LockCamera?.Invoke(null, EventArgs.Empty);
                    LockDraggables?.Invoke(null, EventArgs.Empty);
                    finishRoundController.initialSetup(thisPlayer);
                    //currentPlayer = null; //*** Would this break things?
                    infoWindowController.UpdateFinishRoundHelp();
                    break;
                }
                case GameOver go:{
                    EndOfGameCanvas.SetActive(true);
                    gameOverController.updateTexts();
                    break;
                }
                case DrawCards dCa:{
                    DrawCardsCanvas.SetActive(true);
                    LockCamera?.Invoke(null, EventArgs.Empty);
                    LockDraggables?.Invoke(null, EventArgs.Empty);
                    currentPlayer = dCa.currentPlayer;
                    if(!DrawCounterCanvas.transform.GetChild(0).gameObject.activeSelf){
                        DrawCounterCanvas.transform.GetChild(0).gameObject.SetActive(true);
                    }
                    drawCardsController.updateFaceUpCards();
                    break;
                }
                /*
                case SelectCounters sc{
                    
                    break;
                }
                case Auction a{
                    
                    break;
                }
                */
                default:{
                    Debug.Log("Phase not implemented!");
                    break;
                }
            }
        }

        private void disableCanvases(){
            DrawCounterCanvas.SetActive(false);
            PlanTravelCanvas.SetActive(false);
            MoveBootCanvas.SetActive(false);
            FinishRoundCanvas.SetActive(false);
            EndOfGameCanvas.SetActive(false);
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

        public void drawCard(GameObject clickedCard){
            Debug.Log("Clicked card emit!");
            JObject json = new JObject();
            json.Add("game_id", sessionInfo.getSessionID());
            json.Add("player_id", Elfenroads.Model.game.GetPlayer(sessionInfo.getClient().clientCredentials.username).id);
            json.Add("card_id", clickedCard.GetComponent<GuidViewHelper>().getGuid());
            socket.Instance.Emit("PickCard", json.ToString(), false);
        }

        public void takeGoldCards(){
            JObject json = new JObject();
            json.Add("game_id", sessionInfo.getSessionID());
            json.Add("player_id", Elfenroads.Model.game.GetPlayer(sessionInfo.getClient().clientCredentials.username).id);
            socket.Instance.Emit("TakeGoldCards", json.ToString(), false);
        }

        public void drawRandomCard(){
            JObject json = new JObject();
            Debug.Log("Game ID: " + sessionInfo.getSessionID());
            Debug.Log("Player ID: " + Elfenroads.Model.game.GetPlayer(sessionInfo.getClient().clientCredentials.username).id);
            json.Add("game_id", sessionInfo.getSessionID());
            json.Add("player_id", Elfenroads.Model.game.GetPlayer(sessionInfo.getClient().clientCredentials.username).id);
            socket.Instance.Emit("DrawRandomCard", json.ToString(), false);
        }

        public void SelectCounter(Guid counterToKeepSecret){
            JObject json = new JObject();
            json.Add("game_id", sessionInfo.getSessionID());
            json.Add("player_id", Elfenroads.Model.game.GetPlayer(sessionInfo.getClient().clientCredentials.username).id);
            json.Add("counter_id", counterToKeepSecret);
            socket.Instance.Emit("SelectCounter", json.ToString(), false);
        }

        public void placeBid(int amountToBid){
            JObject json = new JObject();
            json.Add("game_id", sessionInfo.getSessionID());
            json.Add("player_id", Elfenroads.Model.game.GetPlayer(sessionInfo.getClient().clientCredentials.username).id);
            json.Add("bid_amount", amountToBid);
            socket.Instance.Emit("PlaceBid", json.ToString(), false);
        }

        public void passAuction(){
            JObject json = new JObject();
            json.Add("game_id", sessionInfo.getSessionID());
            json.Add("player_id", Elfenroads.Model.game.GetPlayer(sessionInfo.getClient().clientCredentials.username).id);
            socket.Instance.Emit("PassAuction", json.ToString(), false);
        }

        public void placeCounter(Guid counterGuid, Guid roadGuid){
            JObject json = new JObject();
            json.Add("game_id", sessionInfo.getSessionID());
            json.Add("player_id", Elfenroads.Model.game.GetPlayer(sessionInfo.getClient().clientCredentials.username).id);
            json.Add("road_id", roadGuid);
            json.Add("counter_id", counterGuid);
            socket.Instance.Emit("PlaceCounter", json.ToString(), false);
        }

        public void playDoubleCounter(Guid spellGuid, Guid counterGuid, Guid roadGuid){
            JObject json = new JObject();
            json.Add("game_id", sessionInfo.getSessionID());
            json.Add("player_id", Elfenroads.Model.game.GetPlayer(sessionInfo.getClient().clientCredentials.username).id);
            json.Add("road_id", roadGuid);
            json.Add("counter_id", counterGuid);
            json.Add("spell_id", spellGuid);
            socket.Instance.Emit("PlayDouble", json.ToString(), false); 
        }

        public void playExchangeCounter(Guid road1, Guid counter1, Guid road2, Guid counter2){
            JObject json = new JObject();
            json.Add("game_id", sessionInfo.getSessionID());
            json.Add("player_id", Elfenroads.Model.game.GetPlayer(sessionInfo.getClient().clientCredentials.username).id);
            json.Add("roadOne_id", road1);
            json.Add("counterOne_id", counter1);
            json.Add("roadTwo_id", road2);
            json.Add("counterTwo_id", counter2);
            socket.Instance.Emit("PlayExchange", json.ToString(), false); 
        }

        public void passTurn(){
            Debug.Log("About to emit PassTurn!");
            JObject json = new JObject();
            json.Add("game_id", sessionInfo.getSessionID());
            json.Add("player_id", Elfenroads.Model.game.GetPlayer(sessionInfo.getClient().clientCredentials.username).id);
            socket.Instance.Emit("PassTurn", json.ToString(), false);
        }

        // May need to pass in a road if the Town Guid can't be ascertained.
        public void moveBoot(Guid townGuid, List<Guid> cardsToUse){
            String[] stringArray = new String[cardsToUse.Count];
            for(int i = 0 ; i < cardsToUse.Count ; i++){
                stringArray[i] = cardsToUse[i].ToString();
            }

            JObject json = new JObject();
            json.Add("game_id", sessionInfo.getSessionID());
            json.Add("player_id", Elfenroads.Model.game.GetPlayer(sessionInfo.getClient().clientCredentials.username).id);
            json.Add("town_id", townGuid);
            json.Add("card_ids", JsonConvert.SerializeObject(stringArray)); // Serialization is not the same as "ToString"
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

        public void magicFlight(Guid witchGuid, Guid townGuid){
            JObject json = new JObject();
            json.Add("game_id", sessionInfo.getSessionID());
            json.Add("player_id", Elfenroads.Model.game.GetPlayer(sessionInfo.getClient().clientCredentials.username).id);
            json.Add("town_id", townGuid);
            json.Add("witch_id", witchGuid); 
            socket.Instance.Emit("MagicFlight", json.ToString(), false);
        }

        public void endAndTakeGold(int amount){
            JObject json = new JObject();
            json.Add("game_id", sessionInfo.getSessionID());
            json.Add("player_id", Elfenroads.Model.game.GetPlayer(sessionInfo.getClient().clientCredentials.username).id);
            json.Add("gold", amount);
            socket.Instance.Emit("endAndTakeGold", json.ToString(), false);
        }

        public void endAndDrawCards(){
            JObject json = new JObject();
            json.Add("game_id", sessionInfo.getSessionID());
            json.Add("player_id", Elfenroads.Model.game.GetPlayer(sessionInfo.getClient().clientCredentials.username).id);
            socket.Instance.Emit("endAndDrawCards", json.ToString(), false);
        }

        public void finishRound(List<Guid> countersToDiscard){
            String[] stringArray = new String[countersToDiscard.Count];
            for(int i = 0 ; i < countersToDiscard.Count ; i++){
                stringArray[i] = countersToDiscard[i].ToString();
            }
            JObject json = new JObject();
            json.Add("game_id", sessionInfo.getSessionID());
            json.Add("player_id", Elfenroads.Model.game.GetPlayer(sessionInfo.getClient().clientCredentials.username).id);
            json.Add("counter_ids", JsonConvert.SerializeObject(stringArray));
            Debug.Log("Emitting counter with id " + countersToDiscard + " to server!");
            socket.Instance.Emit("CounterDiscarded", json.ToString(), false); 
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