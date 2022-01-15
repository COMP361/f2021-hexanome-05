using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;

namespace Views {
    public class RoadView : MonoBehaviour
    {
        public event EventHandler RoadClicked;

        private Road road;
        public void setAndSubscribeToModel(Road inputRoad){
            this.road = inputRoad;
            road.ModelUpdated += onModelUpdated;
        }

        void onModelUpdated(object sender, EventArgs e) {
            // reflect changes
        }

        void OnClick() {
            RoadClicked?.Invoke(road, EventArgs.Empty);
        }
    }
}