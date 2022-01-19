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
            //If we  made it here, then a road was clicked.
            Road modelRoad = (Road) sender;
            //We need to get the two towns attached to this model road. If one town has the currentPlayer's boot on it, move to the other. Otherwise, invalid.
            Debug.Log(modelRoad.id);

            //NOTE: INVALID - just did this the "quick and dirty" way for testing. This controller should NEVER directly update the model like this.
            if(modelRoad.start.boots.Count > 0){
                //Move to end town.
                Town dummyTown1 = new Town("dummyTown1");
                Town dummyTown2 = new Town("dummyTown2");
                dummyTown1.boots.Add(ModelHelper.StoreInstance().getBoot(0));
                modelRoad.start.Update(dummyTown2);
                modelRoad.end.Update(dummyTown1);

            }else if(modelRoad.end.boots.Count > 0){
                //Move to start town.
                Town dummyTown1 = new Town("dummyTown1");
                Town dummyTown2 = new Town("dummyTown2");
                dummyTown1.boots.Add(ModelHelper.StoreInstance().getBoot(0));
                modelRoad.start.Update(dummyTown1);
                modelRoad.end.Update(dummyTown2);
            }else{
                Debug.Log("Invalid move!");
            }

        }

        private void validate(Road road) {
            //We need to get the two towns attached to this road. If one town has the currentPlayer's boot on it, move to the other. Otherwise, invalid.
            
        }
    }
}