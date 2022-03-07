using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeScript : MonoBehaviour
{
    private Client client = Client.Instance();
    private SessionInfo currentSession;
    private bool isElfenLand = true;
    
    void Start(){
        currentSession = GameObject.Find("SessionInfo").GetComponent<SessionInfo>();
    }

    public void toggleGameMode(bool isChecked){
        if(isChecked){
            currentSession.setIsElfenLand(false);
        }else{
            currentSession.setIsElfenLand(true);
        }
    }
}
