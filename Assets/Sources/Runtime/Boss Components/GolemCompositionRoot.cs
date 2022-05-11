using Sources.Runtime.Player_Components;
using UnityEngine;

namespace Sources.Runtime.Boss_Components
{
    public class GolemCompositionRoot : BossCompositionRoot<GolemShooter, GolemAttack>
    {
        public override void InstallBindings()
        {
            base.InstallBindings();
            Container.Bind<Transform>().WithId("Player").FromInstance(FindObjectOfType<Player>().transform);
        }
    }
}