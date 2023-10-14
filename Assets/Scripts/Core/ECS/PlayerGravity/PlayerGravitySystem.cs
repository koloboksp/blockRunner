using Leopotam.EcsLite;
using UnityEngine;

public sealed class PlayerGravitySystem : IEcsRunSystem
{
    public void Run(IEcsSystems ecsSystems)
    {
        var filter = ecsSystems.GetWorld()
            .Filter<PlayerDynamicComponent>()
            .Inc<PlayerGravityComponent>()
            .End();
        var playerDynamicPool = ecsSystems.GetWorld().GetPool<PlayerDynamicComponent>();
        var playerGravityPool = ecsSystems.GetWorld().GetPool<PlayerGravityComponent>();

        foreach (var entity in filter)
        {
            ref var playerDynamicComponent = ref playerDynamicPool.Get(entity);
            ref var playerGravityComponent = ref playerGravityPool.Get(entity);

            if (playerDynamicComponent.IsFlying)
                continue;
            
            var predictSpeed = playerGravityComponent.Speed + Time.fixedDeltaTime * Physics.gravity.y;
            var predictOffset = predictSpeed * Time.fixedDeltaTime;

            var speed = predictSpeed;
            if (MoveWithPenetrationCheck(playerDynamicComponent.GetPosition(), predictOffset, ref playerDynamicComponent.Y))
                speed = 0;
            
            playerGravityComponent.Speed = speed;
        }
    }

    public static bool MoveWithPenetrationCheck(Vector3 position, float predictOffset, ref float resultPosition)
    {
        var collisionResult = Helpers.FindGroundCollision(
            position + new Vector3(0, predictOffset, 0), 
            Vector3.down);

        var offset = predictOffset;
        if (collisionResult.exist)
            offset = Defines.GroundContactThreshold - collisionResult.distance + predictOffset;
        resultPosition += offset;

        return collisionResult.exist;
    }
}