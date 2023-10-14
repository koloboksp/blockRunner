public struct PlayerInputComponent
{
    public MoveDirection StrafeDirection;
    public bool Jump;
}

public enum MoveDirection
{
    None,
    Right,
    Left,
}

