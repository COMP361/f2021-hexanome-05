using System;

namespace Models {
    public class Boot : GuidModel {
        public Color color { get; protected set; }

        public Boot(Color color) : base() {
            this.color = color;
        }

        [Newtonsoft.Json.JsonConstructor]
        protected Boot(Guid id, Color color) : base(id) {
            this.color = color;
        }
    }
}
