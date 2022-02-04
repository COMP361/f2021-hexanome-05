using System.Collections.Generic;
using System;
using Models;
using Newtonsoft.Json;


namespace Models {
    public class Town : INotifyModelUpdated, IUpdatable<Town> {
        public event EventHandler ModelUpdated;
        public string name { private set; get; }
        public List<TownPiece> townPieces { private set; get; }
        public List<Boot> boots;
        public int goldValue; //For Elfengold.

        //*** This will need to be attached to a Unity GameObject with an appropriate ViewScript! ***
        //May need tweaks to its "Update" function.

        public Town(string name) {

            this.name = name;
            this.townPieces = new List<TownPiece>();
            this.boots = new List<Boot>();
        }

        [JsonConstructor]
        private Town(string name, List<TownPiece> townPieces) {
            this.name = name;
            this.townPieces = new List<TownPiece>(townPieces);
        }

        public void Update(Town town) {
            bool modified = false;

            HashSet<TownPiece> removedItems = new HashSet<TownPiece>(this.townPieces);
            removedItems.ExceptWith(town.townPieces); // original - new (the things that were taken out)
            foreach (TownPiece townPiece in removedItems) {
                this.townPieces.Remove(ModelHelper.StoreInstance().getTownPiece(townPiece.id));
                modified = true;
            }

            HashSet<TownPiece> addedItems = new HashSet<TownPiece>(town.townPieces);
            addedItems.ExceptWith(this.townPieces); // new - original (the things that were added in)
            foreach (TownPiece townPiece in addedItems) {
                this.townPieces.Add(ModelHelper.StoreInstance().getTownPiece(townPiece.id));
                modified = true;
            }

            HashSet<Boot> removedBoots = new HashSet<Boot>(this.boots);
            removedItems.ExceptWith(town.townPieces); // original - new (the things that were taken out)
            foreach (Boot boot in removedBoots) {
                this.boots.Remove(ModelHelper.StoreInstance().getBoot(boot.id));
                modified = true;
            }

            HashSet<Boot> addedBoots = new HashSet<Boot>(town.boots);
            addedItems.ExceptWith(this.townPieces); // new - original (the things that were added in)
            foreach (Boot boot in addedBoots) {
                this.boots.Add(ModelHelper.StoreInstance().getBoot(boot.id));
                modified = true;
            }
            
            if (modified) {
                ModelUpdated?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}