using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Models;
public class ThisPlayerInventoryView : MonoBehaviour
{
    private Player playerModel;
    public TMPro.TMP_Text numCloudCounters;
    public TMPro.TMP_Text numCycleCounters;
    public TMPro.TMP_Text numDragonCounters;
    public TMPro.TMP_Text numLandObstacles;
    public TMPro.TMP_Text numPigCounters;
    public TMPro.TMP_Text numTrollCounters;
    public TMPro.TMP_Text numUnicornCounters;

    public void setAndSubscribeToModel(Player inputPlayer){
         playerModel = inputPlayer;
         playerModel.Updated += onModelUpdated;
         onModelUpdated(null, null);
     }

    void onModelUpdated(object sender, EventArgs e) {
        //Here, needs to add counters to the GridLayoutGroup according to the model. Instantiated Counters must also have their "CounterViewHelper" component's "Guid" fields set appropriately.

        //First, get the CounterGridLayoutGroup from this object. 

        GameObject countersLayoutGroup = GameObject.Find("PlayerCounters");
        int dragonCounters = 0;
        int trollCounters = 0;
        int cloudCounters = 0;
        int cycleCounters = 0;
        int unicornCounters = 0;
        int pigCounters = 0;
        int landObstacles = 0;

        //Then, assign appropriate counters as in RoadView.
         foreach(Counter c in playerModel.inventory.counters){
            switch(c){
                case TransportationCounter tc:
                {
                    switch(tc.cardType){
                        case TransportType.Dragon:
                        {  
                           dragonCounters++;
                            break;
                        }
                        case TransportType.ElfCycle:
                        {
                            cycleCounters++;
                            break;
                        }
                        case TransportType.MagicCloud:
                        {
                            cloudCounters++;
                            break;
                        }
                        case TransportType.TrollWagon:
                        {
                            trollCounters++;
                            break;
                        }
                        case TransportType.GiantPig:
                        {
                            pigCounters++;
                            break;
                        }
                        case TransportType.Unicorn:
                        {
                            unicornCounters++;
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
                    landObstacles++;
                    break;
                }
                default: Debug.Log("Counter is of undefined type!") ; break;
            }
        }

        setAmount(numCloudCounters, cloudCounters);
        setAmount(numCycleCounters, cycleCounters);
        setAmount(numDragonCounters, dragonCounters);
        setAmount(numLandObstacles, landObstacles);
        setAmount(numPigCounters, pigCounters);
        setAmount(numTrollCounters, trollCounters);
        setAmount(numUnicornCounters, unicornCounters);
    
        //LATER: Need to get the object which represents the amount of points gained + card GridLayoutGroup ***
    }

    private static void setAmount(TMPro.TMP_Text text, int amount){
            text.text = amount.ToString() + "x";
        }
}
