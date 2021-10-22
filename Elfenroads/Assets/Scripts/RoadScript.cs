using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoadType
{
    Plain,
    Forest,
    Mountain,
    Desert,
    Stream, // The "rule" for streams will be to have it flow from city1 -> city2. Create roads according to this rule and there shouldn't be a problem.
    Lake
}


public class RoadScript : MonoBehaviour
{
    public GameObject city1;
    public GameObject city2;
    public RoadType type;
    //For M3, leave it at this. Later on though, we'll need to include counter/token functionality.

    void Start()
    {
        GameManager.instance.addRoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject getCity1(){
        return city1;
    }

    public GameObject getCity2(){
        return city2;
    }
}
