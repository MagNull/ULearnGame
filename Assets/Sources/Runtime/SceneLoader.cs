using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    private void Awake()
    {
        var sceneLoader = FindObjectOfType<SceneLoader>();
        if (sceneLoader && sceneLoader != this)
            Destroy(gameObject);
        else
            DontDestroyOnLoad(gameObject);
    }

    public void LoadHub() => SceneManager.LoadScene("Hub");

    public void LoadGolemRoom() => SceneManager.LoadSceneAsync("Golem Room");

    public void LoadReaperRoom() => SceneManager.LoadSceneAsync("Reaper Room");

    public void Exit() => Application.Quit();
}