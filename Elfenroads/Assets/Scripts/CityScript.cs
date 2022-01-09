using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CityScript : MonoBehaviour
{
    public string cityName;
    public bool isStartingCity;

    public List<GameObject> townPiecesOnCity = new List<GameObject>();
    
    private List<GameObject> attachedRoads = new List<GameObject>();

    //For now, this is probably enough. There are certainly things we could add, like a way to present all remaining travel points on a city, or gold values.

    void Start()
    {
        GameManager.instance.addCity(gameObject);
    }

    public void updateRoads(GameObject road){
        attachedRoads.Add(road);
        Debug.Log("Road added to city " + cityName);
    }

    public void updateTownPieces(GameObject tPiece){
        townPiecesOnCity.Add(tPiece);
        Debug.Log("Townpiece added to city " + cityName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
