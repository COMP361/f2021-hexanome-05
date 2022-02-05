using System;

namespace Models
{
    public class MagicSpellCounter : Counter
    {
        public SpellType spellType { protected set; get; }

        public MagicSpellCounter(SpellType spellType) : base() {
            this.spellType = spellType;
        }

        [Newtonsoft.Json.JsonConstructor]
        protected MagicSpellCounter(SpellType spellType, Guid id) : base(id) {
            this.spellType = spellType;
        }
    }

    public enum SpellType {
        Exchange,
        Double
    }
}