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

    public void setCounterAndRoad(Counter thisCounter, Road thisRoad){
        myCounter = thisCounter;
        myRoad = thisRoad;
    }

    public void OnClick(){
        //Inform the Controller if it is ElfenGold.
        if(Elfenroads.Model.game.variant.HasFlag(Game.Variant.Elfengold)){
            myController.counterClicked(myCounter, myRoad);
        }
    }
}
