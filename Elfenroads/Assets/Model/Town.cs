using System.Collections.Generic;
using System;
using Models;

namespace Models {
    public class Town : INotifyModelUpdated, IUpdatable<Town> {
        public event EventHandler ModelUpdated;
        public string name { private set; get; }

        //private List<Road> connectedRoads; //Most likely does not need to know about roads?
        private List<TownPiece> townPieces;
        private List<Boot> boots;
        // Only used in the elfengold extension, skip for now
        // private int goldValue;

        public Town(string name) {
            this.name = name;
            //connectedRoads = new List<Road>();
        }

        // public void AddBoot(Boot boot) {
        //     if (boots.Contains(boot)) {
        //         throw new ArgumentException();
        //     }
        //     boots.Add(boot);
        //     ModelHelper.StoreInstance().getBoot(1);
        // }

        // Calculate the difference, get ID's, get reference by calling modelhelper, 
        public void Update(Town inputTown) {
            // HashSet<TownPiece> inputTownPieceList = new HashSet<TownPiece>(inputTown.townPieces);
            // HashSet<TownPiece> townPieceList = new HashSet<TownPiece>(townPieces);
            // HashSet<TownPiece> = new townPieceList.ExceptWith(inputTownPieceList); // original - new (the things that were taken out)
            // HashSet<TownPiece> = new inputTownPieceList.ExceptWith(townPieceList); // new - original (the things that were added in)
        }
    }
}