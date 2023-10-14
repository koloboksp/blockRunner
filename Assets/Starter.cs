using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Leopotam.EcsLite;
using UnityEngine;

public class Starter : MonoBehaviour
{
    [SerializeField] private RoadMapParser _roadMapParser;
    [SerializeField] private RoadItemsDescription _roadItemsDescription;
    [SerializeField] private GameObject _roadPrefab;
    [SerializeField] private CameraController _cameraController;

    [SerializeField] private UIController _uiController;
    
    private EcsWorld _ecsWorld;
    private IEcsSystems _updateSystems;
    private IEcsSystems _lateUpdateSystems;
    private IEcsSystems _fixedUpdateSystems;
    private CancellationTokenSource _cancellationTokenSource;
    private Player _playerInstance;
    private GameObject _roadInstance;

    private async void Start()
    {
        Application.targetFrameRate = 60;
        
        _cancellationTokenSource = new CancellationTokenSource();
        
        try
        {
            await ProcessGame(true, _cancellationTokenSource.Token);
        }
        catch (OperationCanceledException e)
        {
            Debug.Log(e);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private async Task ProcessGame(bool firstSession, CancellationToken cancellationToken)
    {
        await ClearScene();
        CreateScene();
        
        if(firstSession)
            await _uiController.StartPanel.Show(_cancellationTokenSource.Token);

        _uiController.GamePanel.Show(_playerInstance);
        
        _playerInstance.Run();
        
        while (true)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            if(!_playerInstance.IsAlive())
                break;
            if(_uiController.GamePanel.RestartRequested)
                break;
                
            await Task.Yield();
        }

        _uiController.GamePanel.Hide();
        
        await _uiController.RestartPanel.Show(_playerInstance.Score(), _cancellationTokenSource.Token);

        await ProcessGame(false, cancellationToken);
    }
    
    private void OnDestroy()
    {
        if (_cancellationTokenSource != null)
        {
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
            _cancellationTokenSource = null;
        }
    }

    private void CreateScene()
    {
        _ecsWorld = new EcsWorld();
        
        var roadData = _roadMapParser.GetRoad();
        
        CreateRoad(_ecsWorld);
        CreatePlayer(_ecsWorld, roadData);
        LinkCamera(_ecsWorld);

        _updateSystems = new EcsSystems(_ecsWorld)
            .Add(new PlayerInputSystem());
        _updateSystems.Init();
        
        _lateUpdateSystems = new EcsSystems(_ecsWorld)
            .Add(new CameraSystem());
        _lateUpdateSystems.Init();
        
        _fixedUpdateSystems = new EcsSystems(_ecsWorld)
            .Add(new PlayerCollisionsSystem())
            .Add(new PlayerStrafeSystem())
            .Add(new PlayerJumpSystem()) 
            .Add(new PlayerGravitySystem())
            .Add(new PlayerRunSystem())
            //move buffs placement 
            .Add(new PlayerFlyBuffSystem())
            .Add(new PlayerRunBuffSystem())
            .Add(new PlayerTakeBonusSystem())
            //move buffs placement 
            .Add(new PlayerDynamicSystem())
            .Add(new RoadViewSystem())
            .Add(new RoadSystem(roadData, _roadItemsDescription, _roadInstance))
            .Add(new PlayerHitSystem());
        _fixedUpdateSystems.Init();
    }

    private async Task ClearScene()
    {
        if (_cameraController != null)
        {
            _cameraController.Clear();
        }
        if (_playerInstance != null)
        {
            Destroy(_playerInstance.gameObject);
            _playerInstance = null;
        }
          
        if (_roadInstance != null)
        {
            Destroy(_roadInstance.gameObject);
            _roadInstance = null;
        }

        await Task.Yield();
        
        if (_updateSystems != null)
        {
            _updateSystems.Destroy();
            _updateSystems = null;
        }

        if (_fixedUpdateSystems != null)
        {
            _fixedUpdateSystems.Destroy();
            _fixedUpdateSystems = null;
        }

        if (_ecsWorld != null)
        {
            _ecsWorld.Destroy();
            _ecsWorld = null;
        }
    }
    
    private void CreateRoad(EcsWorld ecsWorld)
    {
        if (_roadPrefab == null)
            throw new ArgumentNullException(nameof(_roadPrefab));
        _roadInstance = GameObject.Instantiate(_roadPrefab);
        
        var roadEntity = ecsWorld.NewEntity();
        var roadPool = ecsWorld.GetPool<RoadComponent>();
        roadPool.Add(roadEntity);
        ref var roadComponent = ref roadPool.Get(roadEntity);
    }

    private void CreatePlayer(EcsWorld ecsWorld, RoadData roadData)
    {
        var playerRespawnDescription = _roadItemsDescription.ItemDescriptions.FirstOrDefault(i => i.Tag == "playerRespawn");
        if (playerRespawnDescription == null)
            throw new ArgumentNullException(nameof(playerRespawnDescription));
        
        var playerDescription = _roadItemsDescription.ItemDescriptions.FirstOrDefault(i => i.Tag == "player");
        if (playerDescription == null)
            throw new ArgumentNullException(nameof(playerDescription));
        
        var foundPlayerPositions = roadData.GetPosition(playerRespawnDescription.Type);
        if (foundPlayerPositions.Count != 1)
            throw new ArgumentException($"Player respawn position not found or it's greater then 1.");

        if (playerDescription.Prefab == null)
            throw new ArgumentNullException(nameof(playerDescription.Prefab));

        var playerObj = Instantiate(playerDescription.Prefab, foundPlayerPositions[0], Quaternion.identity);
        _playerInstance = playerObj.GetComponent<Player>();
        _playerInstance.Setup(ecsWorld);
    }

    private void LinkCamera(EcsWorld ecsWorld)
    {
        if (_cameraController == null)
            throw new ArgumentNullException(nameof(_cameraController));

        _cameraController.Setup(ecsWorld, _playerInstance.Entity);
    }
    
    private void Update()
    {
        if(_updateSystems != null)
            _updateSystems.Run();
    }

    private void LateUpdate()
    {
        if(_lateUpdateSystems != null)
            _lateUpdateSystems.Run();
    }

    private void FixedUpdate()
    {
        if(_fixedUpdateSystems != null)
            _fixedUpdateSystems.Run();
    }
}