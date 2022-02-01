using System.Collections.Generic;
using System;
using Views;
using Models;

namespace Models{
    public class ModelHelper {

        private static ModelHelper _instance;
        public static ModelHelper StoreInstance(){
            if(_instance == null){
                _instance = new ModelHelper();
            }
            return _instance;
        }

        private Dictionary<string, Town> towns;
        private Dictionary<int, Road> roads;
        private Dictionary<int, TownPiece> townPieces;
        private Dictionary<Guid, Boot> boots;
        // Dictionary of Counters
        // Dictionary of TravelCards

        public ModelHelper(){
            townPieces = new Dictionary<int, TownPiece>();
            boots = new Dictionary<Guid, Boot>();
            towns = new Dictionary<string, Town>();
            roads = new Dictionary<int, Road>();
        }

        public void addTown(Town town){
            towns.Add(town.name, town);
        }

        public Town getTown(string townName){
            return towns[townName];
        }

        public void addRoad(Road road){
            roads.Add(road.id, road);
        }

        public Road getRoad(int id){
            return roads[id];
        }

        public void addTownPiece(TownPiece townPiece){
            townPieces.Add(townPiece.id, townPiece);
        }

        public TownPiece getTownPiece(int id){
            return townPieces[id];
        }

        public void addBoot(Boot inputBoot){
            boots.Add(inputBoot.id, inputBoot);
        }

        public Boot getBoot(Guid id){
            return boots[id];
        }
    }
}