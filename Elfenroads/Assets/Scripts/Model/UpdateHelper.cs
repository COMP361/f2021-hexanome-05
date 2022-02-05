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
                    toUpdate.Clear();
                    foreach (T item in deserialized) {
                        toUpdate.Add((T) ModelStore.Get(item.id));
                    }
                    return true;
                }
                return false;
            }

            /// <summary>
            /// Similar to Update(), but treats the list as a mathematical set.
            /// </summary>
            /// <param name="toUpdate">list to be updated</param>
            /// <param name="deserialized">deserialized update</param>
            /// <typeparam name="T">list item type</typeparam>
            /// <returns>true if the list changed, false otherwise.</returns>
            public static bool UpdateUnordered<T>(this List<T> toUpdate, List<T> deserialized) where T : GuidModel {
                if (toUpdate.Except(deserialized).Any() || deserialized.Except(toUpdate).Any()) {
                    toUpdate.Clear();
                    foreach (T item in deserialized) {
                        toUpdate.Add((T) ModelStore.Get(item.id));
                    }
                    return true;
                }
                return false;
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