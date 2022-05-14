using UnityEngine;

namespace Sources.Runtime.Boss_Components.Reaper
{
    public class ReaperPhaseSwitching : BossPhaseSwitching
    {
        private SpriteRenderer _spriteRenderer;
        private Collider2D _collider;

        private void Awake()
        {
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            _collider = GetComponent<Collider2D>();
        }

        protected override void OnPhaseSwitchingStarted()
        {
            _spriteRenderer.color = new Color(1, 1, 1, .7f);
            _collider.enabled = false;
        }

        protected override void OnPhaseSwitchingEnded()
        {
            _spriteRenderer.color = new Color(1, 1, 1, 1f);
            _collider.enabled = true;
        }
    }
}