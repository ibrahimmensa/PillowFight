using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using DanielLochner.Assets.SimpleScrollSnap;

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
    public GameObject NotEnoughCoinsForMultiplayerErrorPanel;
    public GameObject LoadingScreen;

    public GameObject GameUI;
    public GameObject JoyStick;
    public TMP_Text[] lobbyPlayersNames;
    public TMP_Text lobbyTimer;

    public GameObject QuitConfirmationPopup;
    public GameObject VictoryPopup;
    public GameObject DiePopup;
    public GameObject GameOverPopupForTimerMode;
    public GameObject GameOverPopupForSurvivalMode;

    public TMP_Text[] coinsAmountText;
    public TMP_Text TimeTextForTimerMode;

    public GameObject BGMusicToogleOn;
    public GameObject BGMusicToogleOff;
    public GameObject SfxToogleOn;
    public GameObject SfxToogleOff;

    public TMP_Text privateRoomCodeText;
    public TMP_InputField privateRoomCodeInputField;

    public PillowDataHandler pillowDataHandler;
    public Slider PillowPower;
    public TMP_Text PillowGrip;
    public TMP_Text PillowWeight;
    public TMP_Text PillowName;

    public GameObject BuyPillowWithNoAdOption;
    public GameObject BuyPillowWithAdOption;
    public TMP_Text PillowPriceTextIfNoAd;
    public TMP_Text PillowPriceTextIfAd;
    public TMP_Text PillowAdsPriceText;
    public GameObject UpGradePillow;
    public TMP_Text UpGradePillowText;
    public GameObject UpGradePillowWithCoins;
    public GameObject UpGradePillowWithAd;

    public TMP_Text DebugText;

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

    #region ______________________NAME/COINS_UPDATE_FUNCTIONS_____________________

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

    public void UpdateCoinsStatus(int coins)
    {
        for (int i = 0; i < coinsAmountText.Length; i++)
            coinsAmountText[i].text = coins.ToString();
    }

    #endregion

    #region _________________PANEL_OPEN/CLOSE_ANIMATION_FUNCTIONS_________________

    public void PanelOpenFadeIn(GameObject Panel)
    {
        CanvasGroup canvasGroup = Panel.GetComponent<CanvasGroup>();
        RectTransform recttransform = Panel.GetComponent<RectTransform>();
        Debug.Log("Working");
        canvasGroup.alpha = 0f;
        recttransform.transform.localPosition = new Vector3(0f, -1000f, 0f);
        recttransform.DOAnchorPos(new Vector2(0f, 0f), fadeTime, false).SetEase(Ease.OutElastic);
        canvasGroup.DOFade(1, fadeTime);
        Panel.SetActive(true);
    }
    public void PanelCloseFadeOut(GameObject Panel)
    {
        CanvasGroup canvasGroup = Panel.GetComponent<CanvasGroup>();
        RectTransform recttransform = Panel.GetComponent<RectTransform>();
        canvasGroup.alpha = 1f;
        recttransform.transform.localPosition = new Vector3(0f, 0f, 0f);
        recttransform.DOAnchorPos(new Vector2(0f, -1000f), fadeTime, false).SetEase(Ease.InOutQuint);
        canvasGroup.DOFade(0, fadeTime);
        StartCoroutine(ClosePanelAfterDelay(Panel));
    }

    IEnumerator ClosePanelAfterDelay(GameObject Panel)
    {
        yield return new WaitForSeconds(0.6f);
        Panel.SetActive(false);
    }

    #endregion

    #region ________________________MAIN_SCREEN_UI_CONTROLLS______________________

    public void OnClickSettingsButton()
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        PanelOpenFadeIn(SettingsScreen);
    }

    public void onClickCloseSettings()
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        PanelCloseFadeOut(SettingsScreen);
    }

    public void OnClickProfileButton()
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        PanelOpenFadeIn(ProfileScreen);
    }

    public void onClickCloseProfile()
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        PanelCloseFadeOut(ProfileScreen);
    }


    public void OnClickLeaderBoardButton()
    {
        FirebaseManager.Instance.onClickLeaderboardButton();
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        PanelOpenFadeIn(LeaderBoardScreen);
    }

    public void onClickCloseLeaderBoard()
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        PanelCloseFadeOut(LeaderBoardScreen);
    }


    public void OnClickMultiPlayerButton()
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        PanelOpenFadeIn(MultiPlayerScreen);
    }

    public void onClickCloseMultiPlayer()
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        PanelCloseFadeOut(MultiPlayerScreen);
    }

    public void OnClickStoreButton(SimpleScrollSnap scrollView)
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        int i = PlayerPrefs.GetInt("SelectedPillow", 0);
        scrollView.GoToPanel(i);
        PanelOpenFadeIn(StoreScreen);
    }

    public void onClickCloseStore()
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        PanelCloseFadeOut(StoreScreen);
    }


    public void OnClickCharacterSelectionButton()
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        PanelOpenFadeIn(CharacterSelectionScreen);
    }

    public void onClickCloseCharacterSelection()
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        PanelCloseFadeOut(CharacterSelectionScreen);
    }

    #endregion

    #region ____________________________STORE_CONTROLLS___________________________

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

    public void onPillowSelected(SimpleScrollSnap scrollView)
    {
        BuyPillowWithAdOption.SetActive(false);
        BuyPillowWithNoAdOption.SetActive(false);
        UpGradePillow.SetActive(false);
        UpGradePillowWithCoins.SetActive(false);
        UpGradePillowWithAd.SetActive(false);

        if (scrollView.CenteredPanel == 0)
        {
            UpGradePillow.SetActive(true);
            int upgradeLevel = PlayerPrefs.GetInt("UpgradeLevelPillow" + scrollView.CenteredPanel, 0);
            PillowName.text = pillowDataHandler.TotalPillows[scrollView.CenteredPanel].Material;
            PillowGrip.text = pillowDataHandler.TotalPillows[scrollView.CenteredPanel].Upgrades[upgradeLevel].Grip.ToString() + "%";
            PillowWeight.text = pillowDataHandler.TotalPillows[scrollView.CenteredPanel].Upgrades[upgradeLevel].Weight.ToString() + " grams";
            PillowPower.value = pillowDataHandler.TotalPillows[scrollView.CenteredPanel].Upgrades[upgradeLevel].Power;

            UpGradePillowText.text = "Upgrade: " + upgradeLevel + " / 6";

            if (upgradeLevel < 3)
            {
                UpGradePillowWithCoins.SetActive(true);
            }
            else if (upgradeLevel >= 3 && upgradeLevel < 6)
            {
                UpGradePillowWithAd.SetActive(true);
            }
        }
        else
        {
            if(PlayerPrefs.GetInt("Pillow" + scrollView.CenteredPanel, 0) == 0)
            {
                //buy
                PillowName.text = pillowDataHandler.TotalPillows[scrollView.CenteredPanel].Material;
                PillowGrip.text = pillowDataHandler.TotalPillows[scrollView.CenteredPanel].Upgrades[0].Grip.ToString() + "%";
                PillowWeight.text = pillowDataHandler.TotalPillows[scrollView.CenteredPanel].Upgrades[0].Weight.ToString() + " grams";
                PillowPower.value = pillowDataHandler.TotalPillows[scrollView.CenteredPanel].Upgrades[0].Power;

                if (pillowDataHandler.TotalPillows[scrollView.CenteredPanel].priceInAds == 0)
                {
                    BuyPillowWithNoAdOption.SetActive(true);
                    PillowPriceTextIfNoAd.text = pillowDataHandler.TotalPillows[scrollView.CenteredPanel].priceInCoins.ToString();
                }
                else
                {
                    BuyPillowWithAdOption.SetActive(true);
                    PillowPriceTextIfAd.text = pillowDataHandler.TotalPillows[scrollView.CenteredPanel].priceInCoins.ToString();
                    int AdsWatched = PlayerPrefs.GetInt("AdsWatchedForPillow" + scrollView.CenteredPanel, 0);
                    PillowAdsPriceText.text = AdsWatched + "/" + pillowDataHandler.TotalPillows[scrollView.CenteredPanel].priceInAds.ToString();
                }
            }
            else
            {
                //upgrade
                UpGradePillow.SetActive(true);
                int upgradeLevel = PlayerPrefs.GetInt("UpgradeLevelPillow" + scrollView.CenteredPanel, 0);
                PillowName.text = pillowDataHandler.TotalPillows[scrollView.CenteredPanel].Material;
                PillowGrip.text = pillowDataHandler.TotalPillows[scrollView.CenteredPanel].Upgrades[upgradeLevel].Grip.ToString() + "%";
                PillowWeight.text = pillowDataHandler.TotalPillows[scrollView.CenteredPanel].Upgrades[upgradeLevel].Weight.ToString() + " grams";
                PillowPower.value = pillowDataHandler.TotalPillows[scrollView.CenteredPanel].Upgrades[upgradeLevel].Power;

                UpGradePillowText.text = "Upgrade: " + upgradeLevel + " / 6";

                if (upgradeLevel < 3)
                {
                    UpGradePillowWithCoins.SetActive(true);
                }
                else if (upgradeLevel >= 3 && upgradeLevel < 6)
                {
                    UpGradePillowWithAd.SetActive(true);
                }
            }
        }
    }

    #endregion

    #region ________________________MULTIPLAYER_UI_CONTROLLS______________________

    public void onClickPrivateRoom()
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        PanelOpenFadeIn(PrivateRoomScreen);
    }

    public void onClickClosePrivateRoom()
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        PanelCloseFadeOut(PrivateRoomScreen);
    }

    public void onClickCreatePrivateRoom()
    {
        if (PhotonManager.instance.isPhotonConnected)
        {
            AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
            if (PlayerPrefs.GetInt("Coins", 0) >= 500)
            {
                int totalCoins = PlayerPrefs.GetInt("Coins", 0) - 500;
                PlayerPrefs.SetInt("Coins", totalCoins);
                UpdateCoinsStatus(PlayerPrefs.GetInt("Coins"));

                PanelOpenFadeIn(CreatePrivateRoomScreen);
                PhotonManager.instance.RandomRoomCode();
                privateRoomCodeText.text = PhotonManager.instance.privateGameCode;
            }
            else
            {
                PanelOpenFadeIn(NotEnoughCoinsForMultiplayerErrorPanel);
            }
        }
        else
        {
           PanelOpenFadeIn(InternetConnectionErrorPanel);
        }
    }

    public void onClickCloseCreatePrivateRoom(TMP_Text buttonText)
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        PanelCloseFadeOut(CreatePrivateRoomScreen);
        privateRoomCodeText.text = "";
        buttonText.text = "COPY CODE";
    }

    public void onClickJoinPrivateRoom()
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        PanelOpenFadeIn(JoinPrivateRoomScreen);
    }

    public void onClickCloseJoinPrivateRoom()
    {
        privateRoomCodeInputField.text = "";
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        PanelCloseFadeOut(JoinPrivateRoomScreen);
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
        PhotonNetwork.LeaveLobby();
    }

    #endregion

    #region _______________________SOUND_CONTROLLS_+_SETTINGS_____________________

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

    public void onClickPrivacyPolicy()
    {
        Application.OpenURL("https://mensaplay.com/wensa/privacy-policy.html");
    }

    #endregion

    #region _________________________GAMEPLAY_UI_CONTROLLS________________________

    public void onClickAttack()
    {
        PhotonManager.instance._playerObj.GetComponent<PlayerController>().attack();
    }

    public void onClickBlock()
    {
        PhotonManager.instance._playerObj.GetComponent<PlayerController>().block();
    }

    public void onClickQuitGame()
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        QuitConfirmationPopup.SetActive(true);
        if (GameManager.Instance.gameModeType == GameModeType.SURVIVAL_MODE || GameManager.Instance.gameModeType == GameModeType.TIMER_MODE)
        {
            Time.timeScale = 0;
        }
    }

    public void onClickYesQuit()
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        if (GameManager.Instance.gameModeType == GameModeType.MULTIPLAYER)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LeaveLobby();
        }
        else if (GameManager.Instance.gameModeType == GameModeType.SURVIVAL_MODE)
        {
            Time.timeScale = 1;
            if (GameManager.Instance.CurrentGameKillCount < 5 && GameManager.Instance.CurrentGameKillCount > 0)
            {
                int totalCoins = PlayerPrefs.GetInt("Coins", 0) + 250;
                PlayerPrefs.SetInt("Coins", totalCoins);
                UpdateCoinsStatus(PlayerPrefs.GetInt("Coins"));
            }
            else if (GameManager.Instance.CurrentGameKillCount >= 5)
            {
                int totalCoins = PlayerPrefs.GetInt("Coins", 0) + 500;
                PlayerPrefs.SetInt("Coins", totalCoins);
                UpdateCoinsStatus(PlayerPrefs.GetInt("Coins"));
            }
            GameManager.Instance.CurrentGameKillCount = 0;
            Destroy(GameManager.Instance.currentGameEnvironment);
            Destroy(PhotonManager.instance._playerObj);
            Destroy(GameManager.Instance.AIPlayer);
            QuitConfirmationPopup.SetActive(false);
            GameUI.SetActive(false);
            MainScreen.SetActive(true);
            GameManager.Instance.UICamera.SetActive(false);
            GameManager.Instance.UICamera.SetActive(true);
            GameManager.Instance.gameModeType = GameModeType.NONE;
        }
        else if (GameManager.Instance.gameModeType == GameModeType.TIMER_MODE)
        {
            StopCoroutine(GameManager.Instance.TimerForTimerMode);
            GameManager.Instance.TimerModeGameplayOver();
        }
    }
    public void onClickNoQuit()
    {
        AudioManager.Instance.Play(SoundEffect.BUTTONCLICK);
        QuitConfirmationPopup.SetActive(false);
        if (GameManager.Instance.gameModeType == GameModeType.SURVIVAL_MODE || GameManager.Instance.gameModeType == GameModeType.TIMER_MODE)
        {
            Time.timeScale = 1;
        }
    }

    #endregion

}
