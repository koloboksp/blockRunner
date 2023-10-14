using System;
using System.Collections.Generic;
using System.Linq;
using Leopotam.EcsLite;
using UnityEngine;
using Object = UnityEngine.Object;

public sealed class RoadSystem : IEcsRunSystem
{
    private readonly List<ISomething> _noAllocISomethingResult = new List<ISomething>();

    private readonly RoadData _roadData;
    private readonly RoadItemsDescription _roadItemsDescription;
    private readonly GameObject _roadInstance;
    
    public RoadSystem(RoadData roadData, RoadItemsDescription roadItemsDescription, GameObject roadInstance)
    {
        _roadData = roadData;
        _roadItemsDescription = roadItemsDescription;
        _roadInstance = roadInstance;
    }
    
    public void Run(IEcsSystems ecsSystems)
    {
        var filterViewers = ecsSystems.GetWorld()
            .Filter<RoadViewerComponent>()
            .End();
        var filterRoads = ecsSystems.GetWorld()
            .Filter<RoadComponent>()
            .End();
        
        var roadPool = ecsSystems.GetWorld().GetPool<RoadComponent>();
        var roadViewerPool = ecsSystems.GetWorld().GetPool<RoadViewerComponent>();
        
        var all = new List<int>();
        var needToCreate = new List<int>();
        var needToDestroy = new List<int>();
        
        foreach (var roadEntity in filterRoads)
        {
            ref var road = ref roadPool.Get(roadEntity);
            foreach (var viewerEntity in filterViewers)
            {
                ref var roadViewer = ref roadViewerPool.Get(viewerEntity);

                var worldViewRange = new Vector2(
                    roadViewer.Position.x + roadViewer.ViewRange.x, 
                    roadViewer.Position.x + roadViewer.ViewRange.y);
                var worldViewRangeInt = new Vector2Int(
                    Mathf.FloorToInt(worldViewRange.x), 
                    Mathf.CeilToInt(worldViewRange.y));
                var fieldRange = new Vector2Int(
                    Mathf.Max(0, worldViewRangeInt.x),
                    Mathf.Min(worldViewRangeInt.y, _roadData.Rows.Count));

                for (var x = fieldRange.x; x < fieldRange.y; x++)
                {
                    if (!all.Contains(x))
                        all.Add(x);
                }
            }

            FillRowsNeedToCreate(road, all, needToCreate);
            FillRowsNeedToDestroy(road, all, needToDestroy);
           
            DestroyRows(needToDestroy, road);
            CreateRows(ecsSystems, needToCreate, road);
        }
    }
    
    private static void FillRowsNeedToCreate(RoadComponent road, List<int> all, List<int> needToCreate)
    {
        for (var i = 0; i < all.Count; i++)
        {
            var allRows = all[i];
            var foundRow = road.CreatedRows.Find(i => i.RowIndex == allRows);
            if (foundRow == null)
            {
                if (!needToCreate.Contains(allRows))
                    needToCreate.Add(allRows);
            }
        }
    }
    
    private static void FillRowsNeedToDestroy(RoadComponent road, List<int> all, List<int> needToDestroy)
    {
        for (int i = 0; i < road.CreatedRows.Count; i++)
        {
            var fieldCreatedRow = road.CreatedRows[i];
            if (!all.Contains(fieldCreatedRow.RowIndex))
                if (!needToDestroy.Contains(fieldCreatedRow.RowIndex))
                    needToDestroy.Add(fieldCreatedRow.RowIndex);
        }
    }
    
    private static void DestroyRows(List<int> needToDestroy, RoadComponent road)
    {
        for (int i = 0; i < needToDestroy.Count; i++)
        {
            var rowIndex = needToDestroy[i];
            var foundRow = road.CreatedRows.Find(item => item.RowIndex == rowIndex);
            foreach (var gameObject in foundRow.CreatedObjects)
                Object.Destroy(gameObject.gameObject);

            road.CreatedRows.Remove(foundRow);
        }
    }
    
    private void CreateRows(IEcsSystems ecsSystems, List<int> needToCreate, RoadComponent road)
    {
        for (int i = 0; i < needToCreate.Count; i++)
        {
            var rowIndex = needToCreate[i];
            var newRow = new Row();
            newRow.RowIndex = rowIndex;
            road.CreatedRows.Add(newRow);
            var roadDataRow = _roadData.Rows[rowIndex];

            for (int z = 0; z < roadDataRow.Items.GetLength(1); z++)
            for (int y = 0; y < roadDataRow.Items.GetLength(0); y++)
            {
                var itemIndex = new Vector3Int(rowIndex, y, z);
                var itemType = roadDataRow.Items[y, z];
                var roadItemDescription = _roadItemsDescription.ItemDescriptions.FirstOrDefault(i => i.Type == itemType);
                
                if (roadItemDescription != null && roadItemDescription.Prefab != null)
                {
                    var itemObj = Object.Instantiate(roadItemDescription.Prefab, itemIndex, Quaternion.identity, _roadInstance.transform);
                    newRow.CreatedObjects.Add(itemObj);
                    
                    ProcessSomethingItems(itemObj);
                }
            }
        }

        void ProcessSomethingItems(GameObject itemObj)
        {
            itemObj.GetComponents(_noAllocISomethingResult);
            for (var somethingI = 0; somethingI < _noAllocISomethingResult.Count; somethingI++)
            {
                var something = _noAllocISomethingResult[somethingI];
                something.Setup(ecsSystems.GetWorld());
            }
        }
    }
}