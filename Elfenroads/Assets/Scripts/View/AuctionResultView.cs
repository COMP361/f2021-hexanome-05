using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Models;
using Views;

public class AuctionResultView : MonoBehaviour
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

    public RectTransform currentCounterLayoutGroup;

    // Start is called before the first frame update
    void Start()
    {
        myController = GameObject.Find("AuctionController").GetComponent<AuctionController>();
        sessionInfo = GameObject.Find("SessionInfo"); 
    }

    public void updateCurrentCounter(Auction auctionModel) { 
        Debug.Log("Model was updated!");
        //First, destroy all children
        foreach(Transform child in currentCounterLayoutGroup){
            child.SetParent(null);
            DestroyImmediate(child.gameObject);
        }
        currentCounterLayoutGroup.DetachChildren();
        
        //Now, loop through the cards of the model, instantiating appropriate counter each time.
        Counter c = auctionModel.getCurrentAuctioningCounter(); // we should not have updated this yet
        switch(c){
            case TransportationCounter tc:
            {
                //Debug.Log("Transport type of " + c.id + " is: " + tc.transportType);
                switch(tc.transportType){
                    case TransportType.Dragon:
                    {  
                        GameObject instantiatedCounter = Instantiate(dragonCounterPrefab, this.transform);
                        break;
                    }
                    case TransportType.ElfCycle:
                    {
                        GameObject instantiatedCounter = Instantiate(cycleCounterPrefab, this.transform);
                        break;
                    }
                    case TransportType.MagicCloud:
                    {
                        GameObject instantiatedCounter = Instantiate(cloudCounterPrefab, this.transform);
                        break;
                    }
                    case TransportType.TrollWagon:
                    {
                        GameObject instantiatedCounter = Instantiate(trollCounterPrefab, this.transform);
                        break;
                    }
                    case TransportType.GiantPig:
                    {
                        GameObject instantiatedCounter = Instantiate(pigCounterPrefab, this.transform);
                        break;
                    }
                    case TransportType.Unicorn:
                    {
                        GameObject instantiatedCounter = Instantiate(unicornCounterPrefab, this.transform);
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
                        break;
                    }
                    case SpellType.Double:
                    {
                        GameObject instantiatedCounter = Instantiate(doubleCounterPrefab, this.transform);
                        break;
                    }
                }
                break;
            }
            case GoldCounter gc:
            {
                GameObject instantiatedCounter = Instantiate(goldCounterPrefab, this.transform);
                break;
            }
            case ObstacleCounter obc:
            {
                switch(obc.obstacleType){
                    case ObstacleType.Land:
                    {
                        GameObject instantiatedCounter = Instantiate(landObstaclePrefab, this.transform);
                        break;
                    }
                    case ObstacleType.Sea:
                    {
                        GameObject instantiatedCounter = Instantiate(seaObstaclePrefab, this.transform);
                        break;
                    }
                }
                break;
            }
            default: Debug.Log("Counter is of undefined type!") ; break;
        }
    }


    // Update is called once per frame
    // void Update()
    // {
    //     //im copy-pasting Max'code and I know nothing what it does lol
    //     float width = this.gameObject.GetComponent<RectTransform>().rect.width;
    //     Vector2 newSize = new Vector2(width / 5.75f, width / 5.75f);
    //     this.gameObject.GetComponent<GridLayoutGroup>().cellSize = newSize;
    // }
}
