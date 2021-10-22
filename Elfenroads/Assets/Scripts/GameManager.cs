using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This GameState will likely represent the Phases and "broad" game states, which will each have their own Manager scripts.
public enum BroadGameState{
    SelectBoot,
    MoveBoot
}

public class GameManager : MonoBehaviour
{
    public BroadGameState broadGameState;
    public static event Action<BroadGameState> onBroadStateChange;

    //Game manager is a singleton object, so make the appropriate field/function here. Can be called from other scripts via "GameManager.instance".
    public static GameManager instance = null;
    void Awake(){
        if(instance == null){
            instance = this;
        }else if (instance != this){
            Destroy(gameObject);
        }
    }

    //Function which updates the game state. The switch case could enable extra functionality when switching cases if need be, but for now the "Actions" handle that.
    public void UpdateState(BroadGameState newState){
        broadGameState = newState;

        switch(newState){
            case BroadGameState.SelectBoot:
                break;
            case BroadGameState.MoveBoot:
                break;
        }

        //If any other script is "listening" for a state change, this below line will pass on the new state to them.
        onBroadStateChange?.Invoke(newState);
    }



    private List<GameObject> cities = new List<GameObject>();
    private List<GameObject> roads = new List<GameObject>();
    private List<GameObject> boots = new List<GameObject>();


    void Start()
    {
        //The main manager would typically need to get the game type and perform appropriate setup, but for now 
        //it will just create a  boot, and place it on the starting city (Elvenhold)

        //First though, we get the initial state. For now, start at MoveBoot (if we have extra time, include SelectBoot ***).
        UpdateState(BroadGameState.MoveBoot);

        //First, we update each city, so that they can keep track of all their connected roads.
        foreach(GameObject road in roads){
            road.GetComponent<RoadScript>().getCity1().GetComponent<CityScript>().updateRoads(road);
            road.GetComponent<RoadScript>().getCity2().GetComponent<CityScript>().updateRoads(road);
        }

        //NOTE: Roads, Cities and the Manager may be too tightly coupled - Depending on order of Start() functions, we may get unintended results. For now, it works.
    }

    public void addCity(GameObject cityToAdd)
    {
        this.cities.Add(cityToAdd);
    }

    public void addRoad(GameObject roadToAdd)
    {
        this.roads.Add(roadToAdd);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
