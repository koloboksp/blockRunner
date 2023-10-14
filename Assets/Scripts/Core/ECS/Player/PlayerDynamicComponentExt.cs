using UnityEngine;

public static class PlayerDynamicComponentExt
{
    public static Vector3 GetPosition(this PlayerDynamicComponent playerDynamic)
    {
        return new Vector3(playerDynamic.X, playerDynamic.Y, playerDynamic.Z);
    }
    public static void ResetPosition(this ref PlayerDynamicComponent playerDynamic, Vector3 position)
    {
        playerDynamic.X = position.x;
        playerDynamic.Y = position.y;
        playerDynamic.Z = position.z;
    }
}