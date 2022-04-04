using System.Collections.Generic;
using System;
using Views;
using Models;
using UnityEngine;

namespace Models{
    public class ModelStore {
        protected static ModelStore _instance;
        protected static ModelStore StoreInstance() {
            if(_instance == null){
                _instance = new ModelStore();
            }
            return _instance;
        }

        public static void ResetInstance() {
            _instance = null;
        }
        protected Dictionary<Guid, GuidModel> dict;

        protected ModelStore() {
            dict = new Dictionary<Guid, GuidModel>();
        }
        public static void Add(GuidModel model) {
            //Debug.Log("Type: " + model.GetType() + " with Id:" + model.id );
            StoreInstance().dict.Add(model.id, model);
        }

        public static GuidModel Get(Guid id) {
            return StoreInstance().dict[id];
        }
    }
}