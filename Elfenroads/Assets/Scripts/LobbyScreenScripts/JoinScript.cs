using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JoinScript : MonoBehaviour
{
    private Session mySession = null;
    private Client client = Client.Instance();
    private Button joinButton;


    void Start(){
        joinButton = gameObject.GetComponent<Button>(); //*** Should be fine but slight doubt
        joinButton.onClick.AddListener(joinGame);
    }

    public void setSession(Session aSession){
        mySession = aSession;
    }

    private void joinGame(){
        if(mySession == null){
            Debug.Log("Error in joinButton, session was never set");
        }else{
            client.join(mySession);
        }
    }
}
