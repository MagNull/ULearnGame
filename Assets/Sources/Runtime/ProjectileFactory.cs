using Sources.Runtime.Interfaces;
using UnityEngine;

namespace Sources.Runtime
{
    public class ProjectileFactory : MonoBehaviour, IFactory<Projectile>
    {
        [SerializeField]
        private Projectile _projectilePrefab;
        
        public Projectile Create()
        {
            return Instantiate(_projectilePrefab, transform);
        }
    }
}