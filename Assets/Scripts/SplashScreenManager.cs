using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreenManager : MonoBehaviourPunCallbacks
{
    public Slider loadingBar;
    AsyncOperation SceneLoading;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(loading());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator loading()
    {
        while (loadingBar.value < 9)
        {
            loadingBar.value = loadingBar.value + 0.1f;
            yield return new WaitForSeconds(0.00005f);
        }
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        float previousProgress = 0f;
        loadingBar.value = 9;
        SceneLoading = SceneManager.LoadSceneAsync(1);

        // disable scene activation while loading to prevent auto load
        SceneLoading.allowSceneActivation = false;

        while (!SceneLoading.isDone)
        {
            loadingBar.value += (SceneLoading.progress - previousProgress);
            previousProgress = SceneLoading.progress;
            if (SceneLoading.progress >= 0.9f)
            {
                loadingBar.value = 10f;
                SceneLoading.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
