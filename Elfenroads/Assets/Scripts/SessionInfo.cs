using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SessionInfo : MonoBehaviour
{
    private Client thisClient;

    void Start(){
        DontDestroyOnLoad(this.gameObject);

    }

    // Start is called before the first frame update
    public void setClient(){
        thisClient = Client.Instance();
        Debug.Log("SessionInfo object ID is now " + thisClient.thisSessionID);
    }

    public Client getClient(){
        return thisClient;
    }

    public string getSessionID(){
        return thisClient.thisSessionID;
    }

}
