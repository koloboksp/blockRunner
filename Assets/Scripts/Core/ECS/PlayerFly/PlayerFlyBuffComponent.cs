public struct PlayerFlyBuffComponent
{
    public float FlyTime;
    public float TakeOffTime;
    public float LandingTime;
    public float Height;
    
    public float RestTimer;
    public FlyState State;
    public float StartHeight;
}

public enum FlyState
{
    None,
    TakingOff,
    Fly,
    Landing,
}