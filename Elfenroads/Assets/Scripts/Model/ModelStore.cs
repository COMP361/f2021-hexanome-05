using System.Collections.Generic;
using System;
using Views;
using Models;

namespace Models{
    public class ModelStore {
        protected static ModelStore _instance;
        protected static ModelStore StoreInstance() {
            if(_instance == null){
                _instance = new ModelStore();
            }
            return _instance;
        }
        protected Dictionary<Guid, GuidModel> dict;

        protected ModelStore() {
            dict = new Dictionary<Guid, GuidModel>();
        }
        public static void Add(GuidModel model) {
            StoreInstance().dict.Add(model.id, model);
        }

        public static GuidModel Get(Guid id) {
            return StoreInstance().dict[id];
        }
    }
}