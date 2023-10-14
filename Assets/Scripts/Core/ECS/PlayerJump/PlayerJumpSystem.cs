using Leopotam.EcsLite;
using UnityEngine;

public sealed class PlayerJumpSystem : IEcsRunSystem
{
    public void Run(IEcsSystems ecsSystems)
    {
        var filter = ecsSystems.GetWorld()
            .Filter<PlayerDynamicComponent>()
            .Inc<PlayerJumpComponent>()
            .Inc<PlayerObstaclesComponent>()
            .Inc<PlayerInputComponent>()
            .Inc<PlayerComponent>()
            .End();
        var playerDynamicPool = ecsSystems.GetWorld().GetPool<PlayerDynamicComponent>();
        var playerJumpPool = ecsSystems.GetWorld().GetPool<PlayerJumpComponent>();
        var playerCollisionsPool = ecsSystems.GetWorld().GetPool<PlayerObstaclesComponent>();
        var playerInputPool = ecsSystems.GetWorld().GetPool<PlayerInputComponent>();
        var playerPool = ecsSystems.GetWorld().GetPool<PlayerComponent>();

        foreach (var entity in filter)
        {
            ref var playerDynamicComponent = ref playerDynamicPool.Get(entity);
            ref var playerJumpComponent = ref playerJumpPool.Get(entity);
            var playerCollisionsComponent = playerCollisionsPool.Get(entity);
            var playerInputComponent = playerInputPool.Get(entity);
            var playerComponent = playerPool.Get(entity);

            if(playerComponent.Dead)
                continue;
            
            if (playerInputComponent.Jump && playerJumpComponent.Speed <= 0)
            {
                playerJumpComponent.Speed += playerJumpComponent.JumpSpeed;
            }
            else
            {
                if (playerDynamicComponent.IsFlying 
                    || (playerCollisionsComponent.Bottom && playerJumpComponent.Speed <= 0))
                {
                    playerJumpComponent.Speed = 0;
                }
                else
                {
                    playerJumpComponent.Speed += Time.fixedDeltaTime * Physics.gravity.y;
                }
            }
            
            playerDynamicComponent.Y += playerJumpComponent.Speed * Time.fixedDeltaTime;
        }
    }
}