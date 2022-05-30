using System;
using Sources.Runtime.Interfaces;
using Sources.Runtime.Player_Components;
using Sources.Runtime.UI___HUD;
using UnityEngine;
using Zenject;

namespace Sources.Runtime.Input
{
    public class InputBindings : MonoBehaviour
    {
        private PlayerInput _playerInput;
        private IMovement _movement;
        private IShooter _shooter;

        public void Init()
        {
            _playerInput = new PlayerInput();
        }

        [Inject]
        public void BindMovement(IMovement movement) => _movement = movement;

        [Inject]
        public void BindShooting(IShooter shooter) => _shooter = shooter;

        [Inject]
        public void BindBlink([Inject(Id = "Blink")] IAbility blink)
        {
            _playerInput.Player.Blink.performed += _ => blink.Cast();
        }

        [Inject]
        public void BindPause([Inject(Id = "Pause Screen")] StopScreen pauseScreen,
            [Inject(Id = "Die Screen")] StopScreen dieScreen) =>
            _playerInput.Player.Pause.performed += _ =>
            {
                if (pauseScreen.gameObject.activeSelf && 
                    !dieScreen.gameObject.activeSelf)
                    pauseScreen.Disable();
                else
                    pauseScreen.Enable();
            };

        private void Update()
        {
            if (_playerInput.Player.Shoot.inProgress)
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