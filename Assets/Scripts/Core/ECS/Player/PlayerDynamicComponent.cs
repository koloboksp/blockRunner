using UnityEngine;

public struct PlayerDynamicComponent
{
    public Rigidbody PlayerRigidbody;
    public Collider PlayerCollider;
 
    public float X;
    public float Y;
    public float Z;

    public bool IsFlying;
}