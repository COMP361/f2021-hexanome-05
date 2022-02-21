using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Models;
public class ThisPlayerInventoryView : MonoBehaviour
{
    private Player playerModel;
    public GameObject dragonCounterPrefab;
    public GameObject trollCounterPrefab;
    public GameObject cloudCounterPrefab;
    public GameObject cycleCounterPrefab;
    public GameObject unicornCounterPrefab;
    public GameObject pigCounterPrefab;
    public GameObject landObstaclePrefab;

    public void setAndSubscribeToModel(Player inputPlayer){
         playerModel = inputPlayer;
         playerModel.Updated += onModelUpdated;
         //onModelUpdated(null, null);
     }

    void onModelUpdated(object sender, EventArgs e) {
        //Here, needs to add counters to the GridLayoutGroup according to the model. Instantiated Counters must also have their "CounterViewHelper" component's "Guid" fields set appropriately.

        //First, get the CounterGridLayoutGroup from this object. 

        GameObject countersLayoutGroup = GameObject.Find("PlayerCounters");

        //Next, destroy all children (mwahahahaaa)
        while(countersLayoutGroup.transform.childCount > 0){
            Destroy(countersLayoutGroup.transform.GetChild(0));
        }

        //Then, assign appropriate counters as in RoadView.
         foreach(Counter c in playerModel.inventory.counters){
            switch(c){
                case TransportationCounter tc:
                {
                    switch(tc.transportType){
                        case TransportType.Dragon:
                        {  
                           GameObject instantiatedCounter = Instantiate(dragonCounterPrefab, countersLayoutGroup.transform);
                            instantiatedCounter.GetComponent<CounterViewHelper>().setGuid(c.id);
                            break;
                        }
                        case TransportType.ElfCycle:
                        {
                            GameObject instantiatedCounter = Instantiate(cycleCounterPrefab, countersLayoutGroup.transform);
                            instantiatedCounter.GetComponent<CounterViewHelper>().setGuid(c.id);
                            break;
                        }
                        case TransportType.MagicCloud:
                        {
                            GameObject instantiatedCounter = Instantiate(cloudCounterPrefab, countersLayoutGroup.transform);
                            instantiatedCounter.GetComponent<CounterViewHelper>().setGuid(c.id);
                            break;
                        }
                        case TransportType.TrollWagon:
                        {
                            GameObject instantiatedCounter = Instantiate(trollCounterPrefab, countersLayoutGroup.transform);
                         instantiatedCounter.GetComponent<CounterViewHelper>().setGuid(c.id);
                            break;
                        }
                        case TransportType.GiantPig:
                        {
                            GameObject instantiatedCounter = Instantiate(pigCounterPrefab, countersLayoutGroup.transform);
                            instantiatedCounter.GetComponent<CounterViewHelper>().setGuid(c.id);
                            break;
                        }
                        case TransportType.Unicorn:
                        {
                            GameObject instantiatedCounter = Instantiate(unicornCounterPrefab, countersLayoutGroup.transform);
                            instantiatedCounter.GetComponent<CounterViewHelper>().setGuid(c.id);
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
                    GameObject instantiatedCounter = Instantiate(landObstaclePrefab, countersLayoutGroup.transform);
                    instantiatedCounter.GetComponent<CounterViewHelper>().setGuid(c.id);
                    break;
                }
                default: Debug.Log("Counter is of undefined type!") ; break;
            }
        }


        //LATER: Need to get the object which represents the amount of points gained + card GridLayoutGroup ***
    }
}
