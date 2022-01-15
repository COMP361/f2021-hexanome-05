using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Views;
using Models;
using System;


namespace Controls
{
    public class MoveBootControl : MonoBehaviour
    {
        public List<GameObject> roadObjects;
        private List<RoadView> roadViews;
        void Start() {
            roadViews = new List<RoadView>();
            foreach (GameObject road in roadObjects) {
                roadViews.Add(road.GetComponent<RoadView>());
            }
        }

        private void OnEnable() {
            
        }

        private void subscribeToRoadClickEvents() {
            foreach (RoadView roadView in roadViews) {
                roadView.RoadClicked += onRoadClicked;
            }
        }

        private void onRoadClicked(object sender, EventArgs args) {
            
        }

        // private void validate(Road road) {
        //     if (road in )
        // }
    }
}