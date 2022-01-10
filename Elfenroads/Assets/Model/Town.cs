using System.Collections.Generic;
using Models;

namespace Models {
    public class Town {
        public string name { private set; get; }

        private HashSet<Road> connectedRoads;
        private List<TownPiece> townPieces;
        // Only used in the elfengold extension, skip for now
        // private int goldValue;

        public Town(string name) {
            this.name = name;
            connectedRoads = new HashSet<Road>();
        }


        public void connectRoad(Road road) {
            connectedRoads.Add(road);
        }
    }
}