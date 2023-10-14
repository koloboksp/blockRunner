using Leopotam.EcsLite;
using UnityEngine;

public sealed class PlayerRunBuffSystem : IEcsRunSystem
{
    public void Run(IEcsSystems ecsSystems)
    {
        var filter = ecsSystems.GetWorld()
            .Filter<PlayerRunBuffComponent>()
            .Inc<PlayerRunComponent>()
            .End();
        
        var playerRunPool = ecsSystems.GetWorld().GetPool<PlayerRunComponent>();
        var playerRunBuffPool = ecsSystems.GetWorld().GetPool<PlayerRunBuffComponent>();
        
        foreach (var entity in filter)
        {
            ref var playerRunComponent = ref playerRunPool.Get(entity);
            ref var playerRunBuffComponent = ref playerRunBuffPool.Get(entity);
            
            if (!playerRunBuffComponent.Started)
            {
                playerRunBuffComponent.Started = true;
                playerRunComponent.Speed += playerRunBuffComponent.Speed;
                playerRunBuffComponent.RestTimer = playerRunBuffComponent.RestTime;
            }
            else
            {
                if (playerRunBuffComponent.RestTimer > 0)
                {
                    playerRunBuffComponent.RestTimer -= Time.fixedDeltaTime;
                }
                else
                {
                    playerRunBuffComponent.Started = false;
                    playerRunComponent.Speed -= playerRunBuffComponent.Speed;
                    playerRunBuffPool.Del(entity);
                }
            }
        }
    }
}