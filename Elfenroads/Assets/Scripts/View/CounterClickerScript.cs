using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Models;

public class CounterClickerScript : MonoBehaviour
{
    public Counter myCounter;
    public Road myRoad;
    private PlanTravelController myController;
    public bool isSelected = false;

    public void setCounterAndRoad(Counter thisCounter, Road thisRoad, PlanTravelController pt){
        myCounter = thisCounter;
        myRoad = thisRoad;
        myController = pt;
    }

    public void OnClick(){
        //Inform the Controller if it is ElfenGold.
        Debug.Log("Counter was clicked!");
        if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.Elfengold) && Elfenroads.Model.game.currentPhase is PlanTravelRoutes){
            myController.counterClicked(this.gameObject, myRoad);
        }
    }

    Color32 normalColor = new Color32(255, 255, 255, 255);
    Color32 selectedColor = new Color32(0, 255, 17, 255);
    UnityEngine.Color lerpedColor = new Color32(255, 255, 255, 255);
    void Update(){
        if(isSelected){
            lerpedColor = UnityEngine.Color.Lerp(normalColor, selectedColor, Mathf.PingPong(Time.time, 1));
            gameObject.GetComponent<SpriteRenderer>().color = lerpedColor;
        }else{
            gameObject.GetComponent<SpriteRenderer>().color = normalColor;
        }
    }
}
