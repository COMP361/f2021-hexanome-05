using System.Collections;
using System.Collections.Generic;
using System;
using Models;
using Newtonsoft.Json;


namespace Models
{
    public class Counter : IEquatable<Counter>
    {
        public Guid id { set; get; }

        public bool Equals(Counter other) {
            return other != null &&
                    other.id == this.id;
        }

        public override bool Equals(object obj)
        {   
            return Equals(obj as Counter);
        }

        // public override Guid GetHashCode()
        // {
        //     return id;
        // }
    }
} 