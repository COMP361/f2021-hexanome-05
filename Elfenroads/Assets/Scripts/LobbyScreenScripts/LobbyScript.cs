using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Firesplash.UnityAssets.SocketIO;

public class LobbyScript : MonoBehaviour
{

    //Known bug: When a session does not already exist (for the user), when they press "create" the game tells them they already have a session (it still creates the session - the warning message is the thing that's wrong) ***
    IEnumerator pollingRoutine(){ //Just asks the LS for sessions every second, and displays the result.
        // while(true){
        //     #pragma warning disable 4014
        //     thisClient.refreshSessions();
        //     #pragma warning restore 4014
        //     yield return new WaitForSeconds(1f);
        // }

        //Load in the beginning

        const string LS_PATH = "https://mysandbox.icu/LobbyService";

        UnityWebRequest request = UnityWebRequest.Get(LS_PATH + "/api/sessions");

        //Return Codes: 200(success), 408(request timeout)
        //https://developer.mozilla.org/en-US/docs/Web/HTTP/Status
        int returnCode = (int)request.responseCode;

        thisClient.refreshSessions();
        
        while (returnCode == 408){
            //Polling
            request = UnityWebRequest.Get(LS_PATH + "/api/sessions");
            returnCode = (int)request.responseCode;

            yield return new WaitForSeconds(1f);
        }

        if (returnCode == 200){
            thisClient.refreshSessions();
            // RefreshSuccessEvent(request.downloadHandler.text);
        } else {
            // RefreshFailureEvent(request.error + "\n" + request.downloadHandler.text); 
        }

        request.Dispose();

        yield return new WaitForSeconds(1f);
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


    void OnEnable(){

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
        sioCom.Instance.On("join", (msg) => testCallback(msg.ToString()));

        //Next, start polling. For now, this coroutine will simply get an update and display it every second. Later on, if time permits, can make this more sophisticated via the scheme described here, checking for return codes 408 and 200.
        //https://github.com/kartoffelquadrat/AsyncRestLib#client-long-poll-counterpart (This would likely require changing the LobbyService.cs script, as well as the refreshSuccess function(s)).
        StartCoroutine("pollingRoutine");
    }

    public void testCallback(string message){
        Debug.Log("Reached test callback method! Message recieved is: '" + message + "'");
    }

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
        StopCoroutine("pollingRoutine");
        Debug.Log("This client id is: " + thisClient.thisSessionID);
        //Debug.Log("Couroutine stopped! Turning off the socket!");
        //sioCom.Instance.Off("Launch"); // Gives a warning, but may not even be necessary. ***
        //sioCom.Instance.Close();
        Debug.Log("Socket ID in lobby: " + thisClient.socket.Instance.SocketID);
        Debug.Log("Before loading scene, socket status: " + thisClient.socket.Instance.Status);
        //Debug.Log("About to load scene!");
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        // }catch (Exception e){
        //     Debug.Log(e.Message);
        // }
    }


     public async Task deleteSuccess(string input){
        infoText.text = "Deletion successful!";
        Debug.Log("Delete success: " + input);
        thisClient.hasSessionCreated = false;
        thisClient.mySession = null;
        wasDeleted = true;
        await thisClient.refreshSessions();
    }

    public void deleteFailure(string error){
        infoText.text = "Deletion failed! Error: " + error;
        Debug.Log("Delete failure: " + error);
    }

    //May be unnecessary.
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
        thisClient.hasSessionCreated = false;
        var jsonString = result.Replace('"', '\"');

        //After getting a bunch of sessions, we need to use the result string to create rows.
        //From the result string, we only need to store the session ID, launch state, host player and players somewhere - parameters will be the same for all games so those don't matter. (THOUGH LATER WILL NEED TO DEAL WITH SAVEFILES HERE)
        JObject myObj = JObject.Parse(jsonString);
        JObject trueObj = JObject.Parse(myObj["sessions"].ToString()); 
        List<string> sessionIDs = new List<string>();
        foreach(JProperty prop in trueObj.Properties()){
            sessionIDs.Add(prop.Name);
        }
        List<Session> foundSessions = new List<Session>();
        foreach(string ID in sessionIDs){
            #pragma warning disable 0618
            foundSessions.Add(new Session(WWW.EscapeURL(ID), trueObj[ID]["creator"].ToString(), trueObj[ID]["players"].ToString(), trueObj[ID]["launched"].ToString(), trueObj[ID]["savegameid"].ToString()));
            #pragma warning restore 0618
            if(trueObj[ID]["creator"].ToString() == thisClient.clientCredentials.username){
                thisClient.hasSessionCreated = true; //If our client is a host in one of the recieved session
                thisClient.thisSessionID = ID;
            }
        }
        thisClient.sessions = foundSessions; //Set the client's new list of found sessions.

        foreach (Session session in foundSessions)
        {
            if (session.players.Contains(thisClient.clientCredentials.username))
            {
                thisClient.thisSessionID = session.sessionID;
                thisClient.mySession = session;
                //persistentObject.GetComponent<SessionInfo>().setClient();
                SessionInfo.Instance().setClient();
            }else{
                thisClient.mySession = null;
            }
        }
        //Call something here to visually update the rows of the table based on the client info (excluding launched sessions). These rows should include a "Launch" button if it is the current client's session, and a "join" button otherwise.
        displaySessions(foundSessions);
    }
    
    private bool wasDeleted = false;

    private void displaySessions(List<Session> foundSessions) {
        bool createdRowExists = false;
        GameObject existingRow = null;

        if(tableRow == null){
            return;
        }
        foreach (Transform child in tableRow.transform) {
            if(child == null){
                return;
            }else if(child.transform.GetChild(0).gameObject.GetComponent<TMPro.TMP_Text>().text == thisClient.clientCredentials.username && !wasDeleted){
                createdRowExists = true;
                existingRow = child.gameObject;
                wasDeleted = false;
            }else{
                Destroy(child.gameObject);
            }
        }

        foreach(Session session in foundSessions) {
            if(session.hostPlayerName == Client.Instance().clientCredentials.username && createdRowExists){
                if(session.players.Count >= 2 && existingRow.transform.childCount != 4){ //Change this value
                    GameObject instantiatedButton = Instantiate(launchButton, existingRow.transform);
                    instantiatedButton.transform.SetSiblingIndex(2);
                    instantiatedButton.GetComponent<LaunchScript>().setSession(session);
                    existingRow.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text = session.players.Count + "/6";
                }
                continue;
            }
            //Make the new row.
            if( session.launched && ( (!(Client.Instance().clientCredentials.username == "Elfenroads")) || (session.players.Contains(Client.Instance().clientCredentials.username)) ) ){ //If we find a session which was launched, no point to show it.
                continue;
            }
            if(tableRow == null){
                return;
            }

            GameObject instantiatedRow = Instantiate(tableRowPrefab, tableRow.transform); //0 is hostname, 1 is ready players
            //Set the strings for "Hostname" and "readyPlayers"
            try{
                instantiatedRow.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = session.hostPlayerName;
                instantiatedRow.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text = session.players.Count + "/6";

                //Based on session attributes, decide what button (if any) should be added.
                if (Client.Instance().clientCredentials.username == session.hostPlayerName && session.players.Count >= 2) {
                    GameObject instantiatedButton = Instantiate(launchButton, instantiatedRow.transform);
                    instantiatedButton.GetComponent<LaunchScript>().setSession(session);
                } else if ((Client.Instance().clientCredentials.username == session.hostPlayerName && session.players.Count < 2) || session.players.Contains(Client.Instance().clientCredentials.username)) {
                    //Why is this here?
                } else{
                    GameObject instantiatedButton = Instantiate(joinButton, instantiatedRow.transform);
                    instantiatedButton.GetComponent<JoinScript>().setSession(session);
                }

                if(Client.Instance().clientCredentials.username == session.hostPlayerName || Client.Instance().clientCredentials.username == "Elfenroads"){
                    GameObject instantiatedButton = Instantiate(deleteButton, instantiatedRow.transform);
                    instantiatedButton.GetComponent<DeleteScript>().setSession(session);
                }else if(session.players.Contains(Client.Instance().clientCredentials.username) && (Client.Instance().clientCredentials.username != session.hostPlayerName)){
                    GameObject instantiatedButton = Instantiate(leaveButton, instantiatedRow.transform);
                    instantiatedButton.GetComponent<LeaveScript>().setSession(session);
                }

            }catch (Exception e){ //Try-catch put here for the case where "displaySessions" was running at the exact time the session was launched.
                Debug.Log(e);
            }
        }
    }
}
