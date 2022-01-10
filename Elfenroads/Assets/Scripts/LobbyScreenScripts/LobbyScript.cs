using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;
using Firesplash.UnityAssets.SocketIO;
using UnityEngine.SceneManagement;

public class LobbyScript : MonoBehaviour
{

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

        //Get the socket to start listening for a "StartGame" message.
        thisClient.setSioCom(sioCom);
        sioCom.Instance.On("StartGame", callback);
        Debug.Log(sioCom.Instance.IsConnected());

        //Next, start polling. For now, this coroutine will simply get an update and display it every second. Later on, if time permits, can make this more sophisticated via the scheme described here, checking for return codes 408 and 200.
        //https://github.com/kartoffelquadrat/AsyncRestLib#client-long-poll-counterpart (This would likely require changing the LobbyService.cs script, as well as the refreshSuccess function(s)).
        StartCoroutine("pollingRoutine");
    }

    public void changeInfoText(string input){
        infoText.text = input;
        Invoke("clearInfoText", 5f);
    }

    private void clearInfoText(){
        infoText.text = "";
    }

    private void createSessionAttempt(){
        thisClient.createSession(); 
    }

    private void createFailure(string inputError){
        Debug.Log(inputError);
    }

    private void createSuccessResult(string result){
        thisClient.hasSessionCreated = true;
        thisClient.refreshSessions();
        Debug.Log(result);
    }

    private void callback(string input){ //Strangeness is potentially caused here. This likely ought to be somewhere in the LobbyScreen, since as of right now this script is attached to the Login Button, which is disabled later.
        //Load the next scene.
        Debug.Log("reached callback method!");
        //TODO: Here, also add a function which will cancel the polling coroutine (since, now that we've joined the game, there's no reason to continue polling).
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
        sioCom.Instance.Off("StartGame");
    }

    private void refreshSessions(){
        thisClient.refreshSessions();
    }

    private void refreshFailure(string error){
        infoText.text = "Error in refresh:" + error;
        Debug.Log("Error in refresh: " + error);
    }

    private void refreshSuccess(string result){
        Debug.Log(result);

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
            foundSessions.Add(new Session(WWW.EscapeURL(ID), trueObj[ID]["creator"].ToString(), trueObj[ID]["players"].ToString(), trueObj[ID]["launched"].ToString()));
            if(trueObj[ID]["creator"].ToString() == thisClient.clientCredentials.username){
                thisClient.hasSessionCreated = true; //If our client is a host in one of the recieved session
            }
        }
        thisClient.sessions = foundSessions; //Set the client's new list of found sessions.
        //Call something here to visually update the rows of the table based on the client info (excluding launched sessions). These rows should include a "Launch" button if it is the current client's session, and a "join" button otherwise.
        displaySessions(foundSessions);
    }
    
    private void displaySessions(List<Session> foundSessions) {
        foreach (Transform child in tableRow.transform) {
            Destroy(child.gameObject);
        }

        foreach(Session session in foundSessions) {
            //Make the new row.
            // if(session.launched){ //If we find a session which was launched, no point to show it.
            //     continue;
            // }

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
