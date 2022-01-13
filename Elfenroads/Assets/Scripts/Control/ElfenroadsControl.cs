using System;
using UnityEngine;
using SocketIOClient;
using Models;
using Views;


namespace Controls {

    /// <summary>
    /// Manages user interactions and server communication.
    /// </summary>
    public class ElfenroadsControl : Elfenroads {
        private SocketIO socket;

        private void Awake() {
            Elfenroads.Control = this;
        }

        private void Start() {
            // socket = GameObject.Find("SocketIO").GetComponent<SocketIOSingleton>().Instance;

            // enable game updates
            // socket.On("UpdateState", updateState);
        }

        private void moveBootDemo(Town town) {

            // we should implement town.serialize() and use it as the parameter
            socket.EmitAsync("MoveBootDemo", "serialized town");
        }
    }
}