using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;

public static class ClipboardExtension
{
    /// <summary>
    /// Puts the string into the Clipboard.
    /// </summary>
    public static void CopyToClipboard(this string str)
    {
        GUIUtility.systemCopyBuffer = str;
    }
}

public class UIManager : Singleton<UIManager>
{
    // Start is called before the first frame update
    public float fadeTime = 1f;

    public TMP_InputField playerNameText;

    public GameObject MainScreen;
    public GameObject SettingsScreen;
    public GameObject ProfileScreen;
    public GameObject LeaderBoardScreen;
    public GameObject MultiPlayerScreen;
    public GameObject StoreScreen;
    public GameObject CharacterSelectionScreen;
    public GameObject StorePillowScreen;
    public GameObject StoreCoinsScreen;
    public GameObject PrivateRoomScreen;
    public GameObject CreatePrivateRoomScreen;
    public GameObject JoinPrivateRoomScreen;
    public GameObject LobbyScreen;
    public GameObject JoinPrivateRoomFailedPanel;
    public GameObject JoinPrivateRoomFullPanel;
    public GameObject InternetConnectionErrorPanel;
    public GameObject LoadingScreen;
    public GameObject GameUI;
    public TMP_Text[] lobbyPlayersNames;

    public GameObject BGMusicToogleOn;
    public GameObject BGMusicToogleOff;
    public GameObject SfxToogleOn;
    public GameObject SfxToogleOff;

    public TMP_Text privateRoomCodeText;
    public TMP_InputField privateRoomCodeInputField;

    public TMP_Text DebugText;

    public void PanelFadeIn(CanvasGroup canvasGroup, RectTransform recttransform)
    {
        Debug.Log("Working");
        canvasGroup.alpha = 0f;
        recttransform.transform.localPosition = new Vector3(0f, -1000f, 0f);
        recttransform.DOAnchorPos(new Vector2(0f, 0f), fadeTime, false).SetEase(Ease.OutElastic);
        canvasGroup.DOFade(1, fadeTime);
    }
    public void PanelFadeOut(CanvasGroup canvasGroup, RectTransform recttransform)
    {
        canvasGroup.alpha = 1f;
        recttransform.transform.localPosition = new Vector3(0f, 0f, 0f);
        recttransform.DOAnchorPos(new Vector2(0f, -1000f), fadeTime, false).SetEase(Ease.InOutQuint);
        canvasGroup.DOFade(0, fadeTime);
    }
    void Start()
    {
        if (PlayerPrefs.GetString("PlayerName", "") == "")
        {
            string playerName = "Player" + Random.Range(10000, 99999);
            PlayerPrefs.SetString("PlayerName", playerName);
            playerNameText.text = playerName;
        }
        else
        {
            playerNameText.text = PlayerPrefs.GetString("PlayerName");
        }
    }

    public void SetNewName(TMP_InputField newPlayerNameText)
    {
        if (newPlayerNameText.text != "")
        {
            PlayerPrefs.SetString("PlayerName", newPlayerNameText.text);
            playerNameText.text = newPlayerNameText.text;
        }
        else
        {
            playerNameText.text = PlayerPrefs.GetString("PlayerName");
        }
        PhotonNetwork.NickName = playerNameText.text + Random.Range(10000, 99999);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickSettingsButton()
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        PanelFadeIn(SettingsScreen.GetComponent<CanvasGroup>(),SettingsScreen.GetComponent<RectTransform>());
        SettingsScreen.SetActive(true);
    }

    public void onClickCloseSettings()
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        PanelFadeOut(SettingsScreen.GetComponent<CanvasGroup>(), SettingsScreen.GetComponent<RectTransform>());
        Invoke("CloseSettingsScreenAfterDelay", 1);
    }

    void CloseSettingsScreenAfterDelay()
    {
        SettingsScreen.SetActive(false);
    }


    public void OnClickProfileButton()
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        PanelFadeIn(ProfileScreen.GetComponent<CanvasGroup>(), ProfileScreen.GetComponent<RectTransform>());
        ProfileScreen.SetActive(true);
    }

    public void onClickCloseProfile()
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        PanelFadeOut(ProfileScreen.GetComponent<CanvasGroup>(), ProfileScreen.GetComponent<RectTransform>());
        Invoke("CloseProfileScreenAfterDelay", 1);
    }

    void CloseProfileScreenAfterDelay()
    {
        ProfileScreen.SetActive(false);
    }


    public void OnClickLeaderBoardButton()
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        PanelFadeIn(LeaderBoardScreen.GetComponent<CanvasGroup>(), LeaderBoardScreen.GetComponent<RectTransform>());
        LeaderBoardScreen.SetActive(true);
    }

    public void onClickCloseLeaderBoard()
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        PanelFadeOut(LeaderBoardScreen.GetComponent<CanvasGroup>(), LeaderBoardScreen.GetComponent<RectTransform>());
        Invoke("CloseLeaderBoardScreenAfterDelay", 1);
    }

    void CloseLeaderBoardScreenAfterDelay()
    {
        LeaderBoardScreen.SetActive(false);
    }


    public void OnClickMultiPlayerButton()
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        PanelFadeIn(MultiPlayerScreen.GetComponent<CanvasGroup>(), MultiPlayerScreen.GetComponent<RectTransform>());
        MultiPlayerScreen.SetActive(true);
    }

    public void onClickCloseMultiPlayer()
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        PanelFadeOut(MultiPlayerScreen.GetComponent<CanvasGroup>(), MultiPlayerScreen.GetComponent<RectTransform>());
        Invoke("CloseMultiPlayerScreenAfterDelay", 1);
    }

    void CloseMultiPlayerScreenAfterDelay()
    {
        MultiPlayerScreen.SetActive(false);
    }

    public void OnClickStoreButton()
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        PanelFadeIn(StoreScreen.GetComponent<CanvasGroup>(), StoreScreen.GetComponent<RectTransform>());
        StoreScreen.SetActive(true);
    }

    public void onClickCloseStore()
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        PanelFadeOut(StoreScreen.GetComponent<CanvasGroup>(), StoreScreen.GetComponent<RectTransform>());
        Invoke("CloseStoreScreenAfterDelay", 1);
    }

    void CloseStoreScreenAfterDelay()
    {
        StoreScreen.SetActive(false);
    }


    public void OnClickCharacterSelectionButton()
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        PanelFadeIn(CharacterSelectionScreen.GetComponent<CanvasGroup>(), CharacterSelectionScreen.GetComponent<RectTransform>());
        CharacterSelectionScreen.SetActive(true);
    }

    public void onClickCloseCharacterSelection()
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        PanelFadeOut(CharacterSelectionScreen.GetComponent<CanvasGroup>(), CharacterSelectionScreen.GetComponent<RectTransform>());
        Invoke("CloseCharacterSelectionScreenAfterDelay", 1);
    }

    void CloseCharacterSelectionScreenAfterDelay()
    {
        CharacterSelectionScreen.SetActive(false);
    }

    public void onClickPillowInStore()
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        StorePillowScreen.SetActive(true);
        StoreCoinsScreen.SetActive(false);
    }

    public void onClickCoinsInStore()
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        StorePillowScreen.SetActive(false);
        StoreCoinsScreen.SetActive(true);
    }

    public void onClickPrivateRoom()
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        PrivateRoomScreen.SetActive(true);
    }

    public void onClickClosePrivateRoom()
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        PrivateRoomScreen.SetActive(false);
    }

    public void onClickCreatePrivateRoom()
    {
        if (PhotonManager.instance.isPhotonConnected)
        {
            AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
            CreatePrivateRoomScreen.SetActive(true);
            PhotonManager.instance.RandomRoomCode();
            privateRoomCodeText.text = PhotonManager.instance.privateGameCode;
        }
        else
        {
            InternetConnectionErrorPanel.SetActive(true);
        }
    }

    public void onClickCloseCreatePrivateRoom(TMP_Text buttonText)
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        CreatePrivateRoomScreen.SetActive(false);
        privateRoomCodeText.text = "";
        buttonText.text = "COPY CODE";
    }

    public void onClickJoinPrivateRoom()
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        JoinPrivateRoomScreen.SetActive(true);
    }

    public void onClickCloseJoinPrivateRoom()
    {
        privateRoomCodeInputField.text = "";
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        JoinPrivateRoomScreen.SetActive(false);
    }

    public void onClickCopyCode(TMP_Text buttonText)
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        privateRoomCodeText.text.CopyToClipboard();
        buttonText.text = "COPIED";
    }

    public void onClickCloseLobby()
    {
        privateRoomCodeInputField.text = "";
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        PhotonNetwork.LeaveRoom();
    }

    public void onClickBGMusicToogle(bool toogle)
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        if (toogle)
        {
            PlayerPrefs.SetInt("BGMusic", 0); 
            AudioManager.Instance.BGMusic.enabled = true;
            AudioManager.Instance.BGMusic.Play();
        }
        else
        {
            PlayerPrefs.SetInt("BGMusic", 1);
            AudioManager.Instance.BGMusic.Stop();
            AudioManager.Instance.BGMusic.enabled = false;
        }
    }

    public void onClickSFXToogle(bool toogle)
    {
        if (toogle)
        {
            PlayerPrefs.SetInt("SFX", 0);
        }
        else
        {
            PlayerPrefs.SetInt("SFX", 1);
        }
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
    }
}
