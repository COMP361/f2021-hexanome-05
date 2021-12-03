using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firesplash.UnityAssets.SocketIO;

public class ListenerScript : MonoBehaviour
{

    public SocketIOCommunicator sioCOM;

    void OnEnable()
    {
        //sioCOM.Instance.Emit("join","game1");
        Debug.Log(sioCOM.Instance.IsConnected());
        sioCOM.Instance.Emit("join","HelloMax",true);
    }

    private void callback(string payloadData){
        Debug.Log(payloadData);
    }

}
