using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using Models;
using System.Threading.Tasks;

public class LobbyService
{

    public delegate void LoginSuccess(string token);
    public event LoginSuccess LoginSuccessEvent;
    public delegate void LoginFailure(string error);
    public event LoginFailure LoginFailureEvent;

    public delegate void RoleSuccess(string result);
    public event RoleSuccess RoleSuccessEvent;
    public delegate void RoleFailure(string error);
    public event RoleFailure RoleFailureEvent;

    public delegate void RefreshSuccess(string result);
    public event RefreshSuccess RefreshSuccessEvent;
    public delegate void RefreshFailure(string error);
    public event RefreshFailure RefreshFailureEvent;

    public delegate void CreateSuccess(string result);
    public event CreateSuccess CreateSuccessEvent;
    public delegate void CreateFailure(string error);
    public event CreateFailure CreateFailureEvent;

    public delegate void JoinSuccess(string result);
    public event JoinSuccess JoinSuccessEvent;
    public delegate void JoinFailure(string error);
    public event JoinFailure JoinFailureEvent;

    public delegate void LeaveSuccess(string result);
    public event LeaveSuccess LeaveSuccessEvent;
    public delegate void LeaveFailure(string error);
    public event JoinFailure LeaveFailureEvent;

    public delegate void LaunchSuccess(string result);
    public event LaunchSuccess LaunchSuccessEvent;
    public delegate void LaunchFailure(string error);
    public event LaunchFailure LaunchFailureEvent;

    public delegate void DeleteSuccess(string result);
    public event DeleteSuccess DeleteSuccessEvent;
    public delegate void DeleteFailure(string error);
    public event DeleteFailure DeleteFailureEvent;

    const string LS_PATH = "https://mysandbox.icu/LobbyService";

    // Status code reference:
    // https://developer.mozilla.org/en-US/docs/Web/HTTP/Status
    const long STATUS_OK = 200;


    //The login operation.
    public void Login(string username, string password) {
        
        WWWForm data = new WWWForm(); // Using this class is a must for generating POST data, I learned this the hard way!
        data.AddField("grant_type", "password");
        data.AddField("username", username);
        data.AddField("password", password); // no need to urlencode the values, it happens automatically

        UnityWebRequest request = UnityWebRequest.Post(LS_PATH + "/oauth/token", data);

        // base64 encoded string for "bgp-client-name:bgp-client-pw"
        request.SetRequestHeader("Authorization", "Basic YmdwLWNsaWVudC1uYW1lOmJncC1jbGllbnQtcHc=");
        
        request.SendWebRequest().completed += OnLoginCompleted;
    }

    //Called when web request is completed.
    private void OnLoginCompleted(AsyncOperation operation){

        UnityWebRequest request = ((UnityWebRequestAsyncOperation) operation).webRequest;
        if (request.responseCode == STATUS_OK) {
            Debug.Log("Login success!");
            LoginSuccessEvent(request.downloadHandler.text);
        }
        else {
            LoginFailureEvent(request.error + "\n" + request.downloadHandler.text); //Don't need this as part of the demo functionality so I left it out.
        }

        // mark the request for garbage collection
        request.Dispose();
    }

    public void getRole(string token){
        UnityWebRequest request = UnityWebRequest.Get(LS_PATH + "/oauth/role?access_token=" + token);
        UnityWebRequestAsyncOperation operation = request.SendWebRequest();
        operation.completed += OnRoleCompleted;
    }

    private void OnRoleCompleted(AsyncOperation operation){

        UnityWebRequest request = ((UnityWebRequestAsyncOperation) operation).webRequest;
        if (request.responseCode == STATUS_OK) {
            RoleSuccessEvent(request.downloadHandler.text);
        }
        else {
            RoleFailureEvent(request.error + "\n" + request.downloadHandler.text); 
        }

        // mark the request for garbage collection
        request.Dispose();
    }
    
    public async Task refresh(){
        UnityWebRequest request = UnityWebRequest.Get(LS_PATH + "/api/sessions");
        UnityWebRequestAsyncOperation operation = request.SendWebRequest();
        operation.completed += OnRefreshCompleted;

        while(!operation.isDone){
            await Task.Yield();
        }

    }

    private void OnRefreshCompleted(AsyncOperation operation){
        UnityWebRequest request = ((UnityWebRequestAsyncOperation) operation).webRequest;
        if (request.responseCode == STATUS_OK) {
            RefreshSuccessEvent(request.downloadHandler.text);
        }
        else {
            RefreshFailureEvent(request.error + "\n" + request.downloadHandler.text); 
        }

        // mark the request for garbage collection
        request.Dispose();
    }

    public void createSession(string playerName, string token){ //POST /api/sessions. Request body is a json.
        //Doing this differently than in Login bc the LobbyService or SOMETHING was being goofy and wouldn't allow it.
        SessionCreator jsonObj = new SessionCreator();
        jsonObj.creator = playerName;
        string bodyJsonString = JsonUtility.ToJson(jsonObj);
        Debug.Log(bodyJsonString);
        var request = new UnityWebRequest(LS_PATH + "/api/sessions?access_token=" + token, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(bodyJsonString);
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SendWebRequest().completed += OnCreateCompleted;
    }

    private void OnCreateCompleted(AsyncOperation operation){

        UnityWebRequest request = ((UnityWebRequestAsyncOperation) operation).webRequest;
        if (request.responseCode == STATUS_OK) {
            CreateSuccessEvent(request.downloadHandler.text);
        }
        else {
            CreateFailureEvent(request.error + "\n" + request.downloadHandler.text); 
        }

        // mark the request for garbage collection
        request.Dispose();
    }

    public void join(Session aSession, ClientCredentials aClientCredentials){
        
        //This is so wack, but you gotta make this null and not simply an empty list/string for it to work. It is the reason I cry myself to sleep at night.
        byte[] bodyRaw = null;
        UnityWebRequest request = UnityWebRequest.Put(LS_PATH + "/api/sessions/" + aSession.sessionID + "/players/" + aClientCredentials.username + "?access_token=" + aClientCredentials.accessToken, bodyRaw ); 
        UnityWebRequestAsyncOperation operation = request.SendWebRequest();
        operation.completed += OnJoinCompleted;
    }

    private void OnJoinCompleted(AsyncOperation operation){
        UnityWebRequest request = ((UnityWebRequestAsyncOperation) operation).webRequest;
        if (request.responseCode == STATUS_OK) {
            JoinSuccessEvent(request.downloadHandler.text);
        }
        else {
            JoinFailureEvent(request.error + "\n" + request.downloadHandler.text); 
        }

        // mark the request for garbage collection
        request.Dispose();
    }

    public void leave(Session aSession, ClientCredentials aClientCredentials){
        
        //This is so wack, but you gotta make this null and not simply an empty list/string for it to work. It is the reason I cry myself to sleep at night.
        UnityWebRequest request = UnityWebRequest.Delete(LS_PATH + "/api/sessions/" + aSession.sessionID + "/players/" + aClientCredentials.username + "?access_token=" + aClientCredentials.accessToken); 
        UnityWebRequestAsyncOperation operation = request.SendWebRequest();
        operation.completed += OnLeaveCompleted;
    }

    private void OnLeaveCompleted(AsyncOperation operation){
        UnityWebRequest request = ((UnityWebRequestAsyncOperation) operation).webRequest;
        if (request.responseCode == STATUS_OK) {
            // LeaveSuccessEvent(request.downloadHandler.text);
            //Potentially since there is no session to be displayed anymore.
            Debug.Log("Leave successful!");
        }
        else {
            LeaveFailureEvent(request.error + "\n" + request.downloadHandler.text); 
        }

        // mark the request for garbage collection
        request.Dispose();
    }


    public void launch(Session aSession, ClientCredentials aClientCredentials){
        Debug.Log("Request looks like: " + LS_PATH + "/api/sessions/" + aSession.sessionID + "?access_token=" + aClientCredentials.accessToken);
        UnityWebRequest request = UnityWebRequest.Post(LS_PATH + "/api/sessions/" + aSession.sessionID + "?access_token=" + aClientCredentials.accessToken, ""); 
        UnityWebRequestAsyncOperation operation = request.SendWebRequest();
        operation.completed += OnLaunchCompleted;
    }

    private void OnLaunchCompleted(AsyncOperation operation){
        UnityWebRequest request = ((UnityWebRequestAsyncOperation) operation).webRequest;
        if (request.responseCode == STATUS_OK) {
            LaunchSuccessEvent(request.downloadHandler.text);
        }
        else {
            LaunchFailureEvent(request.error + "\n" + request.downloadHandler.text); 
        }

        // mark the request for garbage collection
        request.Dispose();
    }

    public void delete(Session aSession, ClientCredentials aClientCredentials){
        UnityWebRequest request = UnityWebRequest.Delete(LS_PATH + "/api/sessions/" + aSession.sessionID + "?access_token=" + aClientCredentials.accessToken); 
        UnityWebRequestAsyncOperation operation = request.SendWebRequest();
        operation.completed += OnDeleteCompleted;
    }

    private void OnDeleteCompleted(AsyncOperation operation){
        UnityWebRequest request = ((UnityWebRequestAsyncOperation) operation).webRequest;
        if (request.responseCode == STATUS_OK) {
            //Debug.Log("Resulting text is " + request.downloadHandler.text);
            DeleteSuccessEvent("Yay!");
        }
        else {
            DeleteFailureEvent(request.error); 
        }

        // mark the request for garbage collection
        request.Dispose();
    }

}

public class SessionCreator{
    public string creator;
    public string game = "Elfenroads";
    public string savegame = "";
}

