using System.Collections.Generic;
using System;
using Models;
using Views;


namespace Models {
    public class Board {
        public List<Town> towns { protected set; get; }
        public List<Road> roads { protected set; get; }

        public Board(List<Road> roads, List<Town> towns) {
            this.roads = new List<Road>(roads);
            this.towns = new List<Town>(towns);
        }
    }
}
