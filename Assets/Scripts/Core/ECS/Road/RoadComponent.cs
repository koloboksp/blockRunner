using System.Collections.Generic;
using Leopotam.EcsLite;
using UnityEngine;

public struct RoadComponent : IEcsAutoReset<RoadComponent>
{
    public List<Row> CreatedRows;
    
    public void AutoReset(ref RoadComponent c)
    {
        c.CreatedRows = new List<Row>();
    }
}

public class Row
{
    public int RowIndex;
    public readonly List<GameObject> CreatedObjects = new();
}