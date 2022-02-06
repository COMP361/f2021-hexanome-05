using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Models;


public class RoadView : MonoBehaviour {
    public TerrainType roadType;
    public GameObject startTown;
    public GameObject endTown;

    public event EventHandler RoadClicked;
    public bool horizontal = true;
    public GameObject counterPrefab;

    private List<Slot> counterSlots;
    private Road modelRoad;


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

        // Elfenroads.Model.ModelReady += getAndSubscribeToModel;
    }


    // public void getAndSubscribeToModel(object sender, EventArgs e){
    //     this.modelRoad = (Road) ModelStore.Get(new Guid(id));
    //     modelRoad.Updated += onModelUpdated;
    //     //Debug.Log("Road " + id + " subscribed!");
    // }

    void onModelUpdated(object sender, EventArgs e) {
        // reflect changes
    }

    public void OnClick() {
        RoadClicked?.Invoke(modelRoad, EventArgs.Empty);
    }

    // Change parameters later. Either takes in a "counterType" parameter and creates the counter witihn this function via a prefab, or it takes in a prefab that was instantiated elsewhere in which case signature is the same.
    // Regardless, a gameObject is added to the slot and its position is updated to match it.
    public void addToSlot(GameObject obj){

        foreach(Slot s in counterSlots){
            if(s.obj == null){
                s.obj = obj;
                obj.transform.position = (new Vector3(s.xCoord, s.yCoord, gameObject.transform.position.z + 0.5f));
                return;
            }else{
                Debug.Log("No available slot!"); 
            }
        }
    }

    //Removes a "counter" gameObject from the road, and also destroys it.
    public void removeFromSlot(GameObject obj){ // Change parameters later. Likely will take in a "counterType" parameter, and will remove the first counter of that type.
        foreach(Slot s in counterSlots){
            if(s.obj == obj){
                s.obj = null;
                Destroy(obj); // Destroy only the visible GameObject. This will not affect its model counterpart, obviously.
                return;
            }else{
                Debug.Log("Nothing to remove!");
            }
        }
    }
}
