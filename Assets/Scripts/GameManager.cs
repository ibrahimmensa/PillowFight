using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : Singleton<GameManager>
{
    public PhotonView pv;
    public GameObject Environment1Prefab;
    public GameObject UICamera;
    public GameObject currentGameEnvironment;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    [PunRPC]
    public void StartGameMultiplayer()
    {
        UIManager.Instance.LobbyScreen.SetActive(false);
        currentGameEnvironment = Instantiate(Environment1Prefab);
        int index = PhotonManager.instance.playersNameList.IndexOf(PhotonNetwork.NickName);
        PhotonManager.instance._playerObj = PhotonNetwork.Instantiate("Player", currentGameEnvironment.GetComponent<EnvironmentManager>().playerSpawnPoints[index].transform.position, currentGameEnvironment.GetComponent<EnvironmentManager>().playerSpawnPoints[index].transform.rotation, 0);
        UIManager.Instance.MainScreen.SetActive(false);
        UIManager.Instance.GameUI.SetActive(true);
        UICamera.SetActive(false);
        UICamera.SetActive(true);
    }
}
