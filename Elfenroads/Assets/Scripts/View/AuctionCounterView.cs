using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Models;
using Views;
public class AuctionCounterView : MonoBehaviour, GuidHelperContainer
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
                           GameObject instantiatedCounter = Instantiate(dragonCounterPrefab, this.transform);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setGuid(c.id);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setContainer(this);
                            break;
                        }
                        case TransportType.ElfCycle:
                        {
                            GameObject instantiatedCounter = Instantiate(cycleCounterPrefab, this.transform);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setGuid(c.id);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setContainer(this);
                            break;
                        }
                        case TransportType.MagicCloud:
                        {
                            GameObject instantiatedCounter = Instantiate(cloudCounterPrefab, this.transform);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setGuid(c.id);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setContainer(this);
                            break;
                        }
                        case TransportType.TrollWagon:
                        {
                            GameObject instantiatedCounter = Instantiate(trollCounterPrefab, this.transform);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setGuid(c.id);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setContainer(this);
                            break;
                        }
                        case TransportType.GiantPig:
                        {
                            GameObject instantiatedCounter = Instantiate(pigCounterPrefab, this.transform);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setGuid(c.id);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setContainer(this);
                            break;
                        }
                        case TransportType.Unicorn:
                        {
                            GameObject instantiatedCounter = Instantiate(unicornCounterPrefab, this.transform);
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
                    switch(msc.spellType){
                        case SpellType.Exchange:
                        {
                            GameObject instantiatedCounter = Instantiate(exchangeCounterPrefab, this.transform);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setGuid(c.id);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setContainer(this);
                            break;
                        }
                        case SpellType.Double:
                        {
                            GameObject instantiatedCounter = Instantiate(doubleCounterPrefab, this.transform);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setGuid(c.id);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setContainer(this);
                            break;
                        }
                    }
                    break;
                }
                case GoldCounter gc:
                {
                    GameObject instantiatedCounter = Instantiate(goldCounterPrefab, this.transform);
                    instantiatedCounter.GetComponent<GuidViewHelper>().setGuid(c.id);
                    instantiatedCounter.GetComponent<GuidViewHelper>().setContainer(this);
                    break;
                }
                case ObstacleCounter obc:
                {
                    switch(obc.obstacleType){
                        case ObstacleType.Land:
                        {
                            GameObject instantiatedCounter = Instantiate(landObstaclePrefab, this.transform);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setGuid(c.id);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setContainer(this);
                            break;
                        }
                        case ObstacleType.Sea:
                        {
                            GameObject instantiatedCounter = Instantiate(seaObstaclePrefab, this.transform);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setGuid(c.id);
                            instantiatedCounter.GetComponent<GuidViewHelper>().setContainer(this);
                            break;
                        }
                    }
                    break;
                }
                default: Debug.Log("Counter is of undefined type!") ; break;
            }
        }
    }
    

    public void GUIClicked(GameObject clickedCounter){
         // well, it does nothing
    }


    // Update is called once per frame
    void Update()
    {
        //im copy-pasting Max'code and I know nothing what it does lol
        float width = this.gameObject.GetComponent<RectTransform>().rect.width;
        Vector2 newSize = new Vector2(width / 5.75f, width / 5.75f);
        this.gameObject.GetComponent<GridLayoutGroup>().cellSize = newSize;
    }
}
