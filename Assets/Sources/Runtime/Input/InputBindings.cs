using System;
using Sources.Runtime.Interfaces;
using Sources.Runtime.Player_Components;
using UnityEngine;

namespace Sources.Runtime.Input
{
    public class InputBindings : MonoBehaviour
    {
        private PlayerInput _playerInput;
        private IMovement _movement;
        private IShooter _shooter;

        private void Awake()
        {
            _playerInput = new PlayerInput();
        }

        public void BindMovement(IMovement movement) => _movement = movement;

        public void BindShooting(IShooter shooter)
        {
            _shooter = shooter;
        }

        public void BindBlink(Blink blink) => _playerInput.Player.Blink.performed += _ => blink.Cast();

        private void Update()
        {
            if(_playerInput.Player.Shoot.inProgress)
                _shooter.Shoot();
        }

        public void FixedUpdate()
        {
            var offset = _playerInput.Player.Movement.ReadValue<Vector2>();
            _movement.Move(offset.normalized);
        }

        public void OnEnable()
        {
            _playerInput.Enable();
        }

        public void OnDisable()
        {
            _playerInput.Disable();
        }
    }
}