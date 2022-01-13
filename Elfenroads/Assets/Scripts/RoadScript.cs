using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;


public class RoadScript : MonoBehaviour {
    [HideInInspector]
    public Road road { private set; get; }
    public RoadType roadType;
    public GameObject startTown;
    public GameObject endTown;

    void Start() {
        Town start = startTown.GetComponent<TownScript>().town;
        Town end = endTown.GetComponent<TownScript>().town;
        road = new Road(start, end, roadType);
    }

    public void onClick() {
        //This road was clicked. Inform the MoveBootsManager, who will verify that it was a valid road to click on for movement.
        //(I suppose I could take out this middle man and just pass the road object to an onClick() in the GameManager, but then the manager would have very many onClick() functions.)
        // MoveBootsManager.instance.roadClicked(gameObject);
    }
}
