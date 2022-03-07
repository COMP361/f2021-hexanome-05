using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ModeScript : MonoBehaviour
{
    private Session mySession = null;
    private Client client = Client.Instance();
    private SessionInfo currentSession;

    private Button modeButton;
    private Text mode = null;
    private int counter = 0;
    
    void Start(){
        currentSession = GameObject.Find("SessionInfo").GetComponent<SessionInfo>();

        modeButton = gameObject.GetComponent<Button>();
        modeButton.onClick.AddListener(selectMode);
    }

    public void setSession(Session aSession) {
        mySession = aSession;
    }

    public async void selectMode(){
        if(mySession == null){
            Debug.Log("Error in modeButton, session was never set");
        } else{
            counter++;

            if (counter % 2 == 1) {
                mode.text = "ElfenLand";
                currentSession.setGameMode("ElfenLand");
            } else {
                mode.text = "ElfenGold";
                currentSession.setGameMode("ElfenGold");
            }
        }
    }
}
