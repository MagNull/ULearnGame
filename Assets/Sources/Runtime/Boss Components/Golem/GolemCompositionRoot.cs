using Sources.Runtime.Player_Components;
using UnityEngine;

namespace Sources.Runtime.Boss_Components
{
    [RequireComponent(typeof(GolemShooter), typeof(GolemAttack))]
    public class GolemCompositionRoot : BossCompositionRoot<GolemShooter>
    {
        
    }
}