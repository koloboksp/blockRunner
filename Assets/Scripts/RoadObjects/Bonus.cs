using Leopotam.EcsLite;
using UnityEngine;

public class Bonus : MonoBehaviour, ISomething, IBonus
{
    [SerializeField] private int _value = 5;
    
    private EcsWorld _ecsWorld;
    
    public void Apply(IPlayer player)
    {
        var entity = _ecsWorld.NewEntity();
        var playerTakeBonusPool = _ecsWorld.GetPool<PlayerTakeBonusComponent>();
        ref var playerTakeBonusComponent = ref playerTakeBonusPool.Add(entity);
        playerTakeBonusComponent.PlayerEntity = player.Entity;
        playerTakeBonusComponent.Value = _value;
     
        Destroy(gameObject);
    }

    public void Setup(EcsWorld ecsWorld)
    {
        _ecsWorld = ecsWorld;
    }
}