using Models;
using System;

namespace Models {
    /// <summary>
    /// Model with a GUID
    /// </summary>
    public abstract class GuidModel : IEquatable<GuidModel> {
        public Guid id { private set; get; }

        protected GuidModel() {
            this.id = new Guid();
        }

        protected GuidModel(Guid id) {
            this.id = id;
        }

        public bool Equals(GuidModel other) {
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