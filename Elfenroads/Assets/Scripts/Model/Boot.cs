using System.Collections.Generic;
using Models;
using System;


namespace Models {
    public class Boot {
        public Color color {get; set; }
        public Guid id {get; set;}

        public Boot(Guid id, Color color) {
            this.color = color;
            this.id = id;
            
        }
    }
}
