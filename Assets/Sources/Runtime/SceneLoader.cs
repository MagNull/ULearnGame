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

    public void LoadHub()
    {
        SceneManager.LoadScene(0);
    }

    public void LoadGolemRoom()
    {
        SceneManager.LoadSceneAsync(1);
    }
}