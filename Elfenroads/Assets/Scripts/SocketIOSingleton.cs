using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketIOClient;

public class SocketIOSingleton : MonoBehaviour
{
    public string address;
    private SocketIO _instance;
    public SocketIO Instance {
        private set {}
        get {
            if (_instance == null) {
                _instance = new SocketIO(address);
                _instance.ConnectAsync();
            }
            return _instance;
        }
    }

    void Start() {
        DontDestroyOnLoad(this);
    }

    private void OnDestroy() {
        if (_instance == null) {
            return;
        }
        _instance.DisconnectAsync();
    }
}
