using System.Collections.Generic;
using System;
using Views;
using Models;

namespace Models{
    public class ModelStore {
        public static void Add(GuidModel model) {
            StoreInstance().dict.Add(model.id, model);
        }

        public static GuidModel Get(Guid id) {
            return StoreInstance().dict[id];
        }

        private ModelStore() {
            dict = new Dictionary<Guid, GuidModel>();
        }

        private static ModelStore _instance;
        private static ModelStore StoreInstance() {
            if(_instance == null){
                _instance = new ModelStore();
            }
            return _instance;
        }

        private Dictionary<Guid, GuidModel> dict;
    }
}