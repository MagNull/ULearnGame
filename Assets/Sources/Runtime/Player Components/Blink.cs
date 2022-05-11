using System;
using Sources.Runtime.Interfaces;
using UnityEngine;

namespace Sources.Runtime.Player_Components
{
    public class Blink : IAbility
    {
        public event Action Casted;
        public float CoolDown => _coolDown;
        private readonly IMovement _movement;
        private readonly Transform _transform;
        private readonly ParticleSystem _startVFX;
        private readonly ParticleSystem _endVFX;
        private readonly float _wallBlinkDistance;
        private readonly LayerMask _checkLayer;
        private float _distance;
        private readonly float _coolDown;
        private Vector2 _movementDirection;
        private float _lastCastTime;

        public Blink(IMovement movement, Transform transform, float distance, float coolDown, ParticleSystem startVFX,
            ParticleSystem endVFX, float wallBlinkDistance, LayerMask checkLayer)
        {
            _movement = movement;
            _transform = transform;
            _distance = distance;
            _coolDown = coolDown;
            _startVFX = startVFX;
            _endVFX = endVFX;
            _lastCastTime = -_coolDown;
            _wallBlinkDistance = wallBlinkDistance;
            _checkLayer = checkLayer;
            _movement.Moved += dir => _movementDirection = dir == Vector2.zero ? Vector2.up : dir;
        }

        public void Cast()
        {
            if(Time.time - _lastCastTime < _coolDown)
                return;
            
            _lastCastTime = Time.time;
            var blinkPos = (Vector2) _transform.position + _movementDirection * _distance;
            var hit = Physics2D.Raycast(_transform.position, _movementDirection, _distance, _checkLayer);
            blinkPos = hit ? hit.point - _movementDirection * _wallBlinkDistance : blinkPos;
            
            BlinkVFX(blinkPos);
            _transform.position = blinkPos;
            Casted?.Invoke();
        }

        private void BlinkVFX(Vector2 blinkPos)
        {
            _startVFX.transform.position = _transform.position;
            _startVFX.Play();
            _endVFX.transform.position = blinkPos;
            _endVFX.Play();
        }
    }
}