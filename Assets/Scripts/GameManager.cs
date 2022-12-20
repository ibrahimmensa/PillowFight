using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public enum GameModeType
{
    NONE,
    MULTIPLAYER,
    SURVIVAL_MODE,
    TIMER_MODE
}

public class GameManager : Singleton<GameManager>
{
    public PhotonView pv;
    public GameObject Environment1Prefab;
    public GameObject PlayerPrefab;
    public GameObject UICamera;
    public GameObject currentGameEnvironment;
    public int countOfAIPlayers=0;
    public GameObject AIPlayer;
    public Coroutine TimerForTimerMode;
    public int CurrentGameKillCount = 0;
    public GameModeType gameModeType = GameModeType.NONE;

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

    public void StartGameTimerMode()
    {
        gameModeType = GameModeType.TIMER_MODE;
        UIManager.Instance.LobbyScreen.SetActive(false);
        UIManager.Instance.MultiPlayerScreen.SetActive(false);
        UIManager.Instance.PrivateRoomScreen.SetActive(false);
        UIManager.Instance.JoinPrivateRoomScreen.SetActive(false);
        UIManager.Instance.CreatePrivateRoomScreen.SetActive(false);
        currentGameEnvironment = Instantiate(Environment1Prefab);
        PhotonManager.instance._playerObj = Instantiate(PlayerPrefab, currentGameEnvironment.GetComponent<EnvironmentManager>().playerSpawnPoints[0].transform.position, currentGameEnvironment.GetComponent<EnvironmentManager>().playerSpawnPoints[0].transform.rotation);
        PhotonManager.instance._playerObj.GetComponent<PlayerController>().SetPlayerName(UIManager.Instance.playerNameText.text);
        UIManager.Instance.MainScreen.SetActive(false);
        UIManager.Instance.GameUI.SetActive(true);
        UICamera.SetActive(false);
        UICamera.SetActive(true);
        SpawnNextAIPlayer();
        //start timer
        UIManager.Instance.TimeTextForTimerMode.gameObject.SetActive(true);
        UIManager.Instance.TimeTextForTimerMode.text = "TIMER : 01:00";
        TimerForTimerMode = StartCoroutine(TimerForTimerModeGameplay(60,false));
    }

    public void TimerModeExtension()
    {
        Time.timeScale = 1;
        UIManager.Instance.GameOverPopup.SetActive(false);
        UIManager.Instance.TimeTextForTimerMode.text = "TIMER : 00:30";
        TimerForTimerMode = StartCoroutine(TimerForTimerModeGameplay(30,true));
    }

    IEnumerator TimerForTimerModeGameplay(int timer,bool finalCall)
    {
        while (timer > 0)
        {
            yield return new WaitForSeconds(1f);
            timer--;
            if(timer>=10)
                UIManager.Instance.TimeTextForTimerMode.text = "TIMER : 00:" + timer;
            else
                UIManager.Instance.TimeTextForTimerMode.text = "TIMER : 00:0" + timer;
        }
        UIManager.Instance.GameOverPopup.SetActive(true);
        if (finalCall)
        {
            AdsManager.Instance.ShowInterstitialAdWithDelay();
            TimerModeGameplayOver();
        }
        else
            Time.timeScale = 0;
    }

    public void TimerModeGameplayOver()
    {
        Time.timeScale = 1;
        int rewardedCoins = CurrentGameKillCount * 20;
        int totalCoins = PlayerPrefs.GetInt("Coins", 0) + rewardedCoins;
        PlayerPrefs.SetInt("Coins", totalCoins);
        UIManager.Instance.UpdateCoinsStatus(PlayerPrefs.GetInt("Coins"));
        CurrentGameKillCount = 0;
        UIManager.Instance.TimeTextForTimerMode.gameObject.SetActive(false);
        Destroy(currentGameEnvironment);
        Destroy(PhotonManager.instance._playerObj);
        Destroy(AIPlayer);
        UIManager.Instance.QuitConfirmationPopup.SetActive(false);
        UIManager.Instance.GameUI.SetActive(false);
        UIManager.Instance.MainScreen.SetActive(true);
        UICamera.SetActive(false);
        UICamera.SetActive(true);
        gameModeType = GameModeType.NONE;
    }

    public void StartGameSurvivalMode()
    {
        gameModeType = GameModeType.SURVIVAL_MODE;
        UIManager.Instance.LobbyScreen.SetActive(false);
        UIManager.Instance.MultiPlayerScreen.SetActive(false);
        UIManager.Instance.PrivateRoomScreen.SetActive(false);
        UIManager.Instance.JoinPrivateRoomScreen.SetActive(false);
        UIManager.Instance.CreatePrivateRoomScreen.SetActive(false);
        currentGameEnvironment = Instantiate(Environment1Prefab);
        PhotonManager.instance._playerObj = Instantiate(PlayerPrefab, currentGameEnvironment.GetComponent<EnvironmentManager>().playerSpawnPoints[0].transform.position, currentGameEnvironment.GetComponent<EnvironmentManager>().playerSpawnPoints[0].transform.rotation);
        PhotonManager.instance._playerObj.GetComponent<PlayerController>().SetPlayerName(UIManager.Instance.playerNameText.text);
        UIManager.Instance.MainScreen.SetActive(false);
        UIManager.Instance.GameUI.SetActive(true);
        UICamera.SetActive(false);
        UICamera.SetActive(true);
        SpawnNextAIPlayer();
    }

    public void SpawnNextAIPlayer()
    {
        AIPlayer = Instantiate(PlayerPrefab, currentGameEnvironment.GetComponent<EnvironmentManager>().playerSpawnPoints[3].transform.position, currentGameEnvironment.GetComponent<EnvironmentManager>().playerSpawnPoints[3].transform.rotation);
        AIPlayer.GetComponent<PlayerController>().isAIPlayer = true;
    }

    public void StartGameMultiplayer()
    {
        pv.RPC("StartGameMultiplayerRPC", RpcTarget.All);
    }

    [PunRPC]
    void StartGameMultiplayerRPC()
    {
        PhotonManager.instance.gameState = "InGame";
        gameModeType = GameModeType.MULTIPLAYER;
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
        countOfAIPlayers = 4 - PhotonManager.instance.playersNameList.Count;
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
