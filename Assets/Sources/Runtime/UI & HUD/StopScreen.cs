using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Sources.Runtime.UI___HUD
{
    public class StopScreen : MonoBehaviour
    {
        [SerializeField]
        private float _appearanceDuration;

        [SerializeField]
        private Button _hubButton;
        [SerializeField]
        private Button _exitButton;

        private SceneLoader _sceneLoader;

        [Inject]
        private void Init(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
            _hubButton.onClick.AddListener(() =>
            {
                Time.timeScale = 1;
                _sceneLoader.LoadHub();
            });
            _exitButton.onClick.AddListener(Application.Quit);
        }

        public void Enable()
        {
            transform.DOKill();
            gameObject.SetActive(true);
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, _appearanceDuration).onComplete += () => Time.timeScale = 0;
        }

        public void Disable()
        {
            transform.DOKill();
            Time.timeScale = 1;
            transform.DOScale(Vector3.zero, _appearanceDuration).onComplete +=
                () => gameObject.SetActive(false);
        }
    }
}