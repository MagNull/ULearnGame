using System;
using Sources.Runtime.Interfaces;
using UnityEngine;

namespace Sources.Runtime
{
    public class Beam : MonoBehaviour
    {
        [SerializeField]
        private int _damagePerSeconds;
        private float _lastDamageTime;

        private void OnTriggerStay2D(Collider2D col)
        {
            if (col.TryGetComponent(out IDamageable damageable))
            {
                if (Time.time - _lastDamageTime >= 1)
                {
                    damageable.TakeDamage(_damagePerSeconds);
                    _lastDamageTime = Time.time;
                }
            }
        }
    }
}