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
        if (PlayerPrefs.GetInt("Coins", 0) == 0)
        {
            PlayerPrefs.SetInt("Coins", 0);
            UIManager.Instance.UpdateCoinsStatus(PlayerPrefs.GetInt("Coins"));
        }
        else
        {
            UIManager.Instance.UpdateCoinsStatus(PlayerPrefs.GetInt("Coins"));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGameMultiplayer()
    {
        pv.RPC("StartGameMultiplayerRPC", RpcTarget.All);
    }

    [PunRPC]
    void StartGameMultiplayerRPC()
    {
        PhotonManager.instance.gameState = "InGame";
        PhotonNetwork.CurrentRoom.IsOpen = false;
        UIManager.Instance.LobbyScreen.SetActive(false);
        UIManager.Instance.MultiPlayerScreen.SetActive(false);
        UIManager.Instance.PrivateRoomScreen.SetActive(false);
        UIManager.Instance.JoinPrivateRoomScreen.SetActive(false);
        UIManager.Instance.CreatePrivateRoomScreen.SetActive(false);
        currentGameEnvironment = Instantiate(Environment1Prefab);
        int index = PhotonManager.instance.playersNameList.IndexOf(PhotonNetwork.NickName);
        PhotonManager.instance._playerObj = PhotonNetwork.Instantiate("Player", currentGameEnvironment.GetComponent<EnvironmentManager>().playerSpawnPoints[index].transform.position, currentGameEnvironment.GetComponent<EnvironmentManager>().playerSpawnPoints[index].transform.rotation, 0);
        PhotonManager.instance._playerObj.GetComponent<PlayerController>().SetPlayerName(PhotonNetwork.NickName);
        UIManager.Instance.MainScreen.SetActive(false);
        UIManager.Instance.GameUI.SetActive(true);
        UICamera.SetActive(false);
        UICamera.SetActive(true);
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonManager.instance.playersNameList.Count < 4)
            {
                for(int i=0; i < (4 - PhotonManager.instance.playersNameList.Count); i++)
                {
                    GameObject AIPlayer= PhotonNetwork.InstantiateRoomObject("Player", currentGameEnvironment.GetComponent<EnvironmentManager>().playerSpawnPoints[PhotonManager.instance.playersNameList.Count+i].transform.position, currentGameEnvironment.GetComponent<EnvironmentManager>().playerSpawnPoints[PhotonManager.instance.playersNameList.Count + i].transform.rotation, 0);
                    AIPlayer.GetComponent<PlayerController>().isAIPlayer = true;
                }
            }
        }
    }
}
