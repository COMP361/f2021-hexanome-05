using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;


namespace Models {
    public class Player {
        public string name { private set; get; }
        public Color color { private set; get; }
        public Town currentTown { private set; get; }

        private Inventory inventory;

        public Player(string name, Color color, Town currentTown) {
            this.name = name;
            this.color = color;
            this.currentTown = currentTown;
        }
    }
}