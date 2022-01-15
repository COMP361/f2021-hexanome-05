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
            subscribeToRoadClickEvents();
        }

        private void OnEnable() {
            
        }

        private void subscribeToRoadClickEvents() {
            foreach (RoadView roadView in roadViews) {
                roadView.RoadClicked += onRoadClicked;
            }
        }

        private void onRoadClicked(object sender, EventArgs args) {
            //If we  made it here, then a road was clicked. We need to get the currentPlayer from the model, get the town the currentPlayer is on, and check if it is connected to this road.

            //If so, send the appropriate command to the Server.
        }

        // private void validate(Road road) {
        //     if (road in )
        // }
    }
}