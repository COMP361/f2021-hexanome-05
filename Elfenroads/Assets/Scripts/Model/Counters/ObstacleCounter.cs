using System.Collections;
using System.Collections.Generic;
using Models;
using System;

namespace Models
{
    public class ObstacleCounter : Counter
    {
        public ObstacleType obstacleType { protected set; get; }

        public ObstacleCounter(ObstacleType obstacleType) : base() {
            this.obstacleType = obstacleType;
        }

        [Newtonsoft.Json.JsonConstructor]
        protected ObstacleCounter(ObstacleType obstacleType, Guid id) : base(id) {
            this.obstacleType = obstacleType;
        }
    }

    public enum ObstacleType
    {
        Land,
        Sea
    }
}