using Leopotam.EcsLite;
using UnityEngine;

public sealed class PlayerRunSystem : IEcsRunSystem
{
    public void Run(IEcsSystems ecsSystems)
    {
        var filter = ecsSystems.GetWorld()
            .Filter<PlayerInputComponent>()
            .Inc<PlayerRunComponent>()
            .Inc<PlayerDynamicComponent>()
            .Inc<PlayerComponent>()
            .End();
        
        var playerRunPool = ecsSystems.GetWorld().GetPool<PlayerRunComponent>();
        var playerDynamicPool = ecsSystems.GetWorld().GetPool<PlayerDynamicComponent>();
        var playerPool = ecsSystems.GetWorld().GetPool<PlayerComponent>();

        foreach (var entity in filter)
        {
            ref var playerRunComponent = ref playerRunPool.Get(entity);
            ref var playerDynamicComponent = ref playerDynamicPool.Get(entity);
            var playerComponent = playerPool.Get(entity);

            if (playerComponent.Dead) 
                continue;
            
            playerDynamicComponent.X += playerRunComponent.Speed * Time.fixedDeltaTime;
        }
    }
}