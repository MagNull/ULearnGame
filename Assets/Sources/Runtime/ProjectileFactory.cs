using UnityEngine;

namespace DefaultNamespace
{
    public class ProjectileFactory : MonoBehaviour, IFactory<Projectile>
    {
        [SerializeField]
        private Projectile _projectilePrefab;
        
        public Projectile Create()
        {
            return Instantiate(_projectilePrefab);
        }
    }
}