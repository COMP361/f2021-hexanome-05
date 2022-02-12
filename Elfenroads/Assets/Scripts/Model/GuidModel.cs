using Models;
using System;
using UnityEngine;

namespace Models {
    /// <summary>
    /// Model with a GUID
    /// </summary>
    public abstract class GuidModel : IEquatable<GuidModel> {
        public Guid id { private set; get; }

        protected GuidModel() { //Something funky's going on here...
            //this.id = new Guid();
        }

        protected GuidModel(Guid id) {
            this.id = id;
        }

        public bool Equals(GuidModel other) {
            //Debug.Log("Equality check: " + this.id + " and " + other.id );
            return other != null &&
                    other.id == this.id;
        }

        public override bool Equals(object obj)
        {   
            return Equals(obj as GuidModel);
        }

        public override int GetHashCode()
        {
            return id.GetHashCode();
        }
    }
}