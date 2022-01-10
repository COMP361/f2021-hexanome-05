using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;

public enum RoadType {
    Plain,
    Forest,
    Mountain,
    Desert,
    Stream, // The "rule" for streams will be to have it flow from city1 -> city2. Create roads according to this rule and there shouldn't be a problem.
    Lake
}

public class RoadScript : MonoBehaviour {
    [HideInInspector]
    public Road road { private set; get; }
    public RoadType roadType;
    public GameObject startTown;
    public GameObject endTown;

    void Start() {
        Town start = startTown.GetComponent<TownScript>().town;
        Town end = endTown.GetComponent<TownScript>().town;

        switch (roadType) {
            case RoadType.Plain:
                road = new PlainRoad(start, end);
                break;
            case RoadType.Lake:
                road = new LakeRoad(start, end);
                break;
            case RoadType.Stream:
                road = new StreamRoad(start, end);
                break;
            case RoadType.Forest:
                road = new ForestRoad(start, end);
                break;
            case RoadType.Desert:
                road = new DesertRoad(start, end);
                break;
            case RoadType.Mountain:
                road = new MountainRoad(start, end);
                break;
            default:
                Debug.LogError("Road type " + roadType.ToString() + " has not been implemented!");
                break;
        }
    }

    public void onClick() {
        //This road was clicked. Inform the MoveBootsManager, who will verify that it was a valid road to click on for movement.
        //(I suppose I could take out this middle man and just pass the road object to an onClick() in the GameManager, but then the manager would have very many onClick() functions.)
        // MoveBootsManager.instance.roadClicked(gameObject);
    }
}
