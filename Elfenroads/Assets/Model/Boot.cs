using System.Collections.Generic;
using Models;


namespace Models {
    public class Boot {
        public Color color { private set; get; }
        public Town currentTown { private set; get; }

        public Boot(Color color, Town startingTown) {
            this.color = color;
            this.currentTown = startingTown;
        }
    }
}
