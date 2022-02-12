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

    public GameObject dragonCounterPrefab;
    public GameObject trollCounterPrefab;
    public GameObject cloudCounterPrefab;
    public GameObject cycleCounterPrefab;
    public GameObject unicornCounterPrefab;
    public GameObject pigCounterPrefab;
    public GameObject landObstaclePrefab;

    //private List<Slot> counterSlots;
    private Slots counters;
    private Road modelRoad;


    void Start()
    {
        //First, create the Counter slots depending on whether or not they should be arranged horizontally or vertically.
        // Vector3 initialSlot = gameObject.transform.position;
        // if(horizontal){
        //     initialSlot.x -= 0.8f;
        // }else{
        //     initialSlot.y += 0.8f;
        // }
        // counterSlots = new List<Slot>();
        // for(int i = 0 ; i < 3 ; i++){
        //     if(horizontal){
        //         counterSlots.Add(new Slot(initialSlot + new Vector3(0.9f * i,0f, 0f)));
        //         //Instantiate(counterPrefab, initialSlot + new Vector3(0.9f * i,0f, 0f), Quaternion.identity);   //Remove later, just here now to help discern where the "slots" are.
        //     }else{
        //         counterSlots.Add(new Slot(initialSlot + new Vector3(0f, -0.9f * i, 0f)));
        //         //Instantiate(counterPrefab, initialSlot + new Vector3(0f, -0.9f * i, 0f), Quaternion.identity);   //Remove later, just here now to help discern where the "slots" are.
        //     }
        // }

        counters = null; //Double-check this thing. ***
        if(horizontal){
            counters = new Slots(3,3, gameObject.transform.position, 0.9f, 0f);
        }else{
            counters = new Slots(3, 1, gameObject.transform.position, 0f, 0.9f);
        }

        // Elfenroads.Model.ModelReady += getAndSubscribeToModel;
    }


     public void setAndSubscribeToModel(Road r){
         modelRoad = r;
         modelRoad.Updated += onModelUpdated;
         //this.onModelUpdated(null, null);  *** COMMENTED OUT UNTIL I FIX "SLOTS"
     }

    void onModelUpdated(object sender, EventArgs e) {
        Debug.Log("model updated!");
        //First, remove all counters from the slots.
        counters.removeAllFromSlots();

        //Then, go through each counter of the model, and add the right prefab to the slots.
        foreach(Counter c in modelRoad.counters){
            switch(c){
                case TransportationCounter tc:
                {
                    switch(tc.transportType){
                        case TransportType.Dragon:
                        {
                            counters.addToSlot(dragonCounterPrefab, this.gameObject);
                            break;
                        }
                        case TransportType.ElfCycle:
                        {
                            counters.addToSlot(cycleCounterPrefab, this.gameObject);
                            break;
                        }
                        case TransportType.MagicCloud:
                        {
                            counters.addToSlot(cloudCounterPrefab, this.gameObject);
                            break;
                        }
                        case TransportType.TrollWagon:
                        {
                            counters.addToSlot(trollCounterPrefab, this.gameObject);
                            break;
                        }
                        case TransportType.GiantPig:
                        {
                            counters.addToSlot(pigCounterPrefab, this.gameObject);
                            break;
                        }
                        case TransportType.Unicorn:
                        {
                            counters.addToSlot(unicornCounterPrefab, this.gameObject);
                            break;
                        }
                        default: Debug.Log("Model transportation counter of type raft! This is not allowed!") ; break;
                    }
                    break;
                }
                case MagicSpellCounter msc:
                {
                    Debug.Log("Elfengold - Do later");
                    break;
                }
                case GoldCounter gc:
                {
                    Debug.Log("Elfengold - Do later");
                    break;
                }
                case ObstacleCounter obc:
                {
                    //*** Add sea obstacle later, during elfengold.
                    counters.addToSlot(landObstaclePrefab, this.gameObject);
                    break;
                }
                default: Debug.Log("Counter is of undefined type!") ; break;
            }
        }
    }

    public void OnClick() {
        RoadClicked?.Invoke(modelRoad, EventArgs.Empty);
    }

    // Change parameters later. Either takes in a "counterType" parameter and creates the counter witihn this function via a prefab, or it takes in a prefab that was instantiated elsewhere in which case signature is the same.
    // Regardless, a gameObject is added to the slot and its position is updated to match it.
    // public void addToSlot(GameObject obj){

    //     foreach(Slot s in counterSlots){
    //         if(s.obj == null){
    //             s.obj = obj;
    //             obj.transform.position = (new Vector3(s.xCoord, s.yCoord, gameObject.transform.position.z + 0.5f));
    //             return;
    //         }else{
    //             Debug.Log("No available slot!"); 
    //         }
    //     }
    // }

    // //Removes a "counter" gameObject from the road, and also destroys it. Will maybe see use in Elfengold for the "Exchange" spell?
    // public void removeFromSlot(GameObject obj){ // Change parameters later. Likely will take in a "counterType" parameter, and will remove the first counter of that type.
    //     foreach(Slot s in counterSlots){
    //         if(s.obj == obj){
    //             s.obj = null;
    //             Destroy(obj); // Destroy only the visible GameObject. This will not affect its model counterpart, obviously.
    //             return;
    //         }else{
    //             Debug.Log("Nothing to remove!");
    //         }
    //     }
    // }

    // public void removeAllFromSlots(){
    //     foreach (Slot s in counterSlots){
    //         if(s.obj != null){
    //             Destroy(s.obj);
    //             s.obj = null;
    //         }
    //     }
    // }
}
