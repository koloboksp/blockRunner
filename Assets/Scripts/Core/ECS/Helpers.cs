using System.Collections.Generic;
using UnityEngine;

public static class Helpers
{
    private static readonly RaycastHit[] noAllocResults = new RaycastHit[20];
    private static readonly List<ColliderOwner> noAllocColliderOwnersReuslts = new List<ColliderOwner>();

    public static (bool exist, float distance) FindGroundCollision(Vector3 position, Vector3 direction)
    { 
        var resultCount = Physics.BoxCastNonAlloc(
            position, 
            new Vector3(Defines.ColliderWidthHalfSize, Defines.ColliderHalfSize, Defines.ColliderHalfSize), 
            direction, 
            noAllocResults);

        var minDistance = float.MaxValue;
        var iOfMinDistance = -1;
        for (var i = 0; i < resultCount; i++)
        {
            var raycastHit = noAllocResults[i];

            raycastHit.collider.gameObject.gameObject.GetComponents(noAllocColliderOwnersReuslts);
            if (noAllocColliderOwnersReuslts.Count <= 0) 
                continue;
            if (noAllocColliderOwnersReuslts[0].Owner is not IGround) 
                continue;
            if (raycastHit.distance >= minDistance)
                continue;
                    
            iOfMinDistance = i;
            minDistance = raycastHit.distance;
        }

        if (iOfMinDistance >= 0)
        {
            if(minDistance <= Defines.GroundContactThreshold)
                return (true, noAllocResults[iOfMinDistance].distance);

            return (false, noAllocResults[iOfMinDistance].distance);
        }

        return (false, 0.0f);
    }
}