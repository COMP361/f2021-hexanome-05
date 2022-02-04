using Models;
using System.Collections.Generic;


namespace Models {
    public class Inventory
    {
        public List<TownPiece> townPieces;
        public List<Card> cards;
        public List<Counter> counters;
        public int gold;


        public Inventory(IEnumerable<TownPiece> townPieces)
        {
            this.townPieces = new List<TownPiece>(townPieces);
            this.cards = new List<Card>();
            this.counters = new List<Counter>();
            this.gold = 0;
        }

        //*** This will need to be attached to a Unity GameObject with an appropriate ViewScript! ***
        //Needs an "Update" function.
    }
}