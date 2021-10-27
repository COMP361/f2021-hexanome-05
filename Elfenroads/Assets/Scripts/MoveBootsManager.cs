using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBootsManager : MonoBehaviour
{

    public static MoveBootsManager instance = null;
    void Awake(){
        if(instance == null){
            instance = this;
            //This below line essentially lets this script observe the GameManager whenever its state is changed.
            GameManager.onBroadStateChange += GameManagerOnGameStateChanged;
        }else if (instance != this){
            Destroy(gameObject);
        }
        
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
            highlightRoads(boots[0]);

        }
    }

    void moveBoot(GameObject boot){

        boot.transform.position = boot.GetComponent<BootScript>().getCurrentCity().transform.position;
    }

    void highlightRoads(GameObject boot){
        
        GameObject curCity = boot.GetComponent<BootScript>().getCurrentCity();
        
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

        foreach(GameObject boot in boots){
            if(boot.GetComponent<BootScript>().getCurrentCity() == city1){
                boot.GetComponent<BootScript>().setCurrentCity(city2);
            }else if(boot.GetComponent<BootScript>().getCurrentCity() == city2){
                boot.GetComponent<BootScript>().setCurrentCity(city1);
            }else{
                Debug.Log("Invalid operation!");
            }

            moveBoot(boot);
            highlightRoads(boot);
        }
    }

}
