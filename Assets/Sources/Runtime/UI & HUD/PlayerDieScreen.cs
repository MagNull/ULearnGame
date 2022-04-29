using UnityEngine;
using UnityEngine.UI;

namespace Sources.Runtime.UI___HUD
{
    public class PlayerDieScreen : MonoBehaviour
    {
        [SerializeField]
        private Button _hubButton;
        [SerializeField]
        private Button _exitButton;

        private SceneLoader _sceneLoader;

        public void Init(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
            _hubButton.onClick.AddListener(() =>
            {
                Time.timeScale = 1;
                _sceneLoader.LoadHub();
            });
            _exitButton.onClick.AddListener(Application.Quit);
        }
    }
}