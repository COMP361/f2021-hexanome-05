using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json.Linq;

public class LoginScript : MonoBehaviour
{
    private Button loginButton;
    public TMPro.TMP_Text loginInfoText;
    public Button loginElfenroadsButton;
    private Client thisClient;

    public GameObject usernameBox;
    public GameObject passwordBox;
    public GameObject loginScreen;
    public GameObject lobbyScreen;
    

    // Start is called before the first frame update
    void Start()
    {
        //Create Client object and get the button.
        thisClient = Client.Instance();
        loginButton = this.gameObject.GetComponent<Button>();


        //Add onclick listener to button for "login" function.
        loginButton.onClick.AddListener(login);
        loginElfenroadsButton.onClick.AddListener(loginElfenroads);

        thisClient.LoginSuccessEvent += loginSuccessResult;
        thisClient.LoginFailureEvent += loginFailure;
    }

    //Function mostly just ensure fields are filled, and then calls the "LobbyService"'s script through the client.
    void login(){
        thisClient.hasSessionCreated = false;
        thisClient.sessions = null;
        thisClient.clientCredentials = new ClientCredentials();
        thisClient.mySession = null;
        thisClient.thisSessionID = null;
        SessionInfo.Instance().savegame_id = "";    
        if(usernameBox.GetComponent<TMP_InputField>().text == "" || passwordBox.GetComponent<TMP_InputField>().text == ""){
            return;
        }else{
            //Here, we'll call Login() on the Client (and thus the LobbyService).
            loginInfoText.text = "Logging in...";
            thisClient.Login(usernameBox.GetComponent<TMP_InputField>().text,passwordBox.GetComponent<TMP_InputField>().text);
        }
    }
    void loginElfenroads(){
        loginInfoText.text = "Logging in...";
        thisClient.Login("Elfenroads", "$#Y+qDctAF3Fvk?@");
    }


    //This is called by a LoginSuccessEvent, it stores the token into the Client's Player and moves on to the next screen.
    void loginSuccessResult(string response){
        //Debug.Log(token);
#pragma warning disable 0618
        JObject parsed = JObject.Parse(response);
        thisClient.clientCredentials.accessToken = WWW.EscapeURL(((JValue) parsed["access_token"]).ToObject<string>());
        thisClient.clientCredentials.refreshToken = ((JValue) parsed["refresh_token"]).ToObject<string>();
        thisClient.tokenTimeout = ((JValue) parsed["expires_in"]).ToObject<int>();
#pragma warning restore 0618
        //thisClient.thisPlayer.setAccToken(token.Substring(17, 28).Replace("+", "%2B"));
        //thisClient.thisPlayer.setRefToken(token.Substring(86,28).Replace("+", "%2B"));
        //Debug.Log("Player acc token: " + thisClient.clientCredentials.accessToken);
        //Debug.Log("Player ref token: " + thisClient.clientCredentials.refreshToken);

        lobbyScreen = GameObject.Find("Canvas").transform.Find("Lobby Screen").gameObject;
        loginScreen = GameObject.Find("Canvas").transform.Find("Login Screen").gameObject;
        if (loginScreen.activeSelf)  {
            loginScreen.SetActive(false);
            lobbyScreen.SetActive(true);
        }
    }

    void loginFailure(string error){
        Debug.Log("Login failed with error: " + error);
        loginInfoText.text = "Login failed with error: " + error;
    }



}
