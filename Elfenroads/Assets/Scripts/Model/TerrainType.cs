namespace Models
{
    public enum TerrainType {
    Plain,
    Forest,
    Mountain,
    Desert,

    // The "rule" for streams will be to have it flow from city1 -> city2.
    // Create roads according to this rule and there shouldn't be a problem.
    Stream,
    Lake
}
}