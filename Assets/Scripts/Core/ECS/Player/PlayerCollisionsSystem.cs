using Leopotam.EcsLite;
using UnityEngine;

public sealed class PlayerCollisionsSystem : IEcsRunSystem
{
    private static readonly RaycastHit[] noAllocResults = new RaycastHit[20];
    
    public void Run(IEcsSystems ecsSystems)
    {
        var filter = ecsSystems.GetWorld()
            .Filter<PlayerObstaclesComponent>()
            .Inc<PlayerDynamicComponent>()
            .End();
        var playerCollisionsPool = ecsSystems.GetWorld().GetPool<PlayerObstaclesComponent>();
        var playerDynamicPool = ecsSystems.GetWorld().GetPool<PlayerDynamicComponent>();

        foreach (var entity in filter)
        {
            ref var playerCollisionsComponent = ref playerCollisionsPool.Get(entity);
            var playerDynamicComponent = playerDynamicPool.Get(entity);
            
            playerCollisionsComponent.Bottom = Helpers.FindGroundCollision(playerDynamicComponent.GetPosition(), Vector3.down).exist;
            playerCollisionsComponent.Left = FindCollision(playerDynamicComponent.GetPosition(), Vector3.forward, playerDynamicComponent.PlayerCollider);
            playerCollisionsComponent.Right = FindCollision(playerDynamicComponent.GetPosition(), Vector3.back, playerDynamicComponent.PlayerCollider);
        }
    }

    private static bool FindCollision(Vector3 position, Vector3 direction, Collider ignoreCollider)
    {
        var resultCount = Physics.BoxCastNonAlloc(position, Vector3.one * Defines.ColliderHalfSize, direction, noAllocResults);
        for (int i = 0; i < resultCount; i++)
        {
            var raycastHit = noAllocResults[i];
            if(raycastHit.collider == ignoreCollider)
                continue;
            if (raycastHit.distance <= Defines.DistanceThreshold)
                return true;
        }

        return false;
    }
}