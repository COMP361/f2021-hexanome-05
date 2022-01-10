
// Model/Class for games


using System.Collections.Generic;
namespace Models
{
    public enum RoadType
    {
        Plain,
        Forest,
        Mountain,
        Desert,
        Stream, // The "rule" for streams will be to have it flow from city1 -> city2. Create roads according to this rule and there shouldn't be a problem.
        Lake
    }
}