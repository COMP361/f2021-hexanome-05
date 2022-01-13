using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;

namespace Views {
    public class RoadView : MonoBehaviour
    {
        private Road road;
        void Start() {
            Elfenroads.Model.ModelUpdated += onModelUpdated;
        }

        void onModelUpdated(object sender, EventArgs e) {
            
        }
    }
}