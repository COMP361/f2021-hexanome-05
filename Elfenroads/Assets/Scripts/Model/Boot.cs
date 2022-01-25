using System.Collections.Generic;
using Models;
using System;


namespace Models {
    public class Boot {
        public Color color { private set; get; }
        public readonly int id;

        public Boot(int id, Color color) {
            this.color = color;
            this.id = id;
            
        }
    }
}
