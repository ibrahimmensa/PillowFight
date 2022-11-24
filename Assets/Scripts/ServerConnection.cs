using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ServerConnection : MonoBehaviourPunCallbacks
{
    public Image LoadingBarFill;
    // public Button buttonRef;
    private void Start()
    {
       // buttonRef.interactable = false;
        PhotonNetwork.ConnectUsingSettings();
    }    
    
    public void Connection()
    {
        PhotonNetwork.ConnectUsingSettings();
    }


    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        //make the button interactable
        //  buttonRef.interactable = true;

        StartCoroutine(LoadSceneAsync(1));

    }
    IEnumerator LoadSceneAsync(int sceneId)
    {
        //  yield return new WaitForSeconds(5);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / 0.5f);
            LoadingBarFill.fillAmount = progressValue;
            yield return null;
        }
    }
}
