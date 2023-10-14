using Leopotam.EcsLite;
using UnityEngine;

public class SpeedBuff : MonoBehaviour, ISomething, IBuff
{
    [SerializeField] private float _speed = 1;
    [SerializeField] private float _time = 5;

    private EcsWorld _ecsWorld;
    private int _entity;
    
    public void Setup(EcsWorld ecsWorld)
    {
        _ecsWorld = ecsWorld;
    }
    
    public void Apply(IPlayer player)
    {
        var runBuffPool = _ecsWorld.GetPool<PlayerRunBuffComponent>();
        ref var playerRunBuffComponent = ref runBuffPool.Add(player.Entity);
        playerRunBuffComponent.Speed = _speed;
        playerRunBuffComponent.RestTime = _time;
        
        Destroy(gameObject);
    }
}