using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoginScript : MonoBehaviour
{
    private Button loginButton;
    private Player thisPlayer;
    private string lobbyServicePath = "https://mysandbox.icu/LobbyService/";

    public GameObject usernameBox;
    public GameObject passwordBox;
    public GameObject loginScreen;
    public GameObject lobbyScreen;

    // Start is called before the first frame update
    void Start()
    {
        //Create Player object and get the button.
        thisPlayer = new Player();
        loginButton = this.gameObject.GetComponent<Button>();

        //Add onclick listener to button for "login" function.
        loginButton.onClick.AddListener(login);
    }

    void login(){
        if(usernameBox.GetComponent<TMP_InputField>().text == "" || passwordBox.GetComponent<TMP_InputField>().text == ""){
            return;
        }else{
            
            
            
            
            
            loginScreen.SetActive(false);
            lobbyScreen.SetActive(true);
        }
    }




}
