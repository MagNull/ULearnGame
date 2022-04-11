using Sources.Runtime.Interfaces;
using UnityEngine;

namespace Sources.Runtime.Player_Components
{
    public class Player : MonoBehaviour
    {
        [SerializeReference] private IMovement _movement;
    
        public void Init(IMovement movement)
        {
            _movement = movement;
        }
    }
}
