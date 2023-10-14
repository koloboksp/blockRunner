using System;
using System.Collections.Generic;
using UnityEngine;

public class RoadMapParser : MonoBehaviour
{
    [SerializeField] private Texture2D _map;
    [SerializeField] private int _width;
    [SerializeField] private int _height;

    [SerializeField] private List<ColorToItemTypeAssociation> _typesAssociations = new List<ColorToItemTypeAssociation>();
     
    public RoadData GetRoad()
    {
        return RoadDataHelpers.FromImage(_map, _width, _height, _typesAssociations);
    }
}

[Serializable]
public class ColorToItemTypeAssociation
{
    [SerializeField] private Color _color;
    [SerializeField] private int _itemType;
    public Color Color => _color;
    public int ItemType => _itemType;
}