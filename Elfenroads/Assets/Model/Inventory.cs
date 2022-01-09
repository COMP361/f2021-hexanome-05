
// Model/Class for games


using Models;
using System.Collections.Generic;

namespace Model
{
    public class Inventory
    {

        public List<TownPiece> TownPieces { get; set; }

        public Inventory(List<TownPiece> townpieces)
        {
            this.TownPieces = townpieces;
        }
    }
}