using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using Models;


public class RoadView : MonoBehaviour {
    public TerrainType roadType;
    public GameObject startTown;
    public GameObject endTown;
    public GameObject PlanTravelController;
    public GameObject MoveBootsController;

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

    void removeAllFromSlots(Slots target){ //Needs to be added to all "Views" using slots, since Unity is goofy like that and doesn't allow deletion if it isn't in a Monobehavior.
        List<Slot> tSlots = target.getSlots();
        foreach(Slot s in tSlots){
            if(s.obj != null){
                Destroy(s.obj);
                s.obj = null;
            }
        }
    }


     public void setAndSubscribeToModel(Road r){
         modelRoad = r;
         modelRoad.Updated += onModelUpdated;
         //this.onModelUpdated(null, null);  *** COMMENTED OUT UNTIL I FIX "SLOTS"
     }

    void onModelUpdated(object sender, EventArgs e) {
        Debug.Log("model updated!");
        //First, remove all counters from the slots.
        removeAllFromSlots(counters);

        //Then, go through each counter of the model, and add the right prefab to the slots.
        foreach(Counter c in modelRoad.counters){
            switch(c){
                case TransportationCounter tc:
                {
                    switch(tc.transportType){
                        case TransportType.Dragon:
                        {  
                            GameObject parameter = Instantiate(dragonCounterPrefab, Vector3.zero, Quaternion.identity);
                            counters.addToSlot(parameter, this.gameObject);
                            break;
                        }
                        case TransportType.ElfCycle:
                        {
                            GameObject parameter = Instantiate(cycleCounterPrefab, Vector3.zero, Quaternion.identity);
                            counters.addToSlot(parameter, this.gameObject);
                            break;
                        }
                        case TransportType.MagicCloud:
                        {
                            GameObject parameter = Instantiate(cloudCounterPrefab, Vector3.zero, Quaternion.identity);
                            counters.addToSlot(parameter, this.gameObject);
                            break;
                        }
                        case TransportType.TrollWagon:
                        {
                            GameObject parameter = Instantiate(trollCounterPrefab, Vector3.zero, Quaternion.identity);
                            counters.addToSlot(parameter, this.gameObject);
                            break;
                        }
                        case TransportType.GiantPig:
                        {
                            GameObject parameter = Instantiate(pigCounterPrefab, Vector3.zero, Quaternion.identity);
                            counters.addToSlot(parameter, this.gameObject);
                            break;
                        }
                        case TransportType.Unicorn:
                        {
                            GameObject parameter = Instantiate(unicornCounterPrefab, Vector3.zero, Quaternion.identity);
                            counters.addToSlot(parameter, this.gameObject);
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
                    GameObject parameter = Instantiate(landObstaclePrefab, Vector3.zero, Quaternion.identity);
                    counters.addToSlot(parameter, this.gameObject);
                    break;
                }
                default: Debug.Log("Counter is of undefined type!") ; break;
            }
        }
    }

    public void OnClick() {
        RoadClicked?.Invoke(modelRoad, EventArgs.Empty);
    }

    public void cardDragged(String cardType){
        //Inform the MoveBootController here.
        Debug.Log("CardDragged!");
    }

    public void counterDragged(String counterType){
        //Inform the PlanTravelController here.
        Debug.Log("CounterDragged!");
    }
}
