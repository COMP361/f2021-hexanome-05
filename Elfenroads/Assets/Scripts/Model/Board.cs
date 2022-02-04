using System.Collections.Generic;
using System;
using Models;
using Views;


namespace Models {
    public class Board {
        private List<Town> towns;
        private List<Road> roads;

        public Board(List<Road> roads, List<Town> towns) {
            this.roads = new List<Road>(roads);
            this.towns = new List<Town>(towns);
        }
    }
}
