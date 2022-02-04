using System;
using Models;

namespace Models {
    public class Player {
        
        public string name { set; get; }
        public Boot boot { set; get; }
        public Inventory inventory;
        public Town destinationTown; //Could be a string too I guess but Town is probably easier. For the 2nd variant, so can be null.

        public Player(string name, Color color, Guid bootId) {
            this.name = name;
            this.boot = new Boot(bootId, color);
            //ModelHelper.StoreInstance().addBoot(this.boot);
        }

        //*** This will need to be attached to a Unity GameObject with an appropriate ViewScript! ***
        //Needs an "Update" function.
    }
}