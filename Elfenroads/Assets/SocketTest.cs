using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class SocketTest : MonoBehaviour
{
    WebSocket ws;
    // Start is called before the first frame update
    void Start()
    {
        ws = new WebSocket("http://fierce-plateau-19887.herokuapp.com/");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
