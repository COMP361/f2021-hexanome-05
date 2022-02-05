using System;

namespace Models
{
    public class GoldCounter : Counter
    {
        public GoldCounter() : base() {}

        [Newtonsoft.Json.JsonConstructor]
        protected GoldCounter(Guid id) : base(id) {}
    }
}