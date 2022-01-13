using System;
using Newtonsoft.Json;
using Models;

namespace Models {
    public interface INotifyModelUpdated {
        event EventHandler ModelUpdated;
    }

    public interface IUpdatable<T> {
        void update(T updated);
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