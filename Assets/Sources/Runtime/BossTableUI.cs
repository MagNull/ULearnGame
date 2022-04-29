using UnityEngine;
using UnityEngine.UI;

namespace Sources.Runtime
{
    public class BossTableUI : MonoBehaviour
    {
        [SerializeField]
        private Button _golemButton;
        [SerializeField]
        private Button _closeButton;
        
        private SceneLoader _sceneLoader;
        
        public void Init(SceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
            _golemButton.onClick.AddListener(() => _sceneLoader.LoadGolemRoom());
            _closeButton.onClick.AddListener(() => gameObject.SetActive(false));
        }
    }
}