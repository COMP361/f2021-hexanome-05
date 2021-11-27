using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyService
{

    public delegate void LoginSuccess(string token);
    public event LoginSuccess LoginSuccessEvent;

    public delegate void LoginFailure(string error);
    public event LoginFailure LoginFailureEvent;

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
            Debug.Log("Login failed!");
            //LoginFailureEvent(request.error + "\n" + request.downloadHandler.text); //Don't need this as part of the demo functionality so I left it out.
        }

        // mark the request for garbage collection
        request.Dispose();
    }
}
