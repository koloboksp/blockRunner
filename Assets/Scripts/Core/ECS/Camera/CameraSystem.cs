using Leopotam.EcsLite;
using UnityEngine;

public class CameraSystem : IEcsRunSystem
{
    public void Run(IEcsSystems ecsSystems)
    {
        var filter = ecsSystems.GetWorld()
            .Filter<CameraComponent>()
            .End();
        
        var cameraPool = ecsSystems.GetWorld().GetPool<CameraComponent>();
        var playerDynamicPool = ecsSystems.GetWorld().GetPool<PlayerDynamicComponent>();

        foreach (var entity in filter)
        {
            ref var cameraComponent = ref cameraPool.Get(entity);
            var playerDynamicComponent = playerDynamicPool.Get(cameraComponent.DynamicEntity);

            var cameraTransform = cameraComponent.CameraTransform;
            
            var cameraPosition = cameraTransform.position;
            var cameraDirection = -(cameraTransform.rotation * Vector3.forward);
            var desiredPosition = playerDynamicComponent.GetPosition() + cameraComponent.Offset;
            
            var cameraNewPosition = cameraPosition + (desiredPosition - cameraPosition) * Time.deltaTime * cameraComponent.Smoothness;
            var vecToTarget = cameraNewPosition - playerDynamicComponent.GetPosition();
            
            var desiredDirection = vecToTarget.normalized;
            var cameraNewDirection = cameraDirection + (desiredDirection - cameraDirection) * Time.deltaTime * cameraComponent.Smoothness;
            
            cameraTransform.position = cameraNewPosition;
            cameraTransform.rotation = Quaternion.LookRotation(-cameraNewDirection);
        }    
    }

    public static void SetupCameraInitialPosition(Transform cameraTransform, Vector3 cameraOffset, Vector3 targetPosition)
    {
        var desiredPosition = targetPosition + cameraOffset;
        
        var vecToTarget = desiredPosition - targetPosition;
        var dirToTarget = vecToTarget.normalized;
        cameraTransform.position = desiredPosition;
        cameraTransform.rotation = Quaternion.LookRotation(-dirToTarget);
    }
}