using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    public Image LoadingBarFill;
    // Start is called before the first frame update
    void Start()
    { 
        StartCoroutine(LoadSceneAsync(3));
    }
    public void LoadScene(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
    }
    IEnumerator LoadSceneAsync(int sceneId)
    {
        yield return new WaitForSeconds(10);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        
        while(!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.01f);
            LoadingBarFill.fillAmount = progressValue;
            yield return null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
