using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VariantScript : MonoBehaviour
{
    private Client client = Client.Instance();
    private SessionInfo currentSession;
    private bool isLongerGame = true;
    private bool isHomeTown = true;
    
    void Start(){
        currentSession = GameObject.Find("SessionInfo").GetComponent<SessionInfo>();
    }

    public void toggleGameMode(bool isChecked){
        if(isChecked){
            currentSession.setIsElfenGold(true);
        }else{
            currentSession.setIsElfenGold(false);
        }
    }
}
