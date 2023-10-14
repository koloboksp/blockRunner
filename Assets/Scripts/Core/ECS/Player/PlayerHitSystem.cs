using Leopotam.EcsLite;
using UnityEngine;

public sealed class PlayerHitSystem : IEcsRunSystem
{
    public void Run(IEcsSystems ecsSystems)
    {
        var filter = ecsSystems.GetWorld()
            .Filter<PlayerHitComponent>()
            .End();
        
        var playerHitPool = ecsSystems.GetWorld().GetPool<PlayerHitComponent>();
        
        foreach (var entity in filter)
        {
            var playerHitComponent = playerHitPool.Get(entity);
            
            if (playerHitComponent.Other is IDamageDealer damageDealer)
            {
                playerHitComponent.Player.Die();
            }
            else
            {
                if (playerHitComponent.Other is IBuff buff)
                {
                    buff.Apply(playerHitComponent.Player);
                }
                else if (playerHitComponent.Other is IBonus bonus)
                {
                    bonus.Apply(playerHitComponent.Player);
                }
            }
            
            playerHitPool.Del(playerHitComponent.Entity);
        }
    }
}