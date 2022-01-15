using System.Collections.Generic;
using System;
using UnityEngine;
using Models;
using Views;


namespace Models {
    public class Board : INotifyModelUpdated {
        public event EventHandler ModelUpdated;
        private Dictionary<string, Town> towns;
        private HashSet<Road> roads;

        public Board() {
            this.towns = new Dictionary<string, Town>();
            this.roads = new HashSet<Road>();
        }

        public Board(List<GameObject> roadObjects, List<GameObject> townObjects){ 

            this.towns = new Dictionary<string, Town>();
            this.roads = new HashSet<Road>();
            foreach(GameObject t in townObjects){
                //Creates a new Town model for each Town gameObject, and adds them to the dictionary   (Shouldn't we also have TownPieces be just an attribute of a Town/TownScript? I.E. a "PlayerVisited" of some kind, for each player?)
                towns.Add(t.GetComponent<TownScript>().townName, new Town(t.GetComponent<TownScript>().townName));
            }
            foreach(GameObject r in roadObjects){
                //Foreach road, find out what type it is and add a corresponding road to "roads". (The Road model constructor automatically connects the towns to itself.)
                RoadType curRoadType = r.GetComponent<RoadScript>().roadType;
                Road newRoad = new Road(towns[r.GetComponent<RoadScript>().startTown.GetComponent<TownScript>().townName], towns[r.GetComponent<RoadScript>().endTown.GetComponent<TownScript>().townName], curRoadType);
                r.GetComponent<RoadView>().setAndSubscribeToModel(newRoad); //Model was just created, so now we make the GameObject/view subscribe to this Model object.
                roads.Add(newRoad);
            }
        }

        public Town getTownByName(string townName){
            if(towns[townName] != null){
                return towns[townName];
            }else{
                Debug.Log("GetTown returned null!"); // Change this to throw an error of some kind once we decide how we're gonna deal with those.
                return null;
            }
        }

    }
}
