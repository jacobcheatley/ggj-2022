using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    [SerializeField]
    private string[] sceneNames;

    public static SceneLoadManager instance;

    void Start()
    {
        instance = this;
        StartCoroutine(Setup());
    }

    private IEnumerator Setup()
    {
        foreach (string sceneName in sceneNames)
        {
            var scene = SceneManager.GetSceneByName(sceneName);
            if (scene.isLoaded && sceneName != sceneNames[0])
                yield return SceneManager.UnloadSceneAsync(scene);
        }

        if (!SceneManager.GetSceneByName(sceneNames[0]).isLoaded)
            yield return SceneManager.LoadSceneAsync(sceneNames[0], LoadSceneMode.Additive);

        SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneNames[0]));
    }

    public void LoadScene(string sceneName, Action callBack = null)
    {
        IEnumerator LoadSceneAsync()
        {
            yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
            callBack?.Invoke();
        };

        StartCoroutine(LoadSceneAsync());
    }
}
