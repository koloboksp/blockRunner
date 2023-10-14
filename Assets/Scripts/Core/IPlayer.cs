using UnityEngine;

public interface IPlayer
{
    GameObject Owner { get; }
    int Entity { get; }
    void Die();
}