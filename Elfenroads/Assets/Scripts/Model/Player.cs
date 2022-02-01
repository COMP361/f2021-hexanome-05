using System;
using Models;

namespace Models {
    public class Player {
        
        public string name { set; get; }
        public Boot boot { set; get; }
        public Inventory inventory;

        public Player(string name, Color color, Guid bootId) {
            this.name = name;
            this.boot = new Boot(bootId, color);
            //ModelHelper.StoreInstance().addBoot(this.boot);
        }
    }
}