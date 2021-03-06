using Models;
using System.Collections.Generic;
using System;
using Models.Helpers;


namespace Models {
    public class Inventory : IUpdatable<Inventory>
    {
        public event EventHandler Updated;
        public List<TownPiece> townPieces { protected set; get; }
        public List<Card> cards { protected set; get; }
        public List<Counter> counters { protected set; get; }
        public int gold; // Elfengold

        public Inventory()
        {
            this.townPieces = new List<TownPiece>();
            this.cards = new List<Card>();
            this.counters = new List<Counter>();
            this.gold = 0;
        }

        [Newtonsoft.Json.JsonConstructor]
        protected Inventory(List<TownPiece> townPieces, List<Card> cards, List<Counter> counters, int gold) {
            this.townPieces = townPieces;
            this.cards = cards;
            this.counters = counters;
            this.gold = gold;
        }

        public bool Update(Inventory update) {
            bool modified = false;

            if (townPieces.Update(update.townPieces)) {
                modified = true;
            }
            if (cards.Update(update.cards)) {
                modified = true;
            }
            if (counters.Update(update.counters)) {
                modified = true;
            }

            if(gold != update.gold){
                gold = update.gold;
                modified = true;
            }

            if (modified) {
                Updated?.Invoke(this, EventArgs.Empty);
            }

            return modified;
        }
    }
}