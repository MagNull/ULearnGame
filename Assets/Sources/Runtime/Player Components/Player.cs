using DefaultNamespace;
using Sources.Runtime.Interfaces;
using UnityEngine;

namespace Sources.Runtime.Player_Components
{
    public class Player : MonoBehaviour
    {
        [SerializeReference]
        private IMovement _movement;
        [SerializeReference]
        private IShooter _shooter;

        public void Init(IMovement movement, IShooter shooter)
        {
            _movement = movement;
            _shooter = shooter;
        }
    }
}