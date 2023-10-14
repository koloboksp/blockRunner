using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class RoadDataHelpers
{
    public static RoadData FromImage(Texture2D input, int roadWidth, int roadHeight, List<ColorToItemTypeAssociation> colorToItemTypeAssociations)
    {
        var data = new RoadData();
        
        for (int x = 0; x < input.width; x++)
        {
            var items = new int[roadHeight, roadWidth];
            for (int y = 0; y < roadHeight; y++)
            {
                for (int z = 0; z < roadWidth; z++)
                {
                    var pixelI = new Vector2Int(x, (roadWidth + 1) * y + z);
                    var color = input.GetPixel(pixelI.x, pixelI.y);
                    var association = colorToItemTypeAssociations.FirstOrDefault(i => i.Color == color);
                    if (association == null)
                    {
                        Debug.LogError($"Association for color {color} not found.");
                    }
                    else
                    {
                        items[y, z] = association.ItemType;
                    }
                    
                }
            }
            
            var roadRowData = new RoadRowData();
            data.Rows.Add(roadRowData);
            roadRowData.Items = items;
        }

        return data;
    }
    
    public static Color32 ConvertColor(int iColor)
    {
        Color32 c = new Color32();
        c.b = (byte)((iColor) & 0xFF);
        c.g = (byte)((iColor>>8) & 0xFF);
        c.r = (byte)((iColor>>16) & 0xFF);
        c.a = (byte)((iColor>>24) & 0xFF);
        return c;
    }
    
    public static int ConvertColor(Color32 color)
    {
        int c = 0;
        c += color.b;
        c += color.g << 8;
        c += color.r << 16;
        c += color.r << 24;
        return c;
    }
}