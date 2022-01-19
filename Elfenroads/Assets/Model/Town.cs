using System.Collections.Generic;
using System;
using Models;
using Newtonsoft.Json;


namespace Models {
    public class Town : INotifyModelUpdated, IUpdatable<Town> {
        public event EventHandler ModelUpdated;
        public string name { private set; get; }
        public List<TownPiece> townPieces { private set; get; }
        private List<Boot> boots;

        // Only used in the elfengold extension, skip for now
        // private int goldValue;

        public Town(string name) {

            this.name = name;
            this.townPieces = new List<TownPiece>();
        }

        [JsonConstructor]
        private Town(string name, List<TownPiece> townPieces) {
            this.name = name;
            this.townPieces = new List<TownPiece>(townPieces);
        }

        // public void AddBoot(Boot boot) {
        //     if (boots.Contains(boot)) {
        //         throw new ArgumentException();
        //     }
        //     boots.Add(boot);
        //     ModelHelper.StoreInstance().getBoot(1);
        // }

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
            
            if (modified) {
                ModelUpdated?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}