using System.Collections.Generic;
using UnityEngine;
using Models;

namespace Models {
    public class TownPiece {
        public readonly int id;
        public Color color { private set; get; }

        public TownPiece(int id, Color color) {
            this.id = id;
            this.color = color;
        }
    }
}