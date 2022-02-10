using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Models;

public class FaceUpCountersView : MonoBehaviour
{
    private Slots counters;
    private FaceUpCounters countersModel;

    public GameObject dragonCounterPrefab;
    public GameObject trollCounterPrefab;
    public GameObject cloudCounterPrefab;
    public GameObject cycleCounterPrefab;
    public GameObject unicornCounterPrefab;
    public GameObject pigCounterPrefab;

    void Start(){
        counters = new Slots(5,5,gameObject.transform.position, 0.15f , 0); //These are simply guesses, for now.
    }

    public void setAndSubscribeToModel(FaceUpCounters fUpCounters){
         countersModel = fUpCounters;
         countersModel.Updated += onModelUpdated;
         //this.onModelUpdated(null, null);  *** COMMENTED OUT FOR NOW
     }

     void onModelUpdated(object sender, EventArgs e) {
         counters.removeAllFromSlots();
         foreach(Counter c in countersModel.counters){
             switch(c){
                case TransportationCounter tc:
                {
                    switch(tc.transportType){
                        case TransportType.Dragon:
                        {
                            counters.addToSlot(dragonCounterPrefab);
                            break;
                        }
                        case TransportType.ElfCycle:
                        {
                            counters.addToSlot(cycleCounterPrefab);
                            break;
                        }
                        case TransportType.MagicCloud:
                        {
                            counters.addToSlot(cloudCounterPrefab);
                            break;
                        }
                        case TransportType.TrollWagon:
                        {
                            counters.addToSlot(trollCounterPrefab);
                            break;
                        }
                        case TransportType.GiantPig:
                        {
                            counters.addToSlot(pigCounterPrefab);
                            break;
                        }
                        case TransportType.Unicorn:
                        {
                            counters.addToSlot(unicornCounterPrefab);
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
                    //*** Add sea obstacle later, during elfengold. Land obstacles can't ever be in FaceUpCounters, however.
                    break;
                }
                default: Debug.Log("Counter is of undefined type!") ; break;
            }
        }
    }
}
