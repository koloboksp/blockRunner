public static class Defines
{
    public const float ColliderHalfSize = 0.45f;
    public const float ColliderWidthHalfSize = ColliderHalfSize * 0.5f;
    public const float UnitSize = 1f;
    public const float UnitHalfSize = UnitSize * 0.5f;
    public const float Tolerance = 0.05f;
    public const float DistanceThreshold = UnitHalfSize - ColliderHalfSize + Tolerance;

    public const float GroundContactDistance = UnitSize * 0.5f;
    public const float GroundContactThreshold = GroundContactDistance - ColliderHalfSize;
}