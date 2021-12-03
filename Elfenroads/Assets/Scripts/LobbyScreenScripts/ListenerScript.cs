using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firesplash.UnityAssets.SocketIO;

public class ListenerScript : MonoBehaviour
{

    public SocketIOCommunicator sioCOM;

    // Start is called before the first frame update
    void Start()
    {
        //sioCOM.Instance.Emit("join","game1");
        Debug.Log(sioCOM.Instance.IsConnected());
        
        sioCOM.Instance.On("StartGame", callback);
    }

    private void callback(string payloadData){
        Debug.Log(payloadData);
    }

}
