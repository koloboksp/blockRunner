using Leopotam.EcsLite;
using UnityEngine;

public sealed class PlayerInputSystem : IEcsRunSystem
{
    public void Run(IEcsSystems ecsSystems)
    {
        var filter = ecsSystems.GetWorld()
            .Filter<PlayerInputComponent>()
            .Inc<PlayerObstaclesComponent>()
            .End();
        
        var playerInputPool = ecsSystems.GetWorld().GetPool<PlayerInputComponent>();
        var playerCollisionsPool = ecsSystems.GetWorld().GetPool<PlayerObstaclesComponent>();

        foreach (var entity in filter)
        {
            var playerCollisionsComponent = playerCollisionsPool.Get(entity);
            ref var playerInputComponent = ref playerInputPool.Get(entity);
            
            var moveDirection = MoveDirection.None;
            var jump = false;


            var axisRaw = Input.GetAxisRaw("Horizontal");
            if (axisRaw != 0.0f)
            {
                if (axisRaw > 0 && !playerCollisionsComponent.Right)
                    moveDirection = MoveDirection.Right;
                else if (axisRaw < 0 && !playerCollisionsComponent.Left)
                    moveDirection = MoveDirection.Left;
            }
            
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (playerCollisionsComponent.Bottom)
                    jump = true;
            }

            if(SwipeInput.swipedRight)
                moveDirection = MoveDirection.Right;
            if(SwipeInput.swipedLeft)
                moveDirection = MoveDirection.Left;
            if (SwipeInput.swipedUp)
            {
                if (playerCollisionsComponent.Bottom)
                    jump = true;
            }

            playerInputComponent.StrafeDirection = moveDirection;
            playerInputComponent.Jump = jump;
        }
    }
}