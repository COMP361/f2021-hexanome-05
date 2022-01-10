using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Models;

//This GameState will likely represent the Phases and "broad" game states, which will each have their own Manager scripts.
public enum BroadGameState{
    SelectBoot,
    MoveBoot
}

public class GameManager : MonoBehaviour
{
    private BroadGameState broadGameState;
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



    public GameObject bootPrefab;
    public GameObject townPiecePrefab;
    public GameObject inventoryPrefab;

    public BootColor myBootColor = BootColor.RED;

    private List<GameObject> cities = new List<GameObject>();
    private List<GameObject> roads = new List<GameObject>();
    private List<GameObject> boots = new List<GameObject>();
    private List<GameObject> inventories = new List<GameObject>();

    void Start()
    {
        //The main manager would typically need to get the game type and perform appropriate setup, but for now 
        //it will just create a  boot, and place it on the starting city (Elvenhold)

        //First though, we get the initial state. For now, start at MoveBoot (if we have extra time, include SelectBoot ***).
        //UpdateState(BroadGameState.MoveBoot);

        //First, we update each city, so that they can keep track of all their connected roads.
        foreach(GameObject road in roads){
            road.GetComponent<RoadScript>().getCity1().GetComponent<CityScript>().updateRoads(road);
            road.GetComponent<RoadScript>().getCity2().GetComponent<CityScript>().updateRoads(road);
        }

        GameObject startingCity = GameObject.Find("Elfenhold");

        Invoke("instantiatePieces", 0.05f);

        // Initialize Inventory
        var inventory_BLUE = Instantiate(inventoryPrefab) as GameObject;
        var inventory_RED = Instantiate(inventoryPrefab) as GameObject;
        inventories.Add(inventory_BLUE);
        inventories.Add(inventory_RED);
        inventory_BLUE.GetComponent<InventoryManager>().setTownPieceCounter(GameObject.Find("TPCounter_Blue"));
        inventory_RED.GetComponent<InventoryManager>().setTownPieceCounter(GameObject.Find("TPCounter_RED"));

        // Initialize RedBoot
        var instantiatedBoot_RED = Instantiate(bootPrefab) as GameObject;
        instantiatedBoot_RED.GetComponent<BootScript>().Offset = new Vector3(-1,0,0);
        instantiatedBoot_RED.GetComponent<BootScript>().setColor(BootColor.RED);
        instantiatedBoot_RED.transform.position = startingCity.transform.position + instantiatedBoot_RED.GetComponent<BootScript>().Offset;
        instantiatedBoot_RED.GetComponent<BootScript>().setCurrentTown(startingCity);
        instantiatedBoot_RED.GetComponent<BootScript>().setInventory(inventory_RED);
        boots.Add(instantiatedBoot_RED);

        // Initialize BlueBoot
        var instantiatedBoot_BLUE = Instantiate(bootPrefab) as GameObject;
        instantiatedBoot_BLUE.GetComponent<SpriteRenderer>().sprite = instantiatedBoot_BLUE.GetComponent<BootScript>().blueSprite;
        instantiatedBoot_BLUE.GetComponent<BootScript>().Offset = new Vector3(1,0,0);
        instantiatedBoot_BLUE.GetComponent<BootScript>().setColor(BootColor.BLUE);
        instantiatedBoot_BLUE.transform.position = startingCity.transform.position + instantiatedBoot_BLUE.GetComponent<BootScript>().Offset;
        instantiatedBoot_BLUE.GetComponent<BootScript>().setCurrentTown(startingCity);
        instantiatedBoot_BLUE.GetComponent<BootScript>().setInventory(inventory_BLUE);
        boots.Add(instantiatedBoot_BLUE);


         // initialize townpieces of starting city and put them into inventory, for now it is Elfenhold
        foreach(GameObject boot in boots){
            var townPiece = Instantiate(townPiecePrefab) as GameObject;
                townPiece.GetComponent<TownPieceManager>().town = startingCity;
                townPiece.GetComponent<TownPieceManager>().setColor(boot.GetComponent<BootScript>().getColor());
                boot.GetComponent<BootScript>().getInventory().GetComponent<InventoryManager>().addTownPiece(townPiece);
            
        }

        MoveBootsManager.instance.passBoots(boots);
        MoveBootsManager.instance.passRoads(roads);

        UpdateState(BroadGameState.MoveBoot);
        Invoke("callHighlightRoads", 0.05f);

        MoveBootsManager.instance.startListening();
        Cursor.lockState = CursorLockMode.Confined;


        //NOTE: Roads, Cities and the Manager may be too tightly coupled - Depending on order of Start() functions, we may get unintended results. For now, it works.
    }

    void instantiatePieces(){
        GameObject startingCity = GameObject.Find("Elfenhold");
        // Initialize TownPieces for every city
        foreach(GameObject city in cities){
            if (city != startingCity){
                var townPiece_RED = Instantiate(townPiecePrefab) as GameObject;
                townPiece_RED.GetComponent<TownPieceManager>().town = city;
                townPiece_RED.GetComponent<TownPieceManager>().setColor(BootColor.RED);
                townPiece_RED.transform.position = city.transform.position + new Vector3(-1,0,0);

                var townPiece_BLUE = Instantiate(townPiecePrefab) as GameObject;
                townPiece_BLUE.GetComponent<SpriteRenderer>().sprite = townPiece_BLUE.GetComponent<TownPieceManager>().blueSprite;
                townPiece_BLUE.GetComponent<TownPieceManager>().town = city;
                townPiece_BLUE.GetComponent<TownPieceManager>().setColor(BootColor.BLUE);
                townPiece_BLUE.transform.position = city.transform.position + new Vector3(1,0,0);

                city.GetComponent<CityScript>().updateTownPieces(townPiece_RED);
                city.GetComponent<CityScript>().updateTownPieces(townPiece_BLUE);
            }
        }
    }

    void removeStartingTownPiece(GameObject instantiatedBoot_RED, GameObject instantiatedBoot_BLUE){
        GameObject startingCity = GameObject.Find("Elfenhold");
         // remove townpieces at the starting city and put them into inventory, for now it is Elfenhold
        foreach(GameObject townPiece in startingCity.GetComponent<CityScript>().townPiecesOnCity){
            if (townPiece.GetComponent<TownPieceManager>().getColor() == instantiatedBoot_RED.GetComponent<BootScript>().getColor()){
                instantiatedBoot_RED.GetComponent<BootScript>().getInventory().GetComponent<InventoryManager>().addTownPiece(townPiece);
            }
            else if (townPiece.GetComponent<TownPieceManager>().getColor() == instantiatedBoot_BLUE.GetComponent<BootScript>().getColor()){
                instantiatedBoot_BLUE.GetComponent<BootScript>().getInventory().GetComponent<InventoryManager>().addTownPiece(townPiece);
            }
        }

        startingCity.GetComponent<CityScript>().townPiecesOnCity.Clear();
    }

    void callHighlightRoads(){
        MoveBootsManager.instance.highlightRoads();
    }

    public void addCity(GameObject cityToAdd)
    {
        this.cities.Add(cityToAdd);
    }

    public void addRoad(GameObject roadToAdd)
    {
        this.roads.Add(roadToAdd);
    }
}
