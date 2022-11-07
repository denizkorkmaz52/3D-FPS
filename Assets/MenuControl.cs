using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControl : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider loadingSlider;
    public void StartGame()
    {

        StartCoroutine(sceneLoader());
    }
    IEnumerator sceneLoader()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
        loadingScreen.SetActive(true);
        while (!operation.isDone)
        {
            float loaded = Mathf.Clamp01(operation.progress / .9f);
            loadingSlider.value = loaded;
            yield return null;
        }
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
