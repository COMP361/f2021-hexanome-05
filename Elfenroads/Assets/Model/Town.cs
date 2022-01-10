
// Model/Class for games


using System.Collections.Generic;
namespace Models
{
    public class Town
    {

        private string TownName { get; set; }
        private List<TownPiece> TownPieces { get; set; }
        private int GoldValue;


        Town(string townName, List<TownPiece> townPieces, int goldValue)
        {
            this.TownName = townName;
            this.TownPieces = townPieces;
            this.GoldValue = goldValue;
        }
    }
}