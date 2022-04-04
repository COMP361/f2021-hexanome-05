using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Views;
using Models;

public class SelectCounterController : MonoBehaviour, GuidHelperContainer
{
    public GameObject countersLayout;
    public GameObject mainWindow;
    public GameObject waitingWindow;

    public GameObject dragonCounterPrefab;
    public GameObject trollCounterPrefab;
    public GameObject cloudCounterPrefab;
    public GameObject cycleCounterPrefab;
    public GameObject unicornCounterPrefab;
    public GameObject pigCounterPrefab;
    public GameObject landObstaclePrefab;
    public GameObject seaObstaclePrefab;
    public GameObject goldCounterPrefab;
    public GameObject exchangeCounterPrefab;
    public GameObject doubleCounterPrefab;

    public void setupSelectCounter(SelectCounter sc){
        if(! Elfenroads.Control.isCurrentPlayer()){
            waitingWindow.SetActive(true);
            waitingWindow.transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = "Waiting for " + Elfenroads.Control.currentPlayer.name + " to make a selection...";
            mainWindow.SetActive(false);
        }else{
            waitingWindow.SetActive(false);
            mainWindow.SetActive(true);
        
            

            //Then, create the prefabs for the two counters here.
            foreach(Counter c in sc.counters){
                switch(c){
                    case TransportationCounter tc:
                    {
                        switch(tc.transportType){
                            case TransportType.Dragon:
                            {  
                            GameObject instantiatedCounter = Instantiate(dragonCounterPrefab, countersLayout.transform);
                                instantiatedCounter.GetComponent<GuidViewHelper>().setGuid(c.id);
                                instantiatedCounter.GetComponent<GuidViewHelper>().setContainer(this);
                                break;
                            }
                            case TransportType.ElfCycle:
                            {
                                GameObject instantiatedCounter = Instantiate(cycleCounterPrefab, countersLayout.transform);
                                instantiatedCounter.GetComponent<GuidViewHelper>().setGuid(c.id);
                                instantiatedCounter.GetComponent<GuidViewHelper>().setContainer(this);
                                break;
                            }
                            case TransportType.MagicCloud:
                            {
                                GameObject instantiatedCounter = Instantiate(cloudCounterPrefab, countersLayout.transform);
                                instantiatedCounter.GetComponent<GuidViewHelper>().setGuid(c.id);
                                instantiatedCounter.GetComponent<GuidViewHelper>().setContainer(this);
                                break;
                            }
                            case TransportType.TrollWagon:
                            {
                                GameObject instantiatedCounter = Instantiate(trollCounterPrefab, countersLayout.transform);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setGuid(c.id);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setContainer(this);
                                break;
                            }
                            case TransportType.GiantPig:
                            {
                                GameObject instantiatedCounter = Instantiate(pigCounterPrefab, countersLayout.transform);
                                instantiatedCounter.GetComponent<GuidViewHelper>().setGuid(c.id);
                                instantiatedCounter.GetComponent<GuidViewHelper>().setContainer(this);
                                break;
                            }
                            case TransportType.Unicorn:
                            {
                                GameObject instantiatedCounter = Instantiate(unicornCounterPrefab, countersLayout.transform);
                                instantiatedCounter.GetComponent<GuidViewHelper>().setGuid(c.id);
                                instantiatedCounter.GetComponent<GuidViewHelper>().setContainer(this);
                                break;
                            }
                            default: Debug.Log("Model transportation counter of type raft! This is not allowed!") ; break;
                        }
                        break;
                    }
                    case MagicSpellCounter msc:
                    {
                        if(msc.spellType == SpellType.Exchange){
                            GameObject instantiatedCounter = Instantiate(exchangeCounterPrefab, countersLayout.transform);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setGuid(c.id);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setContainer(this);
                        }else if(msc.spellType == SpellType.Double){
                            GameObject instantiatedCounter = Instantiate(doubleCounterPrefab, countersLayout.transform);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setGuid(c.id);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setContainer(this);
                        }
                        break;
                    }
                    case GoldCounter gc:
                    {
                        GameObject instantiatedCounter = Instantiate(goldCounterPrefab, countersLayout.transform);
                        instantiatedCounter.GetComponent<GuidViewHelper>().setGuid(c.id);
                        instantiatedCounter.GetComponent<GuidViewHelper>().setContainer(this);
                        break;
                    }
                    case ObstacleCounter obc:
                    {
                        if(obc.obstacleType == ObstacleType.Land){
                            GameObject instantiatedCounter = Instantiate(landObstaclePrefab, countersLayout.transform);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setGuid(c.id);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setContainer(this);
                        }else if(obc.obstacleType == ObstacleType.Sea){
                            GameObject instantiatedCounter = Instantiate(seaObstaclePrefab, countersLayout.transform);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setGuid(c.id);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setContainer(this);
                        }
                        break;
                    }
                    default: Debug.Log("Counter is of undefined type!") ; break;
                }
            }
        }


    }

    public void GUIClicked(GameObject counterClicked){
        //The counter was clicked! Just call the main controller along with the GUID of the counter which was clicked.
        //Elfenroads.Control.SelectCounter(counterClicked.GetComponent<GuidViewHelper>().getGuid());
        Elfenroads.Control.drawCounter(counterClicked);
    }
}
