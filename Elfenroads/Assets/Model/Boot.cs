using System.Collections.Generic;
using Models;
using System;


namespace Models {
    public class Boot : IUpdatable<Boot>, INotifyModelUpdated {
        public event EventHandler ModelUpdated;
        public Color color { private set; get; }
        public Town currentTown { private set; get; }

        public Boot(Color color, Town startingTown) {
            this.color = color;
            this.currentTown = startingTown;
        }

        public void update(Boot boot) {
            if (this.color == boot.color) {
                this.currentTown = boot.currentTown;
            }
            ModelUpdated?.Invoke(this, EventArgs.Empty);
        }
    }
}
