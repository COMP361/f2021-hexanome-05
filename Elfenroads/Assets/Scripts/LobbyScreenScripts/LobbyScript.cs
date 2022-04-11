using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Firesplash.UnityAssets.SocketIO;
using System.Text;

public class LobbyScript : MonoBehaviour
{
    List<SessionJSON> sessions = new List<SessionJSON>();
    string sessionsLongPollingHash = null;
    const string LS_PATH = "https://mysandbox.icu/LobbyService";

    //Known bug: When a session does not already exist (for the user), when they press "create" the game tells them they already have a session (it still creates the session - the warning message is the thing that's wrong) ***
    // IEnumerator pollingRoutine(){ //Just asks the LS for sessions every second, and displays the result.
    //     // while(true){
    //     //     #pragma warning disable 4014
    //     //     thisClient.refreshSessions();
    //     //     #pragma warning restore 4014
    //     //     yield return new WaitForSeconds(1f);
    //     // }

    //     //Load in the beginning

    //     const string LS_PATH = "https://mysandbox.icu/LobbyService";

    //     UnityWebRequest request = UnityWebRequest.Get(LS_PATH + "/api/sessions");

    //     //Return Codes: 200(success), 408(request timeout)
    //     //https://developer.mozilla.org/en-US/docs/Web/HTTP/Status
    //     int returnCode = 408;
        
    //     thisClient.refreshSessions();

    //     while (returnCode == 408){

    //         //Polling
    //         request = UnityWebRequest.Get(LS_PATH + "/api/sessions");
    //         returnCode = (int)request.responseCode;
    //         Debug.Log("Very Interesting");

    //         yield return new WaitForSeconds(1f);
    //     }

    //     if (returnCode == 200){
    //         Debug.Log("Interesting");
    //         thisClient.refreshSessions();
    //     } else {
    //         Debug.Log("not interesting" + returnCode);
    //     }

    //     request.Dispose();

    //     yield return new WaitForSeconds(1f);
    // }

    public class SessionJSON {
        public string id;
        public string creator;
        public GameParameters gameParameters;
        public bool launched;
        public string[] players;
        public Dictionary<string, string> playerLocations;
        public string savegameid;
    }

    public class GameParameters {
        public string location;
        public int maxSessionPlayers;
        public int minSessionPlayers;
        public string name;
        public string webSupport;
    }

    IEnumerator UpdateSessions() {
        return sessionsLongPollingHash == null ? getSessionsSetHash() : sessionsLongPoll();
    }

    IEnumerator getSessionsSetHash() {
        UnityWebRequest request = UnityWebRequest.Get(LS_PATH + "/api/sessions");

        yield return request.SendWebRequest();
        while (!request.isDone) {
            yield return new WaitForEndOfFrame();
        }
        
        if (request.result == UnityWebRequest.Result.Success) {
            while (!request.downloadHandler.isDone) {
                yield return new WaitForEndOfFrame();
            }

            using (MD5 md5 = MD5.Create()) {
                var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(request.downloadHandler.text));
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++) {
                    sb.Append(hash[i].ToString("x2"));
                }
                sessionsLongPollingHash = sb.ToString();
            }

            sessions = deserializeSessions(request.downloadHandler.text);
            
        }

        request.Dispose();
    }

    IEnumerator sessionsLongPoll() {
        while (true) {
            UnityWebRequest request = UnityWebRequest.Get(LS_PATH + "/api/sessions?hash=" + sessionsLongPollingHash);
            request.timeout = 300000000; //Fuck me

            request.SendWebRequest();

            while (!request.isDone) {
                yield return new WaitForEndOfFrame();
            }
            Debug.Log(request.responseCode);
            if (request.responseCode == 408) {
                request.Dispose();
                continue;
            }
            
            if (request.responseCode == 200) {
                while (!request.downloadHandler.isDone) {
                    yield return new WaitForEndOfFrame();
                }

                using (MD5 md5 = MD5.Create()) {
                    var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(request.downloadHandler.text));
                    StringBuilder sb = new StringBuilder();
                    for (int i = 0; i < hash.Length; i++) {
                        sb.Append(hash[i].ToString("x2"));
                    }
                    sessionsLongPollingHash = sb.ToString();
                }

                sessions = deserializeSessions(request.downloadHandler.text);
            }

            request.Dispose();
            break;
        }
    }

    protected List<SessionJSON> deserializeSessions(string json) {
        var sessions = new List<SessionJSON>();

        JObject result = JObject.Parse(json);

        if (!result.ContainsKey("sessions")) {
            return sessions;
        }

        foreach (JProperty jsession in ((JObject) result["sessions"]).Properties()) {
            SessionJSON session = ((JObject) jsession.Value).ToObject<SessionJSON>();
            session.id = jsession.Name;
            sessions.Add(session);
        }

        return sessions;
    }

    public TMPro.TMP_Text infoText;
    public Button createSessionButton;
    //public Button refreshButton;
    
    public GameObject tableRow;
    public GameObject tableRowPrefab;
    public GameObject launchButton;
    public GameObject joinButton;
    public GameObject deleteButton;
    public GameObject leaveButton;
    public GameObject modeButton;
    public GameObject variantButton;
    public GameObject logoutButton;
    //public SocketIO sioCom.Instance;
    public SocketIOCommunicator sioCom;
    private Client thisClient;


    IEnumerator onEnableCoroutine(){
        while (true) {
            yield return StartCoroutine(UpdateSessions());
            bool isHostOrPlayer = false;
            foreach (SessionJSON sesh in sessions) {
                // are we the host?
                if (sesh.creator == thisClient.clientCredentials.username) {
                    isHostOrPlayer = true;
                    thisClient.mySession = new Session(sesh.id, sesh.creator, sesh.players, sesh.launched, sesh.savegameid, sesh.gameParameters.maxSessionPlayers);
                    thisClient.hasSessionCreated = true;
                    thisClient.thisSessionID = sesh.id;
                    break;
                }

                // are we a player in some sesh?
                foreach (string player in sesh.players) {
                    if (player == thisClient.clientCredentials.username){
                        isHostOrPlayer = true;
                        thisClient.mySession = new Session(sesh.id, sesh.creator, sesh.players, sesh.launched, sesh.savegameid, sesh.gameParameters.maxSessionPlayers);
                        thisClient.thisSessionID = sesh.id;
                        break;
                    }
                }
            }

            if (!isHostOrPlayer) {
                thisClient.mySession = null;
                thisClient.thisSessionID = null;
            }
            displaySessions();
        }
    }

    void OnEnable() {

        //First, create Client object and get the button.
        thisClient = Client.Instance();
        createSessionButton = GameObject.Find("CreateButton").GetComponent<Button>();
        //refreshButton = GameObject.Find("RefreshButton").GetComponent<Button>();


        //Add onclick listener to button for "login" function.
        createSessionButton.onClick.AddListener(createSessionAttempt);
        //refreshButton.onClick.AddListener(refreshSessions);
        
        thisClient.RefreshSuccessEvent += refreshSuccess;
        thisClient.RefreshFailureEvent += refreshFailure;
        thisClient.CreateSuccessEvent += createSuccessResult;
        thisClient.CreateFailureEvent += createFailure;
        
        thisClient.LaunchSuccessEvent += launchSuccess;
        thisClient.LaunchFailureEvent += launchFailure;
        thisClient.DeleteSuccessEvent += deleteSuccess;
        thisClient.DeleteFailureEvent += deleteFailure;
        thisClient.JoinSuccessEvent += joinSuccess;
        thisClient.JoinFailureEvent += joinFailure;
        thisClient.LeaveSuccessEvent += leaveSuccess;
        thisClient.LeaveFailureEvent += leaveFailure;


        //Get the sioCom.Instance to start listening
        //sioCom.Instance = GameObject.Find("sioCom.InstanceIO").GetComponent<sioCom.InstanceIOSingleton>().Instance;
        //thisClient.setsioCom.Instance(sioCom.Instance);
        // Debug.Log("Socket connection status: " + sioCom.Instance.IsConnected());
        // Debug.Log("Socket status: " + sioCom.Instance.Status);
        thisClient.setSocket(sioCom);
        sioCom.Instance.Connect();
        //sioCom.Instance.On("join", (msg) => testCallback(msg.ToString()));

        StartCoroutine(onEnableCoroutine());
        //Next, start polling. For now, this coroutine will simply get an update and display it every second. Later on, if time permits, can make this more sophisticated via the scheme described here, checking for return codes 408 and 200.
        //https://github.com/kartoffelquadrat/AsyncRestLib#client-long-poll-counterpart (This would likely require changing the LobbyService.cs script, as well as the refreshSuccess function(s)).
    }

    // public void testCallback(string message){
    //     Debug.Log("Reached test callback method! Message recieved is: '" + message + "'");
    // }

    public void changeInfoText(string input){
        infoText.text = input;
        Invoke("clearInfoText", 5f);
    }

    private void clearInfoText(){
        infoText.text = " ";
    }

    private void createSessionAttempt(){
        thisClient.createSession(); 
    }

    private void createFailure(string inputError){
        infoText.text = "Error in session creation: " + inputError;
        Debug.Log(inputError);
    }

    private async Task createSuccessResult(string result){
        Debug.Log("CreateSuccess called.");

        thisClient.hasSessionCreated = true;
        infoText.text = "Creation sucessful!";
        await thisClient.refreshSessions();
        //Now that the session has been created, we can turn on the sioCom.Instance.
        Debug.Log("On create, session id is: " + thisClient.thisSessionID);
        sioCom.Instance.Emit("join", thisClient.thisSessionID, true);
        sioCom.Instance.On("Launch", callback);
    }

    private void callback(string input){ //Strangeness is potentially caused here. This likely ought to be somewhere in the LobbyScreen, since as of right now this script is attached to the Login Button, which is disabled later.
        //Load the next scene, stopping the polling coroutine.
        //try{
        //Debug.Log("reached callback method!");
        StopAllCoroutines();
        Debug.Log("This client id is: " + thisClient.thisSessionID);
        //sioCom.Instance.Off("Launch"); // Gives a warning, but may not even be necessary. ***
        //sioCom.Instance.Close();
        Debug.Log("Socket ID in lobby: " + thisClient.socket.Instance.SocketID);
        Debug.Log("Before loading scene, socket status: " + thisClient.socket.Instance.Status);
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
    }


     public async Task deleteSuccess(string input){
        infoText.text = "Deletion successful!";
        Debug.Log("Delete success: " + input);
        thisClient.hasSessionCreated = false;
        thisClient.mySession = null;
        await thisClient.refreshSessions();
    }

    public void deleteFailure(string error){
        infoText.text = "Deletion failed! Error: " + error;
        Debug.Log("Delete failure: " + error);
    }

    //May be unnecessary. *** (Maybe open the socket here?)
    public void launchSuccess(string input) {
        Debug.Log("Launch success: " + input);
    }
    //May be unecessary.
    public void launchFailure(string error){
        //infoText.text = "LS launch failed! Error: " + error;
        //Debug.Log("Launch failure: " + error);
    }

    public async Task joinSuccess(string input){
        infoText.text = "Join successful!";
        Debug.Log("Join success: " + input);
        await thisClient.refreshSessions();
        //Now that the session has been joined, we can turn on the sioCom.Instance.
        sioCom.Instance.Emit("join", thisClient.thisSessionID,true);
        SessionInfo.Instance().savegame_id = thisClient.mySession.saveID;
        sioCom.Instance.On("Launch", callback);
        //Debug.Log(this.thisClient.thisSessionID);
    }

    public void joinFailure(string error){
        infoText.text = "Join failed! Error: " + error;
        Debug.Log("Join failure: " + error);
    }

    public async Task leaveSuccess(string input){
        infoText.text = "Leave successful!";
        Debug.Log("Leave success: " + input);
        await thisClient.refreshSessions();
        sioCom.Instance.Emit("leave", thisClient.thisSessionID,true);
        thisClient.mySession = null;
        sioCom.Instance.Off("Launch", callback);

    }

    public void leaveFailure(string error){
        infoText.text = "Leave failed! Error: " + error;
        Debug.Log("Leave failure: " + error);
    }

    private void refreshFailure(string error){
        infoText.text = "Error in refresh:" + error;
        Debug.Log("Error in refresh: " + error);
    }

    private void refreshSuccess(string result){
        //Debug.Log("Refresh str: " + result);
        //Debug.Log(result);
        // thisClient.hasSessionCreated = false;
        // var jsonString = result.Replace('"', '\"');

        // //After getting a bunch of sessions, we need to use the result string to create rows.
        // //From the result string, we only need to store the session ID, launch state, host player and players somewhere - parameters will be the same for all games so those don't matter. (THOUGH LATER WILL NEED TO DEAL WITH SAVEFILES HERE)
        // JObject myObj = JObject.Parse(jsonString);
        // JObject trueObj = JObject.Parse(myObj["sessions"].ToString()); 
        // List<string> sessionIDs = new List<string>();
        // foreach(JProperty prop in trueObj.Properties()){
        //     sessionIDs.Add(prop.Name);
        // }
        // List<Session> foundSessions = new List<Session>();
        // foreach(string ID in sessionIDs){
        //     #pragma warning disable 0618
        //     foundSessions.Add(new Session(WWW.EscapeURL(ID), trueObj[ID]["creator"].ToString(), trueObj[ID]["players"].ToString(), trueObj[ID]["launched"].ToString(), trueObj[ID]["savegameid"].ToString()));
        //     #pragma warning restore 0618
        //     if(trueObj[ID]["creator"].ToString() == thisClient.clientCredentials.username){
        //         thisClient.hasSessionCreated = true; //If our client is a host in one of the recieved session
        //         thisClient.thisSessionID = ID;
        //     }
        // }
        // thisClient.sessions = foundSessions; //Set the client's new list of found sessions.

        // foreach (Session session in foundSessions)
        // {
        //     if (session.players.Contains(thisClient.clientCredentials.username))
        //     {
        //         thisClient.thisSessionID = session.sessionID;
        //         thisClient.mySession = session;
        //         //persistentObject.GetComponent<SessionInfo>().setClient();
        //         SessionInfo.Instance().setClient();
        //     }else{
        //         thisClient.mySession = null;
        //     }
        // }
        // //Call something here to visually update the rows of the table based on the client info (excluding launched sessions). These rows should include a "Launch" button if it is the current client's session, and a "join" button otherwise.
        // displaySessions();
    }

    private void displaySessions() {
        Debug.Log("DisplaySessions called");
        List<Session> foundSessions = new List<Session>();
        foreach (SessionJSON sesh in sessions) {
            foundSessions.Add(new Session(sesh.id, sesh.creator, sesh.players, sesh.launched, sesh.savegameid, sesh.gameParameters.maxSessionPlayers));
        }
        
        foreach(SessionJSON session in sessions) {
            //Make the new row.
            List<string> playersList = new List<string>(session.players);
            if( session.launched && ( (!(Client.Instance().clientCredentials.username == "Elfenroads")) || (playersList.Contains(Client.Instance().clientCredentials.username)) ) ){ //If we find a session which was launched, no point to show it.
                continue;
            }
            if(tableRow == null){
                return;
            }

            GameObject instantiatedRow = Instantiate(tableRowPrefab, tableRow.transform); //0 is hostname, 1 is ready players
            //Set the strings for "Hostname" and "readyPlayers"
            try{
                Session legacySession = new Session(session.id, session.creator, session.players, session.launched, session.savegameid, session.gameParameters.maxSessionPlayers);
                instantiatedRow.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = session.creator;
                instantiatedRow.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text = session.players.Length + "/" + session.gameParameters.maxSessionPlayers;

                //Based on session attributes, decide what button (if any) should be added.
                if (Client.Instance().clientCredentials.username == session.creator && session.players.Length >= 2) {
                    GameObject instantiatedButton = Instantiate(launchButton, instantiatedRow.transform);
                    instantiatedButton.GetComponent<LaunchScript>().setSession(legacySession);
                } else if ((Client.Instance().clientCredentials.username == session.creator && session.players.Length < 2) || playersList.Contains(Client.Instance().clientCredentials.username)) {
                    //Why is this here?
                } else{
                    GameObject instantiatedButton = Instantiate(joinButton, instantiatedRow.transform);
                    instantiatedButton.GetComponent<JoinScript>().setSession(legacySession);
                }

                if (Client.Instance().clientCredentials.username == session.creator || Client.Instance().clientCredentials.username == "Elfenroads"){
                    GameObject instantiatedButton = Instantiate(deleteButton, instantiatedRow.transform);
                    instantiatedButton.GetComponent<DeleteScript>().setSession(legacySession);
                } else if(playersList.Contains(Client.Instance().clientCredentials.username) && (Client.Instance().clientCredentials.username != session.creator)){
                    GameObject instantiatedButton = Instantiate(leaveButton, instantiatedRow.transform);
                    instantiatedButton.GetComponent<LeaveScript>().setSession(legacySession);
                }

            }catch (Exception e){ //Try-catch put here for the case where "displaySessions" was running at the exact time the session was launched.
                Debug.Log(e);
            }
        }
    }
}
