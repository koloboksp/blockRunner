using System;
using System.Collections.Generic;
using UnityEngine;

public class RoadItemsDescription : MonoBehaviour
{
    [SerializeField] private List<RoadItemDescription> _itemDescriptions = new List<RoadItemDescription>();

    public IReadOnlyList<RoadItemDescription> ItemDescriptions => _itemDescriptions;
}

[Serializable]
public class RoadItemDescription
{
    [SerializeField] private string _tag;
    [SerializeField] private int _type;
    [SerializeField] private GameObject _prefab;

    public string Tag => _tag;
    public int Type => _type;
    public GameObject Prefab => _prefab;
}