using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firesplash.UnityAssets.SocketIO;

using Newtonsoft.Json;

public class MoveBootsManager : MonoBehaviour
{

    public static MoveBootsManager instance = null;
    public SocketIOCommunicator sioCom;

    void Awake(){
        if(instance == null){
            instance = this;
            //This below line essentially lets this script observe the GameManager whenever its state is changed.
            GameManager.onBroadStateChange += GameManagerOnGameStateChanged;
        }else if (instance != this){
            Destroy(gameObject);
        }

        //////////////////////////////////////////////////////////////////
        // sioCom.Instance.On("unityConnection", (string payload) => {  //
        //    Debug.Log("SERVER: " + payload);                          //
        // });                                                          //
        //////////////////////////////////////////////////////////////////
    }

    public void startListening(){
        // Listening for JSON updates from the server and deserializing
        Game serverUpdate = new Game();
        sioCom.Instance.On("MoveBoot", (string payload) => { 
             serverUpdate = JsonConvert.DeserializeObject<Game>(payload);  
             // Update all boots positions
            foreach (PlayerInfo playerInfo in serverUpdate.playerList){
                foreach(GameObject aboot in boots){
                    if (aboot.GetComponent<BootScript>().getColor() == playerInfo.bootColor){
                        moveBoot(aboot);
                    } 
                }
            } 
        }); 

    }

    private List<GameObject> boots;
    private List<GameObject> roads;

    public void passBoots(List<GameObject> bootsInput){
        this.boots = bootsInput;
    }

    public void passRoads(List<GameObject> roadsInput){
        this.roads = roadsInput;
        
    }

    //This function does something depending on the aforementioned game state change.
    void GameManagerOnGameStateChanged(BroadGameState state)
    {
        if (state == BroadGameState.MoveBoot)
        {
            Debug.Log("Time for MoveBoots!");
            //For now, with just one boot, we'll just check the road highlighting here. We'll need to implement this with a more sophisticated turn-based setting later.
            //highlightRoads(boots[0]);
            //highlightRoads();

        }
    }

    void moveBoot(GameObject boot){
        
        // move boot
        boot.transform.position = boot.GetComponent<BootScript>().getCurrentCity().transform.position + boot.GetComponent<BootScript>().Offset;

        // remove townpieces and add into player
        BootColor color = boot.GetComponent<BootScript>().getColor();
        List<GameObject> townPiecesList = boot.GetComponent<BootScript>().getCurrentCity().GetComponent<CityScript>().townPiecesOnCity;

        GameObject toRemove = null;

        foreach (GameObject townPiece in townPiecesList){
            if (townPiece.GetComponent<TownPieceManager>().getColor() == boot.GetComponent<BootScript>().getColor()){
                boot.GetComponent<BootScript>().getInventory().GetComponent<InventoryManager>().addTownPiece(townPiece);
                townPiece.SetActive(false);
                toRemove = townPiece;
            }
        }

        if (toRemove != null){
            townPiecesList.Remove(toRemove);
        }

        

        // Gathering the update info into a JSON format and sending it to the socket
        PlayerInfo playerUpdate = new PlayerInfo();

        playerUpdate.username = "Bob";
        playerUpdate.bootColor = boot.GetComponent<BootScript>().getColor();
        playerUpdate.currentTown = boot.GetComponent<BootScript>().getCurrentCity();
        playerUpdate.townPieces = boot.GetComponent<BootScript>().getInventory().GetComponent<InventoryManager>().getTownPieces();

        Dictionary<string, string> InfoList = new Dictionary<string, string>();
        InfoList.Add("game_id", "2159465239968617637");
        InfoList.Add("name", "maex");
        InfoList.Add("currentTown", boot.GetComponent<BootScript>().getCurrentCity().GetComponent<CityScript>().cityName);

        sioCom.Instance.Emit("MoveBoot", JsonConvert.SerializeObject(InfoList),false);

        //sioCom.Instance.Emit("MoveBoot", JsonConvert.SerializeObject(playerUpdate),false);

        //sioCom.Instance.Emit("MoveBoot", "Test string", true);

        /////////////////////////////////////////////////////////////
        // sioCom.Instance.Emit("unityConnection", "Hello", true); //
        /////////////////////////////////////////////////////////////
    }

    public void highlightRoads(){
        
        GameObject curCity = boots[0].GetComponent<BootScript>().getCurrentCity();
        
        foreach(GameObject road in roads){
            GameObject city1 = road.GetComponent<RoadScript>().getCity1();
            GameObject city2 = road.GetComponent<RoadScript>().getCity2();

            if(city1 == curCity || city2 == curCity){
                road.GetComponent<SpriteRenderer>().color = Color.blue;
            }else{
                road.GetComponent<SpriteRenderer>().color = Color.black;
            }
        }
    }

    public void roadClicked(GameObject road){
        GameObject city1 = road.GetComponent<RoadScript>().getCity1();
        GameObject city2 = road.GetComponent<RoadScript>().getCity2();
        GameObject boot;

       foreach(GameObject aboot in boots){
            if (aboot.GetComponent<BootScript>().getColor() == GameObject.Find("GameManager").GetComponent<GameManager>().myBootColor){
                boot = aboot;

                 if(boot.GetComponent<BootScript>().getCurrentCity() == city1){
                boot.GetComponent<BootScript>().setCurrentCity(city2);
                }else if(boot.GetComponent<BootScript>().getCurrentCity() == city2){
                boot.GetComponent<BootScript>().setCurrentCity(city1);
                }else{
                Debug.Log("Invalid operation!");
                }

            // if (boot.GetComponent<BootScript>().getColor() == GameObject.Find("GameManager").GetComponent<GameManager>().myBootColor){
                moveBoot(boot);
            // }
            } 
        }

           
            highlightRoads();


    }

    public class Game{
        public string _id {get; set;}
        public string gameType {get; set;}
        public int game_id {get; set;}
        public PlayerInfo[] playerList {get; set;}
    }

    public class PlayerInfo{
        public GameObject currentTown {get; set;}
        public List<GameObject> townPieces {get; set;}
        public string username {get; set;}
        public BootColor bootColor {get; set;}
    }

}
