using System.Collections.Generic;
using System;
using Models;

namespace Models {
    public class TownPiece : GuidModel {
        public Color color { protected set; get; }

        [Newtonsoft.Json.JsonConstructor]
        protected TownPiece(Color color, Guid id) : base(id) {
            this.color = color;
        }
    }
}