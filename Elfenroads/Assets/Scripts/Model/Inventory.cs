using Models;
using System.Collections.Generic;


namespace Models {
    public class Inventory
    {
        private List<TownPiece> townPieces;

        public Inventory(IEnumerable<TownPiece> townPieces)
        {
            this.townPieces = new List<TownPiece>(townPieces);
        }
    }
}