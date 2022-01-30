using System.Collections.Generic;
using System;
using UnityEngine;
using Models;

namespace Models {
    public class TownPiece : IEquatable<TownPiece> {
        public int id;
        public Color color { private set; get; }

        public TownPiece(int id, Color color) {
            this.id = id;
            this.color = color;
        }

        public bool Equals(TownPiece other) {
            return other != null &&
                    other.id == this.id;
        }

        public override bool Equals(object obj)
        {   
            return Equals(obj as TownPiece);
        }

        public override int GetHashCode()
        {
            return id;
        }
    }
}