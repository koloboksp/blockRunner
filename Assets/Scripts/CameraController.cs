using Leopotam.EcsLite;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _smoothness = 1;
    [SerializeField] private Vector3 _offset = new Vector3(-6.0f, 4f, 0);

    private EcsWorld _ecsWorld;
    private int _entity;
    
    public void Setup(EcsWorld ecsWorld, int dynamicEntity)
    {
        _ecsWorld = ecsWorld;
        
        _entity = _ecsWorld.NewEntity();
        var cameraPool = _ecsWorld.GetPool<CameraComponent>();
        cameraPool.Add(_entity);
        ref var cameraComponent = ref cameraPool.Get(_entity);
        
        cameraComponent.DynamicEntity = dynamicEntity;
        cameraComponent.CameraTransform = _camera.transform; 
        cameraComponent.Smoothness = _smoothness;
        cameraComponent.Velocity = Vector3.zero;
        cameraComponent.Offset = _offset;
        
        var playerDynamicPool = _ecsWorld.GetPool<PlayerDynamicComponent>();
        var playerDynamicComponent = playerDynamicPool.Get(dynamicEntity);
        CameraSystem.SetupCameraInitialPosition(transform, _offset, playerDynamicComponent.GetPosition());
    }

    public void Clear()
    {
        if (_entity != 0)
        {
            var cameraPool = _ecsWorld.GetPool<CameraComponent>();
            cameraPool.Del(_entity);
        }
    }
}