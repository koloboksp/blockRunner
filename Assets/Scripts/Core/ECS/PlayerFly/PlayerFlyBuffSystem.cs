using Leopotam.EcsLite;
using UnityEngine;

public sealed class PlayerFlyBuffSystem : IEcsRunSystem
{
    public void Run(IEcsSystems ecsSystems)
    {
        var filter = ecsSystems.GetWorld()
            .Filter<PlayerDynamicComponent>()
            .Inc<PlayerFlyBuffComponent>()
            .Inc<PlayerObstaclesComponent>()
            .End();
        
        var playerDynamicPool = ecsSystems.GetWorld().GetPool<PlayerDynamicComponent>();
        var playerFlyBuffPool = ecsSystems.GetWorld().GetPool<PlayerFlyBuffComponent>();
        
        foreach (var entity in filter)
        {
            ref var playerDynamicComponent = ref playerDynamicPool.Get(entity);
            ref var playerFlyBuffComponent = ref playerFlyBuffPool.Get(entity);
          
            switch (playerFlyBuffComponent.State)
            {
                case FlyState.None:
                    playerFlyBuffComponent.State = FlyState.TakingOff;
                    playerFlyBuffComponent.StartHeight = playerDynamicComponent.Y;
                    playerDynamicComponent.IsFlying = true;
                    break;
                case FlyState.TakingOff:
                    ProcessTakeOff(ref playerFlyBuffComponent, ref playerDynamicComponent);
                    break;
                case FlyState.Fly:
                    ProcessFly(ref playerFlyBuffComponent, ref playerDynamicComponent);
                    break;
                case FlyState.Landing:
                    ProcessLanding(ref playerFlyBuffComponent, ref playerDynamicComponent, playerFlyBuffPool, entity);
                    break;
            }
        }
    }
    
    private static void ProcessTakeOff(
        ref PlayerFlyBuffComponent playerFlyBuffComponent, 
        ref PlayerDynamicComponent playerDynamicComponent)
    {
        
        if (playerFlyBuffComponent.RestTimer < playerFlyBuffComponent.TakeOffTime)
        {
            playerFlyBuffComponent.RestTimer += Time.fixedDeltaTime;
            var nTimer = Mathf.Clamp01(playerFlyBuffComponent.RestTimer / playerFlyBuffComponent.TakeOffTime);

            playerDynamicComponent.Y = Mathf.Lerp(playerFlyBuffComponent.StartHeight, playerFlyBuffComponent.Height, nTimer);
        }
        else
        {
            playerFlyBuffComponent.State = FlyState.Fly;
            playerFlyBuffComponent.RestTimer = 0.0f;
        }
    }
    
    private static void ProcessFly(
        ref PlayerFlyBuffComponent playerFlyBuffComponent,
        ref PlayerDynamicComponent playerDynamicComponent)
    {
        if (playerFlyBuffComponent.RestTimer < playerFlyBuffComponent.FlyTime)
        {
            playerFlyBuffComponent.RestTimer += Time.fixedDeltaTime;
            playerDynamicComponent.Y = playerFlyBuffComponent.Height;
        }
        else
        {
            playerFlyBuffComponent.State = FlyState.Landing;
            playerFlyBuffComponent.RestTimer = 0.0f;
        }
    }
    
    private static void ProcessLanding(
        ref PlayerFlyBuffComponent playerFlyBuffComponent, 
        ref PlayerDynamicComponent playerDynamicComponent,
        EcsPool<PlayerFlyBuffComponent> playerFlyBuffPool, 
        int entity)
    {
        if (playerFlyBuffComponent.RestTimer < playerFlyBuffComponent.FlyTime)
        {
            playerFlyBuffComponent.RestTimer += Time.fixedDeltaTime;
            var nTimer = playerFlyBuffComponent.RestTimer / playerFlyBuffComponent.TakeOffTime;
            var newPosition = Mathf.LerpUnclamped(playerFlyBuffComponent.Height, playerFlyBuffComponent.StartHeight, nTimer);
            var offset = newPosition - playerDynamicComponent.Y;

            if (PlayerGravitySystem.MoveWithPenetrationCheck(playerDynamicComponent.GetPosition(), offset, ref playerDynamicComponent.Y))
            {
                playerDynamicComponent.IsFlying = false;
                playerFlyBuffComponent.State = FlyState.None;
                playerFlyBuffPool.Del(entity);
            }
        }
        else
        {
            playerDynamicComponent.IsFlying = false;
            playerFlyBuffComponent.State = FlyState.None;
            playerFlyBuffPool.Del(entity);
        }
    }
}