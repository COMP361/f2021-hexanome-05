using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Models;
using Views;

namespace Models {
    public interface INotifyModelUpdated {
        event EventHandler ModelUpdated;
    }

    public interface IUpdatable<T> {
        void Update(T updated);
    }

    public class ElfenroadsModel : Elfenroads {
        public event EventHandler ModelReady;
        public List<GameObject> roadObjects;
        public List<GameObject> townObjects;

        [HideInInspector]
        public Player curPlayer;

        private Game game;

        private void Awake() {
            Elfenroads.Model = this;

            //Once the session is launched, this is where all required model objects are created and assigned to their appropriate GameObjects (though their other attributes will contain default or null values if they are subject to change, since they would need
            // to be updated via the Server once it has set up the initial state). (See ElfenroadsControl)
        
            //So, create the Game model. First, get all the Road and Town objects from the GameObjects.

            // List<Town> towns = new List<Town>();
            // List<Road> roads = new List<Road>();
            // foreach(GameObject t in townObjects){
            //     //Creates a new Town model for each Town gameObject, and adds them to the dictionary   (Shouldn't we also have TownPieces be just an attribute of a Town/TownView? I.E. a "PlayerVisited" of some kind, for each player?)
            //     Town newTown = new Town(t.GetComponent<TownView>().townName);
            //     towns.Add(newTown);
            //     ModelHelper.StoreInstance().addTown(newTown);
            //     //Debug.Log("Added town " + newTown.name + " to the store!");
            // }
            // foreach(GameObject r in roadObjects){
            //     //Foreach road, find out what type it is and add a corresponding road to "roads".
            //     TerrainType curRoadType = r.GetComponent<RoadView>().roadType;
            //     Road newRoad = new Road(towns[r.GetComponent<RoadView>().startTown.GetComponent<TownView>().townName], towns[r.GetComponent<RoadView>().endTown.GetComponent<TownView>().townName], curRoadType, r.GetComponent<RoadView>().id );
            //     ModelHelper.StoreInstance().addRoad(newRoad);
            //     //Debug.Log("Added road " + newRoad.id + " to the store!");
            //     //r.GetComponent<RoadView>().setAndSubscribeToModel(newRoad); //Model was just created, so now we make the GameObject/view subscribe to this Model object.
            //     roads.Add(newRoad);
            // }

            // Board board = new Board(roads,towns);
            // game = new Game(board);
            // //For testing purposes right now, we'll instantiate a single player in our model. Later, we'd have to get the list of Players either from the Server or from the MenuScene.
            // game.createPlayerTest();
            // ModelHelper.StoreInstance().getTown("Elfenhold").boots.Add(ModelHelper.StoreInstance().getBoot(0));
            // Invoke("ready", 0.1f); //Wait 1s for other awake() functions to finish, then announce that the model is ready. Only here for testing.
        }

        private void ready(){
            ModelReady?.Invoke(game, EventArgs.Empty);
        }

        // public Player getCurrentPlayer() {
        
        // }

    }

}