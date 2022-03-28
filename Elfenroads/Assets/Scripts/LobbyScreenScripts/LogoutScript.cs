using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LogoutScript : MonoBehaviour
{
    private Session mySession = null;
    private Button logoutButton;
    private Client thisClient;

    public GameObject lobbyScreen;
    public GameObject loginScreen;

    void Start()
    {
        thisClient = Client.Instance();
        logoutButton = this.gameObject.GetComponent<Button>();

        logoutButton.onClick.AddListener(logout);
    }

    public void setSession(Session aSession){
        mySession = aSession;
    }

    void logout(){
        if(mySession != null) {
            thisClient.leave(mySession);
        }

        // Client.ResetInstance();
        // socket.Instance.Close();
        // Destroy(GameObject.Find("SessionInfo"));

        //SceneManager.LoadSceneAsync("Lobby Screen");

        // lobbyScreen = SceneManager.GetSceneByName("Lobby Screen");
        // loginScreen = SceneManager.GetSceneByName("Login Screen");

        lobbyScreen.SetActive(false);
        loginScreen.SetActive(true);
    }
}
