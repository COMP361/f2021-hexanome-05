using System.Collections;
using System.Collections.Generic;
using Models;

namespace Models
{
    public class MagicSpellCounter : Counter
    {
        public SpellType spellType { private set; get; }

        public MagicSpellCounter(int id, SpellType spellType){
            this.id = id;
            this.spellType = spellType;
        }

        public enum SpellType {
            Exchange,
            Double
        }
    }
}