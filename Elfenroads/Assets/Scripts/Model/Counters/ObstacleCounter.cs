using System.Collections;
using System.Collections.Generic;
using Models;

namespace Models
{
    public class ObstacleCounter : Counter
    {
        public ObstacleType obstacleType { private set; get; }

        public ObstacleCounter(int id, ObstacleType obstacleType){
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