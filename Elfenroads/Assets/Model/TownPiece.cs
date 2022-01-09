
// Model/Class for games


using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    public class TownPiece : GameUnit
    {

        private Town Town { get; set; }
        private BootColor Color { get; set; }



        TownPiece(Town town, BootColor color, GameObject gameObject) : base(gameObject)
        {
            this.Town = town;
            this.Color = color;

        }
    }
}