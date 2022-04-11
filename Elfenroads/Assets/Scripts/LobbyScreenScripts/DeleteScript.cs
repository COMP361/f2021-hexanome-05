using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteScript : MonoBehaviour
{
    private Session mySession = null;
    private Client client = Client.Instance();
    private Button deleteButton;


    void Start(){
        deleteButton = gameObject.GetComponent<Button>(); 
        deleteButton.onClick.AddListener(deleteGame);
    }

    public void setSession(Session aSession){
        mySession = aSession;
    }

    private void deleteGame(){
        if(mySession == null){
            Debug.Log("Error in DeleteButton, session was never set");
        }else{
            client.delete(mySession);
        }
    }
}
