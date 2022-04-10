using System;
using Sources.Runtime;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float _speed = 1;
    private PlayerMovement _movement;
    
    private InputBindings _inputBindings;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _movement = new PlayerMovement(GetComponent<Rigidbody2D>(), _speed);
        _inputBindings = new InputBindings();
    }

    private void Start()
    {
        _inputBindings.BindMovement(_movement);
    }

    private void OnEnable()
    {
        _inputBindings.OnEnable();
    }

    private void OnDisable()
    {
        _inputBindings.OnDisable();
    }
    
    private void FixedUpdate()
    {
        _inputBindings.Update(Time.deltaTime);
    }
}
