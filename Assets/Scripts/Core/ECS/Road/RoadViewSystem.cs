using Leopotam.EcsLite;

public sealed class RoadViewSystem : IEcsRunSystem
{
    public void Run(IEcsSystems ecsSystems)
    {
        var filter = ecsSystems.GetWorld()
            .Filter<RoadViewerComponent>()
            .Inc<PlayerDynamicComponent>()
            .End();

        var roadViewerPool = ecsSystems.GetWorld().GetPool<RoadViewerComponent>();
        var playerDynamicPool = ecsSystems.GetWorld().GetPool<PlayerDynamicComponent>();
        
        foreach (var entity in filter)
        {
            ref var roadViewerComponent = ref roadViewerPool.Get(entity);
            var playerDynamicComponent = playerDynamicPool.Get(entity);

            roadViewerComponent.Position = playerDynamicComponent.GetPosition();
        }
    }
}