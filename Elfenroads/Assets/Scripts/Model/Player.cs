using System;
using Models.Helpers;
using Models;

namespace Models {
    public class Player : GuidModel, IUpdatable<Player> {
        public event EventHandler Updated
        public string name { private set; get; }
        public Boot boot { private set; get; }
        public Inventory inventory { private set; get; }
        public Town destinationTown; //Could be a string too I guess but Town is probably easier. For the 2nd variant, so can be null.

        public Player(string name, Color color, Guid bootId) {
            this.name = name;
            this.boot = new Boot(bootId, color);
            //ModelHelper.StoreInstance().addBoot(this.boot);
        }

        public bool Update(Player update) {
            bool modified = false;
        }
    }
}