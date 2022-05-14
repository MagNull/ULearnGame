using UnityEngine;

namespace Sources.Runtime.Boss_Components.Golem
{
    public class GolemPhaseSwitching : BossPhaseSwitching
    {
        [SerializeField]
        private GameObject _spikes;
        protected override void OnPhaseSwitchingStarted()
        {
            _spikes.SetActive(true);
        }

        protected override void OnPhaseSwitchingEnded()
        {
            _spikes.SetActive(false);
        }
    }
}