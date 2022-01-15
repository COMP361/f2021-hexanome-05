using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;

namespace Models {
    public class Player {
        
        public string name { private set; get; }
        public Boot boot { private set; get; }
        public Inventory inventory;

        public Player(string name, Color color, Town town) {
            this.name = name;
            this.boot = new Boot(color, town);
        }
    }
}