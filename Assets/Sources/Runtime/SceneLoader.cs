using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private const string _hub = "Hub";
    private const string _startMenu = "Start Menu";
    private const string _golemRoom = "Golem Room";
    private const string _reaperRoom = "Reaper Room";
    
    private void Awake()
    {
        var sceneLoader = FindObjectOfType<SceneLoader>();
        if (sceneLoader && sceneLoader != this)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
    }

    public void LoadHub() => SceneManager.LoadScene(_hub);

    public void LoadMenu() => SceneManager.LoadScene(_startMenu);

    public void LoadGolemRoom() => SceneManager.LoadSceneAsync(_golemRoom);

    public void LoadReaperRoom() => SceneManager.LoadSceneAsync(_reaperRoom);

    public void Exit() => Application.Quit();
}