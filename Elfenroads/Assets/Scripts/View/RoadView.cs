using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;

namespace Views {
    public class RoadView : MonoBehaviour
    {
        public event EventHandler RoadClicked;
        public bool horizontal = true;
        public GameObject counterPrefab;

        private List<Slot> counterSlots;
        private Road road;


        void Start()
    {
        //First, create the Counter slots depending on whether or not they should be arranged horizontally or vertically.
        Vector3 initialSlot = gameObject.transform.position;
        if(horizontal){
            initialSlot.x -= 0.8f;
        }else{
            initialSlot.y += 0.8f;
        }
        counterSlots = new List<Slot>();
        for(int i = 0 ; i < 3 ; i++){
            if(horizontal){
                counterSlots.Add(new Slot(initialSlot + new Vector3(0.9f * i,0f, 0f)));
                Instantiate(counterPrefab, initialSlot + new Vector3(0.9f * i,0f, 0f), Quaternion.identity);   //Remove later, just here now to help discern where the "slots" are.
            }else{
                counterSlots.Add(new Slot(initialSlot + new Vector3(0f, -0.9f * i, 0f)));
                Instantiate(counterPrefab, initialSlot + new Vector3(0f, -0.9f * i, 0f), Quaternion.identity);   //Remove later, just here now to help discern where the "slots" are.
            }
        }
    }


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