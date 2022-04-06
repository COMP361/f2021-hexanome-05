using System;
using System.Collections.Generic;
using System.Linq;

namespace Models
{
    namespace Helpers
    {
        public static class UpdateHelper
        {
            /// <summary>
            /// Update the list using a deserialized (update) list.
            /// </summary>
            /// <param name="toUpdate">list to be updated</param>
            /// <param name="deserialized">deserialized update</param>
            /// <typeparam name="T">list item type</typeparam>
            /// <returns>true if the list changed, false otherwise.</returns>
            public static bool Update<T>(this List<T> toUpdate, List<T> deserialized) where T : GuidModel {
                if (toUpdate.SequenceEqual(deserialized)) {
                    return false;
                }

                toUpdate.Clear();
                foreach (T item in deserialized) {
                    toUpdate.Add((T) ModelStore.Get(item.id));
                }
                return true;
            }

            public static bool Update<T>(this List<T> toUpdate, List<Counter> deserialized) where T : Counter {
                if (toUpdate.SequenceEqual(deserialized, CounterEqualityComparer.Instance)) {
                    return false;
                }

                toUpdate.Clear();
                foreach (T item in deserialized) {
                    T counter = (T) ModelStore.Get(item.id);
                    counter.isFaceUp = item.isFaceUp;
                    toUpdate.Add(counter);
                }
                return true;
            }

            /// <summary>
            /// Updates the list, and calls .Update() on each item in the list.
            /// </summary>
            /// <param name="toUpdate">list to be updated</param>
            /// <param name="deserialized">deserialized update</param>
            /// <typeparam name="T">list item type</typeparam>
            /// <returns>true if the list (or any item in the list) changed, false otherwise.</returns>
            public static bool DeepUpdate<T>(this List<T> toUpdate, List<T> deserialized) where T : GuidModel, IUpdatable<T> {
                bool modified = false;
                
                if (toUpdate.Update(deserialized)) {
                    modified = true;
                }
                // update each item
                List<bool> results = new List<bool>();
                for (int i = 0; i < toUpdate.Count; i++) {
                    results.Add(toUpdate[i].Update(deserialized[i]));
                }
                
                // check if at least one item was modified
                if (results.Any( (result) => { return result; })) {
                    modified = true;
                }

                return modified;
            }
        }

        class CounterEqualityComparer : IEqualityComparer<Counter> {
            private static CounterEqualityComparer _instance;
            public static CounterEqualityComparer Instance {
                get {
                    if (_instance == null) {
                        _instance = new CounterEqualityComparer();
                    }

                    return _instance;
                }
                private set {
                    _instance = value;
                }
            }


            public bool Equals(Counter c1, Counter c2) {
                if (c2 == null && c1 == null) {
                    return true;
                }
                else if (c1 == null || c2 == null) {
                    return false;
                }
                
                return c1.id == c2.id && c1.isFaceUp == c2.isFaceUp;
            }

            public int GetHashCode(Counter c) {
                return c.id.GetHashCode();
            }
        }
    }

    public interface IUpdatable<T> {
        event EventHandler Updated;

        /// <summary>
        /// Updates the model.
        /// </summary>
        /// <param name="update">deserialized update object</param>
        /// <returns>true if model was modified, false otherwise</returns>
        bool Update(T update);
    }
}