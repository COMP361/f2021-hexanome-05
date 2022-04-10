using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginScript : MonoBehaviour
{
    private Button loginButton;
    public TMPro.TMP_Text loginInfoText;
    public Button loginRyanButton;
    public Button loginJoergButton;
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
        loginRyanButton.onClick.AddListener(loginRyan);
        loginJoergButton.onClick.AddListener(loginJoerg);
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

    void loginRyan(){
        loginInfoText.text = "Logging in...";
        thisClient.Login("ryan", "abc123_ABC123");
    }

    void loginJoerg(){  
        loginInfoText.text = "Logging in...";
        thisClient.Login("joerg", "abc123_ABC123");
    }

    void loginElfenroads(){
        loginInfoText.text = "Logging in...";
        thisClient.Login("Elfenroads", "$#Y+qDctAF3Fvk?@");
    }


    //This is called by a LoginSuccessEvent, it stores the token into the Client's Player and moves on to the next screen.
    void loginSuccessResult(string token){
        //Debug.Log(token);
#pragma warning disable 0618
        thisClient.clientCredentials.accessToken = WWW.EscapeURL(token.Substring(17,28));
        thisClient.clientCredentials.refreshToken = WWW.EscapeURL(token.Substring(86,28));
#pragma warning restore 0618
        //thisClient.thisPlayer.setAccToken(token.Substring(17, 28).Replace("+", "%2B"));
        //thisClient.thisPlayer.setRefToken(token.Substring(86,28).Replace("+", "%2B"));
        //Debug.Log("Player acc token: " + thisClient.clientCredentials.accessToken);
        //Debug.Log("Player ref token: " + thisClient.clientCredentials.refreshToken);

        lobbyScreen = GameObject.Find("Canvas").transform.Find("Lobby Screen").gameObject;
        loginScreen = GameObject.Find("Canvas").transform.Find("Login Screen").gameObject;
        loginScreen.SetActive(false);
        lobbyScreen.SetActive(true);
    }

    void loginFailure(string error){
        Debug.Log("Login failed with error: " + error);
        loginInfoText.text = "Login failed with error: " + error;
    }



}
