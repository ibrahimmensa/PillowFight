using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using TMPro;

public class PhotonManager : MonoBehaviourPunCallbacks, IInRoomCallbacks
{
    string gameVersion = "1";
    public string gameState = "none";
    public string privateGameCode;
    public List<string> playersNameList = new List<string>();

    public static PhotonManager instance;
    public PhotonView pv;
    public GameObject _playerObj;
    public bool isPhotonConnected = false;
    public int lobbyTimer = 30;

    Coroutine lobbyTimerCoroutine;

    char[] characters="ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789".ToCharArray();


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
        //PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        Connect();
        PhotonNetwork.SerializationRate = 5;
    }

    public void Connect()
    {
        if (PhotonNetwork.IsConnected)
        {
            Debug.Log("Already connected");
            //join random room
        }

        else
        {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
            //roomListings = new List<RoomInfo>();
        }
    }

    public void RandomRoomCode()
    {
        int desiredCodeLength = 15;
        string code = "";
        while (code.Length < desiredCodeLength)
        {
            code += characters[Random.Range(0, characters.Length)];
        }
        Debug.Log("Random code: " + code);
        privateGameCode = code;
    }

    public void CreatePrivateRoom()
    {
        if (isPhotonConnected)
        {
            AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);

            RoomOptions newRoom = new RoomOptions() { MaxPlayers = 4, IsVisible = false };
            PhotonNetwork.CreateRoom(privateGameCode, newRoom);
            UIManager.Instance.LoadingScreen.SetActive(true);
        }
        else
        {
            UIManager.Instance.PanelOpenFadeIn(UIManager.Instance.InternetConnectionErrorPanel);
        }
    }

    public void JoinPrivateRoom()
    {
        if (isPhotonConnected)
        {
            AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
            if (UIManager.Instance.privateRoomCodeInputField.text != "")
            {
                if (PlayerPrefs.GetInt("Coins", 0) >= 500)
                {
                    int totalCoins = PlayerPrefs.GetInt("Coins", 0) - 500;
                    PlayerPrefs.SetInt("Coins", totalCoins);
                    UIManager.Instance.UpdateCoinsStatus(PlayerPrefs.GetInt("Coins"));

                    PhotonNetwork.JoinRoom(UIManager.Instance.privateRoomCodeInputField.text);
                    UIManager.Instance.LoadingScreen.SetActive(true);
                }
                else
                {
                    UIManager.Instance.PanelOpenFadeIn(UIManager.Instance.NotEnoughCoinsForMultiplayerErrorPanel);
                }
            }
        }
        else
        {
            UIManager.Instance.PanelOpenFadeIn(UIManager.Instance.InternetConnectionErrorPanel);
        }
    }

    public void CreatePublicRoom()
    {
        if (isPhotonConnected)
        {
            Debug.Log("public room created");
            RoomOptions newRoom = new RoomOptions() { MaxPlayers = 4, IsVisible = true };
            PhotonNetwork.CreateRoom(null, newRoom, TypedLobby.Default);
        }
        else
        {
            UIManager.Instance.PanelOpenFadeIn(UIManager.Instance.InternetConnectionErrorPanel);
        }
    }

    public void JoinPublicRoom()
    {
        if (isPhotonConnected)
        {
            AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
            if (PlayerPrefs.GetInt("Coins", 0) >= 500)
            {
                int totalCoins = PlayerPrefs.GetInt("Coins", 0) - 500;
                PlayerPrefs.SetInt("Coins", totalCoins);
                UIManager.Instance.UpdateCoinsStatus(PlayerPrefs.GetInt("Coins"));

                PhotonNetwork.JoinRandomRoom();
                UIManager.Instance.LoadingScreen.SetActive(true);
            }
            else
            {
                UIManager.Instance.PanelOpenFadeIn(UIManager.Instance.NotEnoughCoinsForMultiplayerErrorPanel);
            }
        }
        else
        {
            UIManager.Instance.PanelOpenFadeIn(UIManager.Instance.InternetConnectionErrorPanel);
        }
    }


#region MonoBehaviorPunCallBacks

    public override void OnConnected()
    {
        base.OnConnected();
        //isPhotonConnected = true;
        PhotonNetwork.NickName = UIManager.Instance.playerNameText.text + Random.Range(10000, 99999); 
        lobbyTimer = 30;
        gameState = "none";
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnected() was called by PUN");
    }


    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
        isPhotonConnected = true;
        PhotonNetwork.JoinLobby();
        UIManager.Instance.LoadingScreen.SetActive(false);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        isPhotonConnected = false;
        UIManager.Instance.LoadingScreen.SetActive(false); 
        UIManager.Instance.PanelOpenFadeIn(UIManager.Instance.InternetConnectionErrorPanel);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom"+returnCode+message);
        CreatePublicRoom();
    }


    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
        //enable room setting panel
        Debug.Log("PUN Basics Tutorial/Launcher:OnCreateRoom() was called by PUN.");

    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
        Debug.Log("PUN Basics Tutorial/Launcher:OnCreateRoomFailed() was called by PUN.");
    }



    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
        gameState = "InLobby";
        UIManager.Instance.LoadingScreen.SetActive(false);
        UIManager.Instance.lobbyTimer.text="";
        UIManager.Instance.PanelOpenFadeIn(UIManager.Instance.LobbyScreen);
        if (PhotonNetwork.IsMasterClient)
        {
            playersNameList.Add(PhotonNetwork.NickName);
            UIManager.Instance.lobbyPlayersNames[0].text = playersNameList[0].Remove(playersNameList[0].Length - 5);
            lobbyTimerCoroutine= StartCoroutine(LobbyCountDown());
        }
        //if (PhotonNetwork.IsMasterClient)
        //{
        //    UIManager.instance.SetLobbyScreenForMaster();
        //    //UIManager.instance.StartGameButton.interactable = true;
        //}
        //else
        //{
        //    UIManager.instance.SetLobbyScreenForOthers();
        //    RoomCode = int.Parse(PhotonNetwork.CurrentRoom.Name);
        //}

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinRoomFailed() called by PUN. Now this client is in a room."+returnCode+message);
        if (returnCode == 32758)
        {
            UIManager.Instance.PanelOpenFadeIn(UIManager.Instance.JoinPrivateRoomFailedPanel);
        }
        if (returnCode == 32765)
        {
            UIManager.Instance.PanelOpenFadeIn(UIManager.Instance.JoinPrivateRoomFullPanel);
        }
        UIManager.Instance.LoadingScreen.SetActive(false);
    }

    public override void OnLeftRoom()
    {
        Debug.Log("PUN Basics Tutorial/Launcher: OnLeftRoom() called by PUN.");

        playersNameList.Clear();
        for (int i = 0; i < UIManager.Instance.lobbyPlayersNames.Length; i++)
        {
            UIManager.Instance.lobbyPlayersNames[i].text = "";
        }
        if (gameState == "InLobby")
        {
            UIManager.Instance.PrivateRoomScreen.SetActive(false);
            UIManager.Instance.JoinPrivateRoomScreen.SetActive(false);
            UIManager.Instance.CreatePrivateRoomScreen.SetActive(false);
            UIManager.Instance.PanelCloseFadeOut(UIManager.Instance.LobbyScreen);
            UIManager.Instance.LoadingScreen.SetActive(true);
            if (lobbyTimerCoroutine != null)
                StopCoroutine(lobbyTimerCoroutine);
        }
        lobbyTimer = 30;

        if (gameState == "InGame")
        {
            Destroy(GameManager.Instance.currentGameEnvironment); 
            Destroy(_playerObj);
            GameManager.Instance.gameModeType = GameModeType.NONE;
            UIManager.Instance.QuitConfirmationPopup.SetActive(false);
            UIManager.Instance.GameUI.SetActive(false);
            UIManager.Instance.MainScreen.SetActive(true);
            UIManager.Instance.LoadingScreen.SetActive(true);
            GameManager.Instance.UICamera.SetActive(false);
            GameManager.Instance.UICamera.SetActive(true);
            GameManager.Instance.countOfAIPlayers=0;
            gameState = "none";
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", newPlayer.NickName);
        if (PhotonNetwork.IsMasterClient)
        {
            playersNameList.Add(newPlayer.NickName);
            pv.RPC("PlayerListInLobbySync", RpcTarget.All, playersNameList.ToArray());
        }
        if (PhotonNetwork.CurrentRoom.PlayerCount == 4)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            if (PhotonNetwork.IsMasterClient)
            {
                if (gameState != "InGame")
                {
                    StopCoroutine(lobbyTimerCoroutine);
                    GameManager.Instance.StartGameMultiplayer();
                }
            }
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        Debug.LogFormat("OnPlayerLeftRoom() {0}", otherPlayer.NickName);
        if (PhotonNetwork.IsMasterClient)
        {
            playersNameList.Remove(otherPlayer.NickName);
            pv.RPC("PlayerListInLobbySync", RpcTarget.All, playersNameList.ToArray());
        }
        if(gameState=="InLobby" && PhotonNetwork.CurrentRoom.PlayerCount < 4)
        {
            PhotonNetwork.CurrentRoom.IsOpen = true;
        }
        if (gameState == "InGame" && PhotonNetwork.CurrentRoom.PlayerCount == 1 && GameManager.Instance.countOfAIPlayers==0)
        {
            UIManager.Instance.VictoryPopup.SetActive(true);
            AdsManager.Instance.ShowInterstitialAdWithDelay();
            int totalCoins = PlayerPrefs.GetInt("Coins", 0) + 500;
            PlayerPrefs.SetInt("Coins", totalCoins);
            UIManager.Instance.UpdateCoinsStatus(PlayerPrefs.GetInt("Coins"));
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LeaveLobby();
        }
    }

    void IInRoomCallbacks.OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.IsMasterClient && gameState == "InLobby")
        {
            lobbyTimerCoroutine=StartCoroutine(LobbyCountDown());
        }
    }

    #endregion

    [PunRPC]
    public void PlayerListInLobbySync(string[] playerNames)
    {
        playersNameList.Clear();
        for(int i = 0; i < playerNames.Length; i++)
        {
            playersNameList.Add(playerNames[i]);
        }
        for(int i = 0; i < UIManager.Instance.lobbyPlayersNames.Length; i++)
        {
            UIManager.Instance.lobbyPlayersNames[i].text = "";
        }
        for (int i = 0; i < playersNameList.Count; i++)
        {
            UIManager.Instance.lobbyPlayersNames[i].text = playersNameList[i].Remove(playersNameList[i].Length - 5);
        }
    }

    [PunRPC]
    public void PlayerListInLobbySync(string playerName)
    {
        playersNameList.Clear();
        playersNameList.Add(playerName);
        for (int i = 0; i < UIManager.Instance.lobbyPlayersNames.Length; i++)
        {
            UIManager.Instance.lobbyPlayersNames[i].text = "";
        }
        for (int i = 0; i < playersNameList.Count; i++)
        {
            UIManager.Instance.lobbyPlayersNames[i].text = playersNameList[i].Remove(playersNameList[i].Length - 5);
        }
    }

    IEnumerator LobbyCountDown()
    {
        while (lobbyTimer > 0)
        {
            lobbyTimer--;
            yield return new WaitForSecondsRealtime(1f);
            pv.RPC("LobbyTimerSync", RpcTarget.All, lobbyTimer);
        }
        if (gameState != "InGame")
        {
            GameManager.Instance.StartGameMultiplayer();
        }
    }

    [PunRPC]
    void LobbyTimerSync(int time)
    {
        lobbyTimer = time;
        UIManager.Instance.lobbyTimer.text = lobbyTimer.ToString();
    }

}
