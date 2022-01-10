using System.Collections.Generic;
using UnityEngine;
using Models;

namespace Models {
    public class TownPiece {
        public Color color { private set; get; }

        public TownPiece(Color color) {
            this.color = color;
        }
    }
}