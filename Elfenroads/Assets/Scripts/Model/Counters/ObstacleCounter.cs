using System.Collections;
using System.Collections.Generic;
using Models;
using System;

namespace Models
{
    public class ObstacleCounter : Counter
    {
        public ObstacleType obstacleType { set; get; }

        public ObstacleCounter(Guid id, ObstacleType obstacleType){
            this.id = id;
            this.obstacleType = obstacleType;
        }
    }

    public enum ObstacleType
    {
        Land,
        Sea
    }
}