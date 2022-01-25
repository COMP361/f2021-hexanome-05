using System;
using Models;
using Views;
using Controls;


namespace Views {
    public interface INotifyRoadSelected {
        event EventHandler<Road> RoadSelected;
    }


    public class ElfenroadsView : Elfenroads, INotifyRoadSelected {
        public event EventHandler<Road> RoadSelected;

        private void Awake() {
            Elfenroads.View = this;
        }
    }
}