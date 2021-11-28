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
    private string serverPath = "https://fierce-plateau-19887.herokuapp.com/";
    private bool hasRegisteredServer = false;
    private bool isAdmin = false;
    private bool hasSessionCreated = false;
    public Player thisPlayer;
    public event LoginSuccess LoginSuccessEvent;
    public event LoginFailure LoginFailureEvent;
    public event RoleSuccess RoleSuccessEvent;
    public event RoleFailure RoleFailureEvent;
    public event RegisterSuccess RegisterSuccessEvent;
    public event RegisterFailure RegisterFailureEvent;
    public event CreateSuccess CreateSuccessEvent;
    public event CreateFailure CreateFailureEvent;

    public Client(){
        this.lobbyService = new LobbyService();
        thisPlayer = new Player();

        lobbyService.LoginSuccessEvent += (data) => LoginSuccessEvent(data);
        lobbyService.LoginFailureEvent += (error) => LoginFailureEvent(error);
        lobbyService.RoleSuccessEvent += (data) => RoleSuccessEvent(data);
        lobbyService.RoleFailureEvent += (data) => RoleFailureEvent(data);

        this.RoleSuccessEvent += roleSuccess;
        this.RoleFailureEvent += roleFailure;
    }

    public void Login(string username, string password){
        lobbyService.Login(username, password);
    }

    public void getRole(){
        lobbyService.getRole(thisPlayer.getAccToken());
    }

    public void roleSuccess(string input){
        if(input.Substring(15,10).Equals("ROLE_ADMIN")){
            isAdmin = true;
        }else{
            isAdmin = false;
        }
        Debug.Log("isAdmin set to " + isAdmin);
    }

    public void roleFailure(string error){ //For now, just log the error.
        Debug.Log("Getting the role failed with error: "  + error);
    }

    public void createSession(){
        
        //First, we must get the registration status of the Server, and store it into hasRegisteredServer.
        //       *call Server for registration status here*


        if(hasRegisteredServer && !hasSessionCreated){ //If the server is registered, we can call the LS to create a session.

            //hasSessionCreated = true;
            return;
        }
        //If server is not registered and Client has an admin token, send registration request to the Server.
        if(isAdmin){
            //  *call Server for registration request here*
            //createSession(); //Call this again, where we will hopefully reach the first case this time.
            CreateFailureEvent("You're the admin, but this functionality isn't done yet :)"); //Remove later.

        }else{ //If server is not registered and Client has no admin token, we can't do anything so we just return.
            CreateFailureEvent("Server has not registered a game service, and you do not have the permissions to register it!");
            return;
        }
    }
}
