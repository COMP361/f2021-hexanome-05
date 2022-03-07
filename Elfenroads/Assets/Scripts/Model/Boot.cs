using System;

namespace Models {
    public class Boot : GuidModel {
        public Color color { get; protected set; }
        public string currentTown_id { get; protected set; }

        public Boot(Color color) : base() {
            this.color = color;
        }

        [Newtonsoft.Json.JsonConstructor]
        protected Boot(Guid id, Color color, string currentTown_id) : base(id) {
            this.color = color;
            this.currentTown_id = currentTown_id;
        }
    }
}
