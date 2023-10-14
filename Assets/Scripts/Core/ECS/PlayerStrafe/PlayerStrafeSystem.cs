using Leopotam.EcsLite;
using UnityEngine;

public class PlayerStrafeSystem : IEcsRunSystem
{
    public void Run(IEcsSystems ecsSystems)
    {
        var filter = ecsSystems.GetWorld()
            .Filter<PlayerInputComponent>()
            .Inc<PlayerStrafeComponent>()
            .Inc<PlayerDynamicComponent>()
            .End();
        
        var playerInputPool = ecsSystems.GetWorld().GetPool<PlayerInputComponent>();
        var playerStrafePool = ecsSystems.GetWorld().GetPool<PlayerStrafeComponent>();
        var playerDynamicPool = ecsSystems.GetWorld().GetPool<PlayerDynamicComponent>();
        
        foreach (var entity in filter)
        {
            var playerCollisionsComponent = playerInputPool.Get(entity);
            ref var playerStrafeComponent = ref playerStrafePool.Get(entity);
            ref var playerDynamicComponent = ref playerDynamicPool.Get(entity);
            
            var inChangingLineState = playerStrafeComponent.RestTimer > 0;
            if (playerCollisionsComponent.StrafeDirection != MoveDirection.None
                && !inChangingLineState)
            {
                var zOffset = 0;
                if (playerCollisionsComponent.StrafeDirection == MoveDirection.Left)
                    zOffset = 1; 
                else if (playerCollisionsComponent.StrafeDirection == MoveDirection.Right)
                    zOffset = -1;

                var currentLine = Mathf.FloorToInt(playerDynamicComponent.Z);
                var desiredLine = currentLine + zOffset;
                var restDistance = Mathf.Abs(desiredLine - playerDynamicComponent.Z);
                playerStrafeComponent.PreviousZ = playerDynamicComponent.Z;
                playerStrafeComponent.DesiredZ = desiredLine;
                playerStrafeComponent.RestTime = restDistance / playerStrafeComponent.Speed;
                playerStrafeComponent.RestTimer = playerStrafeComponent.RestTime;
            }
            
            if (inChangingLineState)
            {
                playerStrafeComponent.RestTimer -= Time.fixedDeltaTime;
                var nRestTimer = Mathf.Clamp01(playerStrafeComponent.RestTimer / playerStrafeComponent.RestTime);
                
                playerStrafeComponent.Z = Mathf.Lerp(
                    playerStrafeComponent.DesiredZ,
                    playerStrafeComponent.PreviousZ,
                    nRestTimer);
                
                playerDynamicComponent.Z = playerStrafeComponent.Z;
            }
        }
    }
}