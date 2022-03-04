using System;
using Models;
using Views;
using Controls;


namespace Views {
    public class ElfenroadsView : Elfenroads {

        private void Awake() {
            Elfenroads.View = this;
        }
    }
}