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

        private Game game;

        private void Awake() {
            Elfenroads.Model = this;

            //Once the session is launched, this is where all required model objects are created and assigned to their appropriate GameObjects (though their other attributes will contain default or null values if they are subject to change, since they would need
            // to be updated via the Server once it has set up the initial state). (See ElfenroadsControl)
        
            //So, create the Game model. First, get all the Road and Town objects from the GameObjects.

            Dictionary<string, Town> towns = new Dictionary<string, Town>();
            List<Road> roads = new List<Road>();
            foreach(GameObject t in townObjects){
                //Creates a new Town model for each Town gameObject, and adds them to the dictionary   (Shouldn't we also have TownPieces be just an attribute of a Town/TownView? I.E. a "PlayerVisited" of some kind, for each player?)
                towns.Add(t.GetComponent<TownView>().townName, new Town(t.GetComponent<TownView>().townName));
            }
            foreach(GameObject r in roadObjects){
                //Foreach road, find out what type it is and add a corresponding road to "roads".
                RoadType curRoadType = r.GetComponent<RoadView>().roadType;
                Road newRoad = new Road(towns[r.GetComponent<RoadView>().startTown.GetComponent<TownView>().townName], towns[r.GetComponent<RoadView>().endTown.GetComponent<TownView>().townName], curRoadType);
                //r.GetComponent<RoadView>().setAndSubscribeToModel(newRoad); //Model was just created, so now we make the GameObject/view subscribe to this Model object.
                roads.Add(newRoad);
            }

            Board board = new Board(roads,towns);
            //game = new Game(sessionInfo);
            game = new Game(board);
            //For testing purposes right now, we'll instantiate a single player in our model. Later, we'd have to get the list of Players either from the Server or from the MenuScene.
            List<string> testList = new List<string>();
            testList.Add("test");
            //game.createPlayers(testList);
            game.SetBoard(board);

            // Load Scene -> Create Game and Board -> Server asks Host for variants -> Host replies to Server -> Server updates the gamemode and returns it -> Server asks for colors -> Clients respond -> Phase 1 starts
            //ModelReady triggered here.
        }



        //Called by the GameController once the amount of players (and their names) have been received from the Server (passwords in model not needed)   *** Grab from menuscene? But then, how to assign players correctly?
        public void createPlayers(List<string> playerNames){ 
            game.createPlayers(playerNames);
        }  

        // public Player getCurrentPlayer() {
        
        // }

    }

}