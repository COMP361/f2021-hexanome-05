using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class SocketTest : MonoBehaviour
{
    static WebSocket ws;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("ws initialized");
        ws = new WebSocket("ws://localhost:1400");
        ws.Connect();
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Message Received from" + ((WebSocket)sender).Url + ", Data :" + e.Data);
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (ws == null){
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            ws.Send("join");
            Debug.Log("sent join message");
        }  
    }
}



