using System;
using Models;

namespace Models {
    public interface INotifyModelUpdated {
        event EventHandler ModelUpdated;
    }

    public class ElfenroadsModel : Elfenroads, INotifyModelUpdated {
        public event EventHandler ModelUpdated;
        private Game game;

        private void Awake() {
            game = new Game();
            Elfenroads.Model = this;
        }
    }
}