
// Model/Class for games


using System.Collections.Generic;
namespace Models
{
    public class TownPiece
    {

        private Town Town { get; set; }
        private BootColor Color { get; set; }


        TownPiece(Town town, BootColor color)
        {
            this.Town = town;
            this.Color = color;

        }
    }
}