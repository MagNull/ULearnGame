using System.Collections;
using System.Collections.Generic;
using Sources.Runtime;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeReference] private IMovement _movement;
    
    public void Init(IMovement movement)
    {
        _movement = movement;
    }
}
