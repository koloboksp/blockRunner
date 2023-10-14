using UnityEngine;

public class ColliderOwner : MonoBehaviour
{
    [SerializeField] private MonoBehaviour _owner;

    public MonoBehaviour Owner => _owner;
}