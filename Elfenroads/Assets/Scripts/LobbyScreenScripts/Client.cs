using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : ClientInterface
{
    private static Client instance; // Using the singleton pattern

    // returns singleton instance.
    public static Client Instance() {
        if (instance == null) {
            instance = new Client();
        }

        return instance;
    }

    private LobbyService lobbyService;
    public Player thisPlayer;
    public event LoginSuccess LoginSuccessEvent;
    public event LoginFailure LoginFailureEvent;

    public Client(){
        this.lobbyService = new LobbyService();
        thisPlayer = new Player();

        lobbyService.LoginSuccessEvent += (data) => LoginSuccessEvent(data);
        //lobbyService.LoginFailureEvent += (error) => LoginFailureEvent(error); //Don't need this for the demo functionality so left it out
    }

    public void Login(string username, string password){
        lobbyService.Login(username, password);
    }

}
