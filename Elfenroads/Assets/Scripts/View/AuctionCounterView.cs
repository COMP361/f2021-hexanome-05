using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Models;
using Views;
public class AuctionCounterView : MonoBehaviour
{   

    private AuctionController myController;
    private GameObject sessionInfo;

    public GameObject dragonCounterPrefab;
    public GameObject trollCounterPrefab;
    public GameObject cloudCounterPrefab;
    public GameObject cycleCounterPrefab;
    public GameObject unicornCounterPrefab;
    public GameObject pigCounterPrefab;
    public GameObject landObstaclePrefab;
    public GameObject seaObstaclePrefab;
    public GameObject goldCounterPrefab;
    public GameObject doubleCounterPrefab;
    public GameObject exchangeCounterPrefab;

    public RectTransform countersLeftLayoutGroup;


    // Start is called before the first frame update
    void Start()
    {
        myController = GameObject.Find("AuctionController").GetComponent<AuctionController>();
        sessionInfo = GameObject.Find("SessionInfo"); 
    }

    public void updateWaitingCounters(Auction auctionModel) { 
        Debug.Log("Model was updated!");
        //First, destroy all children
        foreach(Transform child in countersLeftLayoutGroup){
            child.SetParent(null);
            DestroyImmediate(child.gameObject);
        }
        countersLeftLayoutGroup.DetachChildren();

        //Now, loop through the cards of the model, instantiating appropriate counter each time.
        foreach(Counter c in auctionModel.countersForAuction){
            switch(c){
                case TransportationCounter tc:
                {
                    //Debug.Log("Transport type of " + c.id + " is: " + tc.transportType);
                    switch(tc.transportType){
                        case TransportType.Dragon:
                        {  
                           GameObject instantiatedCounter = Instantiate(dragonCounterPrefab, countersLeftLayoutGroup);
                            break;
                        }
                        case TransportType.ElfCycle:
                        {
                            GameObject instantiatedCounter = Instantiate(cycleCounterPrefab, countersLeftLayoutGroup);
                            break;
                        }
                        case TransportType.MagicCloud:
                        {
                            GameObject instantiatedCounter = Instantiate(cloudCounterPrefab, countersLeftLayoutGroup);
                            break;
                        }
                        case TransportType.TrollWagon:
                        {
                            GameObject instantiatedCounter = Instantiate(trollCounterPrefab, countersLeftLayoutGroup);
                            break;
                        }
                        case TransportType.GiantPig:
                        {
                            GameObject instantiatedCounter = Instantiate(pigCounterPrefab, countersLeftLayoutGroup);
                            break;
                        }
                        case TransportType.Unicorn:
                        {
                            GameObject instantiatedCounter = Instantiate(unicornCounterPrefab, countersLeftLayoutGroup);
                            break;
                        }
                        
                        default: Debug.Log("Model transportation counter of type raft! This is not allowed!") ; break;
                    }
                    break;
                }
                case MagicSpellCounter msc:
                {
                    switch(msc.spellType){
                        case SpellType.Exchange:
                        {
                            GameObject instantiatedCounter = Instantiate(exchangeCounterPrefab, countersLeftLayoutGroup);
                            break;
                        }
                        case SpellType.Double:
                        {
                            GameObject instantiatedCounter = Instantiate(doubleCounterPrefab, countersLeftLayoutGroup);
                            break;
                        }
                    }
                    break;
                }
                case GoldCounter gc:
                {
                    GameObject instantiatedCounter = Instantiate(goldCounterPrefab, countersLeftLayoutGroup);
                    break;
                }
                case ObstacleCounter obc:
                {
                    switch(obc.obstacleType){
                        case ObstacleType.Land:
                        {
                            GameObject instantiatedCounter = Instantiate(landObstaclePrefab, countersLeftLayoutGroup);
                            break;
                        }
                        case ObstacleType.Sea:
                        {
                            GameObject instantiatedCounter = Instantiate(seaObstaclePrefab, countersLeftLayoutGroup);
                            break;
                        }
                    }
                    break;
                }
                default: Debug.Log("Counter is of undefined type!") ; break;
            }
        }
    }

    // // Update is called once per frame
    // void Update()
    // {
    //     //im copy-pasting Max'code and I know nothing what it does lol
    //     float width = this.gameObject.GetComponent<RectTransform>().rect.width;
    //     Vector2 newSize = new Vector2(width / 5.75f, width / 5.75f);
    //     this.gameObject.GetComponent<GridLayoutGroup>().cellSize = newSize;
    // }
}
