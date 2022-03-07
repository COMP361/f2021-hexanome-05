using System.Collections.Generic;
using System;
using Models;
using Models.Helpers;
using UnityEngine;

namespace Models {
    public class Town : GuidModel, IUpdatable<Town> {
        public event EventHandler Updated;
        public string name { protected set; get; }
        public List<TownPiece> townPieces { protected set; get; }
        public List<Boot> boots { protected set; get; }
        public int goldValue; // Elfengold

        public Town(string name) : base() {
            this.name = name;
            this.townPieces = new List<TownPiece>();
            this.boots = new List<Boot>();
        }

        [Newtonsoft.Json.JsonConstructor]
        protected Town(string name, List<TownPiece> townPieces, List<Boot> boots, Guid id) : base(id) {
            this.name = name;
            this.townPieces = new List<TownPiece>(townPieces);
            this.boots = new List<Boot>(boots);
        }

        public bool Update(Town update) {
            bool modified = false;

            if (townPieces.Update(update.townPieces)) {
                modified = true;
            }

            if (boots.Update(update.boots)) {
                modified = true;
            }
            
            if (modified) {
                Updated?.Invoke(this, EventArgs.Empty);
            }

            foreach(Boot b in boots){
                Debug.Log("In model, for town " + name + " with id " + this.id + " we have boot: " + b.color);
            }

            return modified;
        }
    }
}