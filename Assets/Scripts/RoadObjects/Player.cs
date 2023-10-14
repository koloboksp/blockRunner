using Leopotam.EcsLite;
using UnityEngine;

public class Player : MonoBehaviour, IPlayer
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private Collider _collider;
    [SerializeField] private PlayerCollisionChecker _playerCollisionChecker;

    [SerializeField] private float _strafeSpeed = 5.0f;
    [SerializeField] private float _runSpeed = 3.0f;
    [SerializeField] private float _jumpSpeed = 10.0f;

    [SerializeField] private Vector2Int _viewRange = new (-3, 7);
    
    private EcsWorld _ecsWorld;
    private int _entity;

    public Rigidbody Rigidbody => _rigidbody;
    public Collider Collider => _collider;

    public void Setup(EcsWorld ecsWorld)
    {
        _ecsWorld = ecsWorld;
        
        _entity = _ecsWorld.NewEntity();
        _playerCollisionChecker.EcsWorld = _ecsWorld;

        SetupInput();
        SetupFieldViewer();
        SetupObstacles();
        SetupMovable();
        SetupPose();
        SetupGravity();
        SetupPlayer();
    }

    private void OnDestroy()
    {
        DestroyPlayer();
        DestroyGravity();
        DestroyPose();
        DestroyMovable();
        DestroyObstacles();
        DestroyFieldViewer();
        DestroyInput();
    }
    
    private void SetupPose()
    {
        var playerDynamicPool = _ecsWorld.GetPool<PlayerDynamicComponent>();
        ref var playerDynamicComponent = ref playerDynamicPool.Add(_entity);
        playerDynamicComponent.PlayerRigidbody = _rigidbody;
        playerDynamicComponent.PlayerCollider = _collider;
        playerDynamicComponent.ResetPosition(transform.position);
    }

    private void SetupGravity()
    {
        var playerGravityPool = _ecsWorld.GetPool<PlayerGravityComponent>();
        playerGravityPool.Add(_entity);
    }

    private void SetupPlayer()
    {
        var pool = _ecsWorld.GetPool<PlayerComponent>();
        pool.Add(_entity);
    }
    
    private void SetupMovable()
    {
        var playerStrafeComponentPool = _ecsWorld.GetPool<PlayerStrafeComponent>();
        ref var playerStrafeComponent = ref playerStrafeComponentPool.Add(_entity);
        playerStrafeComponent.Speed = _strafeSpeed;
        
        var playerJumpPool = _ecsWorld.GetPool<PlayerJumpComponent>();
        ref var playerJumpComponent = ref playerJumpPool.Add(_entity);
        playerJumpComponent.JumpSpeed = _jumpSpeed;
    }

    private void SetupObstacles()
    {
        var playerObstaclesComponentPool = _ecsWorld.GetPool<PlayerObstaclesComponent>();
        playerObstaclesComponentPool.Add(_entity);
    }

    private void SetupFieldViewer()
    {
        var fieldViewerComponentPool = _ecsWorld.GetPool<RoadViewerComponent>();
        ref var fieldViewerComponentComponent = ref fieldViewerComponentPool.Add(_entity);
        fieldViewerComponentComponent.ViewRange = _viewRange;
    }
    
    private void SetupInput()
    {
        var playerInputPool = _ecsWorld.GetPool<PlayerInputComponent>();
        playerInputPool.Add(_entity);
    }
    
    private void DestroyPlayer()
    {
        var pool = _ecsWorld.GetPool<PlayerComponent>();
        pool.Del(_entity);
    }
    
    private void DestroyGravity()
    {
        var pool = _ecsWorld.GetPool<PlayerGravityComponent>();
        pool.Del(_entity);
    }
    
    private void DestroyPose()
    {
        var pool = _ecsWorld.GetPool<PlayerDynamicComponent>();
        pool.Del(_entity);
    }

    private void DestroyMovable()
    {
        var playerStrafeComponentPool = _ecsWorld.GetPool<PlayerStrafeComponent>();
        playerStrafeComponentPool.Del(_entity);
       
        var playerRunPool = _ecsWorld.GetPool<PlayerRunComponent>();
        playerRunPool.Del(_entity);

        var playerJumpPool = _ecsWorld.GetPool<PlayerJumpComponent>();
        playerJumpPool.Del(_entity);
    }

    private void DestroyObstacles()
    {
        var pool = _ecsWorld.GetPool<PlayerObstaclesComponent>();
        pool.Del(_entity);
    }

    private void DestroyFieldViewer()
    {
        var pool = _ecsWorld.GetPool<RoadViewerComponent>();
        pool.Del(_entity);
    }
    
    private void DestroyInput()
    {
        var pool = _ecsWorld.GetPool<PlayerInputComponent>();
        pool.Del(_entity);
    }

    public void Run()
    {
        var playerRunPool = _ecsWorld.GetPool<PlayerRunComponent>();
        ref var playerRunComponent = ref playerRunPool.Add(_entity);
        playerRunComponent.Speed = _runSpeed;
    }
    
    
    public GameObject Owner => gameObject;
    public int Entity => _entity;

    public void Die()
    {
        var playerPool = _ecsWorld.GetPool<PlayerComponent>();
        ref var playerComponent = ref playerPool.Get(_entity);
        playerComponent.Dead = true;
    }

    public bool IsAlive()
    {
        var playerPool = _ecsWorld.GetPool<PlayerComponent>();
        var playerComponent = playerPool.Get(_entity);
        return !playerComponent.Dead;
    }
    
    public int Score()
    {
        var playerPool = _ecsWorld.GetPool<PlayerComponent>();
        var playerComponent = playerPool.Get(_entity);
        return playerComponent.Score;
    }
}