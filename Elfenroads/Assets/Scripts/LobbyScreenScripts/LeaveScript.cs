using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaveScript : MonoBehaviour
{
    private Session mySession = null;
    private Client client = Client.Instance();
    private Button joinButton;


    void Start(){
        joinButton = gameObject.GetComponent<Button>();
        joinButton.onClick.AddListener(joinGame);
    }

    public void setSession(Session aSession){
        mySession = aSession;
    }

    private async void joinGame(){
        if(mySession == null){
            Debug.Log("Error in leaveButton, session was never set");
        }else{
            client.leave(mySession);
            await client.refreshSessions();
        }
    }
}