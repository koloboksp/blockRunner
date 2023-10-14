using System.Collections.Generic;
using UnityEngine;

public class RoadData
{
    private readonly List<RoadRowData> _rows = new List<RoadRowData>();
    
    public List<RoadRowData> Rows
    {
        get => _rows;
    }

    public List<Vector3Int> GetPosition(int itemType)
    {
        var result = new List<Vector3Int>();
        
        for (int x = 0; x < _rows.Count; x++)
        {
            var roadRowData = _rows[x];
            var items = roadRowData.Items;
            for (int y = 0; y < items.GetLength(0); y++)
            {
                for (int z = 0; z < items.GetLength(1); z++)
                {
                    if (items[y, z] == itemType)
                    {
                        result.Add(new Vector3Int(x,y,z));
                    }
                } 
            }
        }

        return result;
    }
}

public class RoadRowData
{
    private int[,] _items;
    
    public int[,] Items
    {
        get => _items;
        set => _items = value;
    }
}