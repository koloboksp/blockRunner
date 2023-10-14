using Leopotam.EcsLite;

public sealed class PlayerDynamicSystem : IEcsRunSystem
{
    public void Run(IEcsSystems ecsSystems)
    {
         var filter = ecsSystems.GetWorld()
             .Filter<PlayerDynamicComponent>()
             .End();
         var playerDynamicPool = ecsSystems.GetWorld().GetPool<PlayerDynamicComponent>();
         
         foreach (var entity in filter)
         {
             ref var playerDynamicComponent = ref playerDynamicPool.Get(entity);
             playerDynamicComponent.PlayerRigidbody.MovePosition(playerDynamicComponent.GetPosition());
         }
    }
}