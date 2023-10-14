using Leopotam.EcsLite;

public class PlayerTakeBonusSystem : IEcsRunSystem
{
    public void Run(IEcsSystems ecsSystems)
    {
        var filter = ecsSystems.GetWorld()
            .Filter<PlayerTakeBonusComponent>()
            .End();
        var playerTakeBonusPool = ecsSystems.GetWorld().GetPool<PlayerTakeBonusComponent>();
        var playerPool = ecsSystems.GetWorld().GetPool<PlayerComponent>();

        foreach (var entity in filter)
        {
            ref var playerTakeBonusComponent = ref playerTakeBonusPool.Get(entity);
            ref var playerComponent = ref playerPool.Get(playerTakeBonusComponent.PlayerEntity);

            playerComponent.Score += playerTakeBonusComponent.Value;
            
            playerTakeBonusPool.Del(entity);
        }
    }
}