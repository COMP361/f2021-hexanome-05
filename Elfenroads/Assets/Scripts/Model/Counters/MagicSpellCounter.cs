using System.Collections;
using System.Collections.Generic;
using Models;
using System;

namespace Models
{
    public class MagicSpellCounter : Counter
    {
        public SpellType spellType { set; get; }

        public MagicSpellCounter(Guid id, SpellType spellType){
            this.id = id;
            this.spellType = spellType;
        }
    }

    public enum SpellType {
            Exchange,
            Double
        }
}