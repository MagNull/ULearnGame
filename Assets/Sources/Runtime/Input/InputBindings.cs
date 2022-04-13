using DefaultNamespace;
using Sources.Runtime.Interfaces;
using UnityEngine;

namespace Sources.Runtime.Input
{
    public class InputBindings : IUpdatable
    {
        private readonly PlayerInput _playerInput;
        private IMovement _movement;

        public InputBindings()
        {
            _playerInput = new PlayerInput();
        }

        public void BindMovement(IMovement movement) => _movement = movement;

        public void BindShooting(IShooter shooter)
        {
            _playerInput.Player.Shoot.started += _ => shooter.StartShooting();
            _playerInput.Player.Shoot.canceled += _ => shooter.EndShooting();
        }

        public void Update(float deltaTime)
        {
            var offset = _playerInput.Player.Movement.ReadValue<Vector2>();
            _movement.Move(offset.normalized);
        }

        public void OnEnable()
        {
            _playerInput.Enable();
            ;
        }

        public void OnDisable()
        {
            _playerInput.Disable();
        }
    }
}