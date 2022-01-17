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

        private Dictionary<int, TownPiece> townPieces;
        private Dictionary<int, Boot> boots;
        // Dictionary of Counters
        // Dictionary of TravelCards

        public ModelHelper(){
            townPieces = new Dictionary<int, TownPiece>();
            boots = new Dictionary<int, Boot>();
        }

        public TownPiece GetTownPiece(int id){
            return townPieces[id];
        }

        public void addBoot(Boot inputBoot){
            boots.Add(inputBoot.id, inputBoot);
        }

        public Boot getBoot(int id){
            return boots[id];
        }
    }
}