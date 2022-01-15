using System.Collections.Generic;
using System;
using Models;


namespace Models {
    public class Board : INotifyModelUpdated {
        public event EventHandler ModelUpdated;
        private HashSet<Town> towns;
        private HashSet<Road> roads;

        public Board() {
            this.towns = new HashSet<Town>();
            this.roads = new HashSet<Road>();
        }

        // public Board(IEnumerable<Town> towns, IEnumerable<Road> roads) {
        //     this.towns = new HashSet<Town>(towns);
        //     this.roads = new HashSet<Road>(roads);
        // }

        public void addTown(Town town) {
            towns.Add(town);
        }

        public void addRoad(Road road) {
            roads.Add(road);
        }
    }
}
