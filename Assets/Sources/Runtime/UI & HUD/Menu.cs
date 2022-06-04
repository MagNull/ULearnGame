using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Sources.Runtime.UI___HUD
{
    public class Menu : MonoBehaviour
    {
        [SerializeField]
        private Button _playButton;
        [SerializeField]
        private Button _exitButton;

        private void Start()
        {
            var sceneLoader = FindObjectOfType<SceneLoader>();
            _playButton.onClick.AddListener(() =>
            {
                Time.timeScale = 1;
                sceneLoader.LoadHub();
            });

            _exitButton.onClick.AddListener(Application.Quit);
        }
    }
}