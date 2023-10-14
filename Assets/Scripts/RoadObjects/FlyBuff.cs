using Leopotam.EcsLite;
using UnityEngine;

public class FlyBuff : MonoBehaviour, ISomething, IBuff
{
    [SerializeField] private float _time = 5;
    [SerializeField] private float _takeOffTime = 0.5f;
    [SerializeField] private float _landingTime = 0.5f;
    [SerializeField] private float _height = 4f;

    private EcsWorld _ecsWorld;
    private int _entity;
    
    public void Setup(EcsWorld ecsWorld)
    {
        _ecsWorld = ecsWorld;
    }
    
    public void Apply(IPlayer player)
    {
        var flyBuffPool = _ecsWorld.GetPool<PlayerFlyBuffComponent>();
        ref var playerFlyBuffComponent = ref flyBuffPool.Add(player.Entity);
        playerFlyBuffComponent.FlyTime = _time - _takeOffTime - _landingTime;
        playerFlyBuffComponent.TakeOffTime = _takeOffTime;
        playerFlyBuffComponent.LandingTime = _landingTime;
        playerFlyBuffComponent.Height = _height;
     
        Destroy(gameObject);
    }
}