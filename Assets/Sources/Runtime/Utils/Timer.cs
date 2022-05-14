using System;

namespace Sources.Runtime.Utils
{
    public class Timer
    {
        private readonly float _duration;
        private readonly Action _action;
        private float _currentTick;

        public Timer(float duration, Action action)
        {
            _duration = duration;
            _action = action;
            _currentTick = duration;
        }
        
        public void Tick(float deltaTime)
        {
            if(_currentTick <= 0)
                return;
            _currentTick -= _duration;
            if (_currentTick <= 0)
                _action?.Invoke();
        }
    }
}