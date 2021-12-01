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
    private string serverPath = "https://fierce-plateau-19887.herokuapp.com/"; // Did this get changed? ***
    private bool isAdmin = false;
    public bool hasSessionCreated = false;
    public Player thisPlayer;

    public event LoginSuccess LoginSuccessEvent;
    public event LoginFailure LoginFailureEvent;
    public event RoleSuccess RoleSuccessEvent;
    public event RoleFailure RoleFailureEvent;
    public event RefreshSuccess RefreshSuccessEvent;
    public event RefreshFailure RefreshFailureEvent;
    public event CreateSuccess CreateSuccessEvent;
    public event CreateFailure CreateFailureEvent;
    public event JoinSuccess JoinSuccessEvent;
    public event JoinFailure JoinFailureEvent;
    public List<Session> sessions = new List<Session>();

    public Client(){
        this.lobbyService = new LobbyService();
        thisPlayer = new Player();

        lobbyService.LoginSuccessEvent += (data) => LoginSuccessEvent(data);
        lobbyService.LoginFailureEvent += (error) => LoginFailureEvent(error);
        lobbyService.RoleSuccessEvent += (data) => RoleSuccessEvent(data);
        lobbyService.RoleFailureEvent += (data) => RoleFailureEvent(data);
        lobbyService.RefreshSuccessEvent += (data) => RefreshSuccessEvent(data);
        lobbyService.RefreshFailureEvent += (data) => RefreshFailureEvent(data);
        lobbyService.CreateSuccessEvent += (data) => CreateSuccessEvent(data);
        lobbyService.CreateFailureEvent += (data) => CreateFailureEvent(data);
        lobbyService.JoinSuccessEvent += (data) => JoinSuccessEvent(data);
        lobbyService.JoinFailureEvent += (data) => JoinFailureEvent(data);

        this.RoleSuccessEvent += roleSuccess;
        this.RoleFailureEvent += roleFailure;
        this.JoinSuccessEvent += joinSuccess;
        this.JoinFailureEvent += joinFailure;
    }

    public void Login(string username, string password){
        thisPlayer.setName(username);
        thisPlayer.setPassword(password);
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

    public void refreshSessions(){
        lobbyService.refresh();
    }

    public void join(Session aSession){
        lobbyService.join(aSession, thisPlayer);
    }

    public void joinSuccess(string input){

    }

    public void joinFailure(string error){

    }

    public void createSession(){
        
        //First, we must get the registration status of the Server, and store it into hasRegisteredServer.
        //       *call Server for registration status here*


        if(!hasSessionCreated){ //If the client doesn't already have a created session, we can create one.
            lobbyService.createSession(thisPlayer.getName(), thisPlayer.getAccToken());
            hasSessionCreated = true;
            return;
        }else{
            Debug.Log("You already have a session!");
            return; //Otherwise, we don't want to allow someone to create a bunch of sessions so we just return.
        }
    }
}
