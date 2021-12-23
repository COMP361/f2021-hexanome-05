    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Firesplash.UnityAssets.SocketIO;
using UnityEngine.SceneManagement;

public class LoginScript : MonoBehaviour
{
    private Button loginButton;
    private Client thisClient;

    public SocketIOCommunicator sioCom;
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

        thisClient.LoginSuccessEvent += loginSuccessResult;
        thisClient.LoginFailureEvent += loginFailure;
    }

    //Function mostly just ensure fields are filled, and then calls the "LobbyService"'s script through the client.
    void login(){
        if(usernameBox.GetComponent<TMP_InputField>().text == "" || passwordBox.GetComponent<TMP_InputField>().text == ""){
            return;
        }else{
            //Here, we'll call Login() on the Client (and thus the LobbyService).
            thisClient.Login(usernameBox.GetComponent<TMP_InputField>().text,passwordBox.GetComponent<TMP_InputField>().text);
        }
    }

    //This is called by a LoginSuccessEvent, it stores the token into the Client's Player and moves on to the next screen.
    void loginSuccessResult(string token){
        Debug.Log(token);
        thisClient.thisPlayer.setAccToken(WWW.EscapeURL(token.Substring(17,28)));
        thisClient.thisPlayer.setRefToken(WWW.EscapeURL(token.Substring(86,28)));
        //thisClient.thisPlayer.setAccToken(token.Substring(17, 28).Replace("+", "%2B"));
        //thisClient.thisPlayer.setRefToken(token.Substring(86,28).Replace("+", "%2B"));
        Debug.Log("Player acc token: " + thisClient.thisPlayer.getAccToken());
        Debug.Log("Player ref token: " + thisClient.thisPlayer.getRefToken());

        //Replace this with the beginning of long-polling.
        //Need to make an initial call to getSessions (currently called refreshSessions - need to update it so that it can take a hash (or not))
        //and store the returned string somewhere. Then, get a MD5 hash of the returned string, and call getSessions again with that hash as a
        //parameter.
        //The alternative way would be to do regular polling. Just create a loop, and while the return code is 408, ask for the update again.
        //as soon as the return code is 200, then we can update and then just get back into the loop, as described in a short code snippet here
        // https://github.com/kartoffelquadrat/AsyncRestLib#client-long-poll-counterpart
        //You could import System.Security.Cryptography. And then use MD5CryptoServiceProvider(), ComputeHash() and so on. 
        thisClient.refreshSessions();
        thisClient.setSioCom(sioCom);
        Debug.Log(sioCom.Instance.IsConnected());
        sioCom.Instance.On("StartGame", callback);

        loginScreen.SetActive(false);
        lobbyScreen.SetActive(true);
    }

    private void callback(string input){
        //Load the next scene.
        Debug.Log("reached callback method!");
        //TODO: Here, also add a function which will cancel the long-polling (since, now that we've joined the game, there's no reason to continue polling).
        SceneManager.LoadScene("MainScene", LoadSceneMode.Single);
        //sioCom.Instance.Off("StartGame");
    }

    void loginFailure(string error){
        Debug.Log("Login failed with error: " + error);
    }



}
