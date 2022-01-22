using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionInfo : MonoBehaviour
{
    private string sessionID;

    void Start(){
        DontDestroyOnLoad(this.gameObject);
    }

    // Start is called before the first frame update
    public void setSessionID(){
        sessionID = Client.Instance().thisSessionID;
    }

    public string getSessionID(){
        return sessionID;
    }

}
