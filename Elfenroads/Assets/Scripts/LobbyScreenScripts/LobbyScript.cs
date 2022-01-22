using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Firesplash.UnityAssets.SocketIO;

public class LobbyScript : MonoBehaviour
{

    //Known bug: When a session does not already exist (for the user), when they press "create" the game tells them they already have a session (it still creates the session - the warning message is the thing that's wrong) ***
    IEnumerator pollingRoutine(){ //Just asks the LS for sessions every second, and displays the result.
        while(true){    
            thisClient.refreshSessions();
            yield return new WaitForSeconds(1f);
        }
    }

    public TMPro.TMP_Text infoText;
    public Button createSessionButton;
    //public Button refreshButton;
    
    public GameObject tableRow;
    public GameObject tableRowPrefab;
    public GameObject launchButton;
    public GameObject joinButton;
    public GameObject deleteButton;
    public GameObject persistentObject;
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
        
        thisClient.LaunchSuccessEvent += launchFailure;
        thisClient.LaunchFailureEvent += launchFailure;
        thisClient.DeleteSuccessEvent += deleteSuccess;
        thisClient.DeleteFailureEvent += deleteFailure;
        thisClient.JoinSuccessEvent += joinSuccess;
        thisClient.JoinFailureEvent += joinFailure;


        //Get the sioCom.Instance to start listening
        //sioCom.Instance = GameObject.Find("sioCom.InstanceIO").GetComponent<sioCom.InstanceIOSingleton>().Instance;
        //thisClient.setsioCom.Instance(sioCom.Instance);
        Debug.Log("Socket connection status: " + sioCom.Instance.IsConnected());
        Debug.Log("Socket status: " + sioCom.Instance.Status);
        thisClient.setSocket(sioCom);
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
        sioCom.Instance.Emit("join", thisClient.thisSessionID, true);
        sioCom.Instance.On("Launch", callback);
        Debug.Log("Session ID: " + thisClient.thisSessionID);
    }

    private void callback(string input){ //Strangeness is potentially caused here. This likely ought to be somewhere in the LobbyScreen, since as of right now this script is attached to the Login Button, which is disabled later.
        //Load the next scene, stopping the polling coroutine.
        try{
        Debug.Log("reached callback method!");
        StopCoroutine("pollingRoutine");
        Debug.Log("Couroutine stopped! Turning off the socket!");
        sioCom.Instance.Off("StartGame");
        Debug.Log("About to load scene!");
        SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }catch (Exception e){
            Debug.Log(e.Message);
        }
    }


     public async Task deleteSuccess(string input){
        infoText.text = "Deletion successful!";
        Debug.Log("Delete success: " + input);
        thisClient.hasSessionCreated = false;
        await thisClient.refreshSessions();
    }

    public void deleteFailure(string error){
        infoText.text = "Deletion failed! Error: " + error;
        Debug.Log("Delete failure: " + error);
    }

    //May be unnecessary.
    public void launchSuccess(string input){
        Debug.Log("Launch success: " + input);
    }

    public void launchFailure(string error){
        infoText.text = "LS launch failed! Error: " + error;
        Debug.Log("Launch failure: " + error);
    }

    public async Task joinSuccess(string input){
        infoText.text = "Join successful!";
        Debug.Log("Join success: " + input);
        await thisClient.refreshSessions();
        //Now that the session has been created, we can turn on the sioCom.Instance.
        sioCom.Instance.Emit("join", thisClient.thisSessionID,true);
        sioCom.Instance.On("Launch", callback);
        Debug.Log(this.thisClient.thisSessionID);
    }

    public void joinFailure(string error){
        infoText.text = "Join failed! Error: " + error;
        Debug.Log("Join failure: " + error);
    }

    private void refreshFailure(string error){
        infoText.text = "Error in refresh:" + error;
        Debug.Log("Error in refresh: " + error);
    }

    private void refreshSuccess(string result){
        //Debug.Log(result);

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
            foundSessions.Add(new Session(WWW.EscapeURL(ID), trueObj[ID]["creator"].ToString(), trueObj[ID]["players"].ToString(), trueObj[ID]["launched"].ToString()));
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
                persistentObject.GetComponent<SessionInfo>().setSessionID();
            }
        }
        //Call something here to visually update the rows of the table based on the client info (excluding launched sessions). These rows should include a "Launch" button if it is the current client's session, and a "join" button otherwise.
        displaySessions(foundSessions);
    }
    
    private void displaySessions(List<Session> foundSessions) {
        foreach (Transform child in tableRow.transform) {
            Destroy(child.gameObject);
        }

        foreach(Session session in foundSessions) {
            //Make the new row.
            if( session.launched && ( (!(Client.Instance().clientCredentials.username == "Elfenroads")) || (session.players.Contains(Client.Instance().clientCredentials.username)) ) ){ //If we find a session which was launched, no point to show it.
                continue;
            }

            GameObject instantiatedRow = Instantiate(tableRowPrefab, tableRow.transform); //0 is hostname, 1 is ready players
            //Set the strings for "Hostname" and "readyPlayers"
            instantiatedRow.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = session.hostPlayerName;
            instantiatedRow.transform.GetChild(1).GetComponent<TMPro.TMP_Text>().text = session.players.Count + "/6";

            //Based on session attributes, decide what button (if any) should be added.
            if (Client.Instance().clientCredentials.username == session.hostPlayerName && session.players.Count >= 2) {
                GameObject instantiatedButton = Instantiate(launchButton, instantiatedRow.transform);
                instantiatedButton.GetComponent<LaunchScript>().setSession(session);
            } else if ((Client.Instance().clientCredentials.username == session.hostPlayerName && session.players.Count < 2) || session.players.Contains(Client.Instance().clientCredentials.username)) {

            } else{
                GameObject instantiatedButton = Instantiate(joinButton, instantiatedRow.transform);
                instantiatedButton.GetComponent<JoinScript>().setSession(session);
            }

            if(Client.Instance().clientCredentials.username == session.hostPlayerName || Client.Instance().clientCredentials.username == "Elfenroads"){
                GameObject instantiatedButton = Instantiate(deleteButton, instantiatedRow.transform);
                instantiatedButton.GetComponent<DeleteScript>().setSession(session);
            }
        }
    }
}
