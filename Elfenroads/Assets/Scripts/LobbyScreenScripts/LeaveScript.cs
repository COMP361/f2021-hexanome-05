using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaveScript : MonoBehaviour
{
    private Session mySession = null;
    private Client client = Client.Instance();
    private Button leaveButton;


    void Start(){
        leaveButton = gameObject.GetComponent<Button>();
        leaveButton.onClick.AddListener(leaveGame);
    }

    public void setSession(Session aSession){
        mySession = aSession;
    }

    private async void leaveGame(){
        if(mySession == null){
            Debug.Log("Error in leaveButton, session was never set");
        }else{
            client.leave(mySession);
            await client.refreshSessions();
        }
    }
}