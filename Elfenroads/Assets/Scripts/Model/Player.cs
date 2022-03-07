using System;
using Models.Helpers;
using Models;

namespace Models {
    public class Player : GuidModel, IUpdatable<Player> {
        public event EventHandler Updated;
        public string name { protected set; get; }
        public Boot boot { protected set; get; }
        public Inventory inventory { protected set; get; }

        //Could be a string too I guess but Town is probably easier. For the 2nd variant, so can be null.
        public Town destinationTown { protected set; get; } // <---- needs discussion

        public Player(string name, Color color) : base() {
            this.name = name;
            this.boot = new Boot(color);
            this.inventory = new Inventory();
        }

        [Newtonsoft.Json.JsonConstructor]
        protected Player(string name, Boot boot, Inventory inventory, Town destinationTown, Guid id) : base(id) {
            this.name = name;
            this.boot = boot;
            this.inventory = inventory;
            this.destinationTown = destinationTown;
        }

        public bool Update(Player update) {
            bool modified = false;

            if ( !boot.Equals(update.boot) ) {
                boot = (Boot) ModelStore.Get(update.boot.id);
                modified = true;
            }

            
            // if ( !destinationTown.Equals(update.destinationTown)) {
            //     destinationTown = (Town) ModelStore.Get(update.destinationTown.id);
            //     modified = true;
            // }

            if (inventory.Update(update.inventory)) {
                modified = true;
            }

            if (modified) {
                Updated?.Invoke(this, EventArgs.Empty);
            }

            return modified;
        }
    }
}