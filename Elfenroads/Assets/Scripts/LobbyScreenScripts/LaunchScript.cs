using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaunchScript : MonoBehaviour
{
    private Session mySession = null;
    private Client client = Client.Instance();
    private Button launchButton;


    void Start(){
        launchButton = gameObject.GetComponent<Button>(); //*** Should be fine but slight doubt
        launchButton.onClick.AddListener(launchGame);
    }

    public void setSession(Session aSession){
        mySession = aSession;
    }

    private void launchGame(){
        if(mySession == null || mySession.players.Count <= 2){
            Debug.Log("Error in LaunchButton, session was never set");
            Debug.Log("mySession = " + mySession + ", playerCount = " + mySession.players.Count);
        }else{
            //client.launch();
        }
    }
}
