using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;
using SocketIOClient;
using System.Threading.Tasks;
using Firesplash.UnityAssets.SocketIO;
using System;

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

    public static void ResetInstance() {
        instance = null;
    }

    private LobbyService lobbyService;
    //private bool isAdmin = false;
    public bool hasSessionCreated = false;
    public ClientCredentials clientCredentials;
    public SocketIOCommunicator socket;
    public string thisSessionID;
    public Session mySession;

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
    public event LeaveSuccess LeaveSuccessEvent;
    public event LeaveFailure LeaveFailureEvent;
    public event LaunchSuccess LaunchSuccessEvent;
    public event LaunchFailure LaunchFailureEvent;
    public event DeleteSuccess DeleteSuccessEvent;
    public event DeleteFailure DeleteFailureEvent;

    public List<Session> sessions = new List<Session>();

    public Client(){
        this.lobbyService = new LobbyService();
        clientCredentials = new ClientCredentials();

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
        lobbyService.LeaveSuccessEvent += (data) => LeaveSuccessEvent(data);
        lobbyService.LeaveFailureEvent += (data) => LeaveFailureEvent(data);
        lobbyService.LaunchSuccessEvent += (data) => LaunchSuccessEvent(data);
        lobbyService.LaunchFailureEvent += (data) => LaunchFailureEvent(data);
        lobbyService.DeleteSuccessEvent += (data) => DeleteSuccessEvent(data);
        lobbyService.DeleteFailureEvent += (data) => DeleteFailureEvent(data);
        
        // this.RoleSuccessEvent += roleSuccess;
        // this.RoleFailureEvent += roleFailure;
    }

    public void setSocket(SocketIOCommunicator socket){
        this.socket = socket;
    }

    public void Login(string username, string password){
        clientCredentials.username = username;
        clientCredentials.password = password;
        lobbyService.Login(username, password);
    }

    // public void getRole(){
    //     lobbyService.getRole(clientCredentials.refreshToken);
    // }

    // public void roleSuccess(string input){
    //     if(input.Substring(15,10).Equals("ROLE_ADMIN")){
    //         isAdmin = true;
    //     }else{
    //         isAdmin = false;
    //     }
    //     Debug.Log("isAdmin set to " + isAdmin);
    // }

    // public void roleFailure(string error){ //For now, just log the error.
    //     Debug.Log("Getting the role failed with error: "  + error);
    // }

    public async Task refreshSessions(){
        await lobbyService.refresh();
    }

    public void join(Session aSession){
        lobbyService.join(aSession, clientCredentials);
    }

    public void leave(Session aSession){
        lobbyService.leave(aSession, clientCredentials);
    }

    public void launch(Session aSession){
        lobbyService.launch(aSession, clientCredentials);
        
    }

    public void delete(Session aSession){
        lobbyService.delete(aSession, clientCredentials);
    }

    public async Task<List<String>> getSavegames() {
        return await lobbyService.getSavegames(); // this is as dumb as it looks. what does this Client class do, anyway?
    }

    public Session getSessionByID(string ID){
        foreach(Session session in sessions){
            if(session.sessionID == ID){
                return session;
            }
        }
        return null;
    }

    public void createSession(){
        
        //First, we must get the registration status of the Server, and store it into hasRegisteredServer.
        //       *call Server for registration status here*


        if(!hasSessionCreated || mySession == null){ //If the client doesn't already have a created session, we can create one.
            lobbyService.createSession(clientCredentials.username, clientCredentials.accessToken);
            return;
        }else{
            GameObject.Find("Lobby Screen").GetComponent<LobbyScript>().changeInfoText("You already have a session or are already in another game!");
            return; //Otherwise, we don't want to allow someone to create a bunch of sessions so we just return.
        }
    }
}

public struct ClientCredentials
{
    public string username;
    public string password;
    public string accessToken;
    public string refreshToken;
}
