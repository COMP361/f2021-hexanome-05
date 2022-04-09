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
}
