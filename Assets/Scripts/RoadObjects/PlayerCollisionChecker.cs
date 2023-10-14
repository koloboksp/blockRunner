using Leopotam.EcsLite;
using UnityEngine;

public class PlayerCollisionChecker : MonoBehaviour
{
    [SerializeField] private Player _player;
    public EcsWorld EcsWorld { get; set; }
    
    private void OnTriggerEnter(Collider other)
    {
        var playerHit = EcsWorld.NewEntity();
        var playerHitPool = EcsWorld.GetPool<PlayerHitComponent>();
        playerHitPool.Add(playerHit);
        ref var playerHitComponent = ref playerHitPool.Get(playerHit);

        playerHitComponent.Entity = playerHit;
        playerHitComponent.Player = _player;
        playerHitComponent.Other = other.gameObject.transform.parent.gameObject.GetComponent<ISomething>();
    }
}