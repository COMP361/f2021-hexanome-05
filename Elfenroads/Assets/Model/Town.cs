using System.Collections.Generic;
using Models;

namespace Models {
    public class Town {
        public string name { private set; get; }

        //private List<Road> connectedRoads; //Most likely does not need to know about roads?
        private List<TownPiece> townPieces;
        // Only used in the elfengold extension, skip for now
        // private int goldValue;

        public Town(string name) {
            this.name = name;
            //connectedRoads = new List<Road>();
        }


        //public void connectRoad(Road road) {
        //    connectedRoads.Add(road);
        //}
    }
}