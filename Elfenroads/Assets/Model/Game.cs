using System.Collections.Generic;
using UnityEngine;


namespace Models {
    public class Game {
        private Dictionary<string, Town> towns;
        private HashSet<Road> roads;
        private List<Player> players;
        private Player currentPlayer;
        //Later: Add currentPhase, currentRound, isElfenGold, phaseLoops, and variants. (and startingPlayer)

        public Game() {
            this.towns = new Dictionary<string, Town>();
            this.roads = new HashSet<Road>();
        }

        public void createRoadsAndTowns(List<GameObject> roadObjects, List<GameObject> townObjects){
            foreach(GameObject t in townObjects){
                //Creates a new Town model for each Town gameObject, and adds them to the dictionary   (Shouldn't we also have TownPieces be just an attribute of a Town/TownScript? I.E. a "PlayerVisited" of some kind, for each player?)
                towns.Add(t.GetComponent<TownScript>().townName, new Town(t.GetComponent<TownScript>().townName));
            }

            foreach(GameObject r in roadObjects){
                //Foreach road, find out what type it is and add a corresponding road to "roads". (The Road model constructor automatically connects the towns to itself.)
                RoadType curRoadType = r.GetComponent<RoadScript>().roadType;
                roads.Add(new Road(towns[r.GetComponent<RoadScript>().startTown.GetComponent<TownScript>().townName], towns[r.GetComponent<RoadScript>().endTown.GetComponent<TownScript>().townName], curRoadType));
            }
        }

        public void createPlayers(List<string> playerNames){
            foreach(string name in playerNames){
                Player newPlayer = new Player(name, Color.NONE, towns["Elfenhold"]);
                players.Add(newPlayer);
            }
        }

    }
}