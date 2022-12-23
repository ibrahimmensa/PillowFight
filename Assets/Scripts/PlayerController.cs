using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
using System.Collections;
using System.Collections.Generic;
using Photon;
using Photon.Pun;
using UnityEngine.UI;

public enum PlayerState
{
    IDLE,
    WALKING,
    ATTACK,
    BLOCK
}

public class PlayerController : MonoBehaviourPun, IPunObservable
{
    public string PlayerName="";
    public bool isAIPlayer=false;
    public Image healthText;
    [SerializeField]
    private Joystick Joystick;
    [SerializeField]
    public Rigidbody rb;
    public Animator animator;
    public float Health = 100;
    private float MinHealth = 0;
    private float MaxHealth = 100;
    public PhotonView view;
    public float speedPlayer = 0.65f;
    public Canvas PlayerCanvas;
    public GameObject Pillow;
    public bool hasHit = false;
    public bool hasBlock = false;
    public EnemyDetector EnemeyDetectionTriggerForAI;
    float distanceFromNearestPlayer = 10000;
    float delayBeforeEveryAIAttack = 0;

    public PlayerState playerState = PlayerState.IDLE;


    public void SetPlayerName(string name)
    {
        PlayerName = name;
    }

    private void Start()
    {
        if (GameManager.Instance.gameModeType == GameModeType.MULTIPLAYER)
        {
            if (view.IsMine)
            {
                if (isAIPlayer)
                {
                    view.RPC("setAIPlayer", RpcTarget.All);
                }
                else
                {
                    Joystick = UIManager.Instance.JoyStick.GetComponent<FixedJoystick>();
                }
            }
        }

        if (GameManager.Instance.gameModeType == GameModeType.SURVIVAL_MODE || GameManager.Instance.gameModeType == GameModeType.TIMER_MODE)
        {
            if (isAIPlayer)
            {
                isAIPlayer = true;
                EnemeyDetectionTriggerForAI.gameObject.SetActive(true);
                speedPlayer = 0.55f;
            }
            else
            {
                Joystick = UIManager.Instance.JoyStick.GetComponent<FixedJoystick>();
            }
        }
    }

    [PunRPC]
    void setAIPlayer()
    {
        isAIPlayer = true;
        EnemeyDetectionTriggerForAI.gameObject.SetActive(true);
        speedPlayer = 0.55f;
        view.OwnershipTransfer = OwnershipOption.Takeover;
    }

    private void Update()
    {
        if (view.IsMine || GameManager.Instance.gameModeType == GameModeType.SURVIVAL_MODE || GameManager.Instance.gameModeType == GameModeType.TIMER_MODE)
        {
            if (isAIPlayer)
            {
                GameObject currentEnemy = NearestPlayer();
                if (currentEnemy != null)
                {
                    if (distanceFromNearestPlayer <= 0.4f)
                    {
                        if (playerState == PlayerState.IDLE || playerState == PlayerState.WALKING)
                        {
                            delayBeforeEveryAIAttack += Time.deltaTime;
                            if (delayBeforeEveryAIAttack > 1.5f)
                            {
                                int actionPossibility = Random.Range(0, 10);
                                if (actionPossibility <= 5)
                                    attack();
                                else
                                    block();
                            }
                            else
                            {
                                animator.SetBool("Walking", false);
                                playerState = PlayerState.IDLE;
                            }
                        }
                    }
                    else
                    {
                        if (playerState == PlayerState.IDLE || playerState == PlayerState.WALKING)
                        {
                            delayBeforeEveryAIAttack = 0;
                            animator.SetBool("Walking", true);
                            MoveAI(currentEnemy.transform);
                            playerState = PlayerState.WALKING;
                        }
                    }
                }
                else
                {
                    animator.SetBool("Walking", false);
                    playerState = PlayerState.IDLE;
                }
            }
            else
            {
                if (Joystick.Horizontal != 0 && Joystick.Vertical != 0)
                {
                    if (playerState == PlayerState.IDLE || playerState == PlayerState.WALKING)
                    {
                        animator.SetBool("Walking", true);
                        Move(Joystick.Horizontal, Joystick.Vertical);
                        playerState = PlayerState.WALKING;
                    }
                }
                else
                {
                    animator.SetBool("Walking", false);
                    if(playerState == PlayerState.IDLE || playerState == PlayerState.WALKING)
                        playerState = PlayerState.IDLE;
                }
            }

            //___________________________PLAYER BOUNDARIES_____________________________
            if (transform.position.x > 0)
            {
                transform.position = new Vector3(0, transform.position.y, transform.position.z);
            }
            if (transform.position.x < -2)
            {
                transform.position = new Vector3(-2, transform.position.y, transform.position.z);
            }
            if (transform.position.z > -0.7f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, -0.7f);
            }
            if (transform.position.z < -3)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, -3);
            }
            //___________________________________________________________________________
        }

        PlayerCanvas.transform.LookAt(transform.position + GameManager.Instance.currentGameEnvironment.GetComponent<EnvironmentManager>().cam.transform.rotation * Vector3.back, GameManager.Instance.currentGameEnvironment.GetComponent<EnvironmentManager>().cam.transform.rotation * Vector3.up);
    }

    public void Move(float Horizontal, float Vertical)
    {
        Vector3 dir = new Vector3(Horizontal, 0, Vertical);
        rb.velocity = new Vector3(-Horizontal * speedPlayer, rb.velocity.y, -Vertical * speedPlayer);
        if (dir != Vector3.zero)
        {
            transform.LookAt(transform.position - dir);
        }
    }

    public void MoveAI(Transform enemy)
    {
        transform.LookAt(enemy);
        rb.velocity = transform.forward * speedPlayer;
    }

    GameObject NearestPlayer()
    {
        GameObject nearestPlayer=null;
        float distance = 10000;
        foreach (Collider player in EnemeyDetectionTriggerForAI.TriggerList)
        {
            try
            {
                float dist = Vector3.Distance(player.transform.position, transform.position);
                if (distance > dist)
                {
                    nearestPlayer = player.gameObject;
                    distance = dist;
                    distanceFromNearestPlayer = dist;
                }
            }
            catch
            {
                for(int i = 0; i < EnemeyDetectionTriggerForAI.TriggerList.Count; i++)
                {
                    if (EnemeyDetectionTriggerForAI.TriggerList[i] == null)
                        EnemeyDetectionTriggerForAI.TriggerList.RemoveAt(i);
                }
                break;
            }
        }
        return nearestPlayer;
    }

    public void attack()
    {
        if (GameManager.Instance.gameModeType == GameModeType.MULTIPLAYER)
        {
            if (view.IsMine)
            {
                if (playerState == PlayerState.IDLE || playerState == PlayerState.WALKING)
                {
                    view.RPC("attackCall", RpcTarget.All);
                    animator.SetBool("Walking", false);
                    animator.SetTrigger("Attack");
                    view.RPC("attackDoneCall", RpcTarget.All);
                }
            }
        }
        else if (GameManager.Instance.gameModeType == GameModeType.SURVIVAL_MODE || GameManager.Instance.gameModeType == GameModeType.TIMER_MODE)
        {
            if (playerState == PlayerState.IDLE || playerState == PlayerState.WALKING)
            {
                attackCall();
                animator.SetBool("Walking", false);
                animator.SetTrigger("Attack");
                attackDoneCall();
            }
        }
    }

    [PunRPC]
    public void attackCall()
    {
        playerState = PlayerState.ATTACK;
        delayBeforeEveryAIAttack = 0;
        hasHit = false;
        Invoke("attackWithDelay", 0.5f);
    }

    void attackWithDelay()
    {
        Pillow.GetComponent<BoxCollider>().enabled = true;
    }

    void attackDoneWithDelay()
    {
        Pillow.GetComponent<BoxCollider>().enabled = false;
        hasHit = false;
        playerState = PlayerState.IDLE;
    }

    [PunRPC]
    public void attackDoneCall()
    {
        Invoke("attackDoneWithDelay", 1.5f);
    }

    public void block()
    {
        if (GameManager.Instance.gameModeType == GameModeType.MULTIPLAYER)
        {
            if (view.IsMine)
            {
                if (playerState == PlayerState.IDLE || playerState == PlayerState.WALKING)
                {
                    view.RPC("blockCall", RpcTarget.All);
                    animator.SetBool("Walking", false);
                    //animator.SetTrigger("Block");
                    view.RPC("blockDoneCall", RpcTarget.All);
                }
            }
        }
        else if(GameManager.Instance.gameModeType == GameModeType.SURVIVAL_MODE || GameManager.Instance.gameModeType == GameModeType.TIMER_MODE)
        {
            if (playerState == PlayerState.IDLE || playerState == PlayerState.WALKING)
            {
                blockCall();
                animator.SetBool("Walking", false);
                //animator.SetTrigger("Block");
                blockDoneCall();
            }
        }
    }

    [PunRPC]
    public void blockCall()
    {
        hasBlock = true;
        playerState = PlayerState.BLOCK;
    }

    void blockDoneWithDelay()
    {
        hasBlock = false;
        playerState = PlayerState.IDLE;
    }

    [PunRPC]
    public void blockDoneCall()
    {
        Invoke("blockDoneWithDelay", 1.5f);
    }

    public void Damage(int damageAmount)
    {
        if (!hasBlock)
        {
            if (GameManager.Instance.gameModeType == GameModeType.MULTIPLAYER)
                view.RPC("DamageRPC", RpcTarget.All,damageAmount);
            else if (GameManager.Instance.gameModeType == GameModeType.SURVIVAL_MODE || GameManager.Instance.gameModeType == GameModeType.TIMER_MODE)
                DamageRPC(damageAmount);
        }
    }


    [PunRPC]
    public void DamageRPC(int damageAmount)
    {
        //hurt animation
        Health -= damageAmount;
        healthText.fillAmount = Health / MaxHealth;
        if (Health <= 0)
        {
            die();
        }
    }

    public void die()
    {
        if (GameManager.Instance.gameModeType == GameModeType.MULTIPLAYER)
        {
            if (isAIPlayer)
            {
                GameManager.Instance.countOfAIPlayers--;
                if (PhotonNetwork.CurrentRoom.PlayerCount == 1 && GameManager.Instance.countOfAIPlayers == 0 && PhotonNetwork.IsMasterClient)
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
            if (view.IsMine && !isAIPlayer)
            {
                UIManager.Instance.DiePopup.SetActive(true);
                AdsManager.Instance.ShowInterstitialAdWithDelay();
                PhotonNetwork.LeaveRoom();
                PhotonNetwork.LeaveLobby();
            }
        }

        else if(GameManager.Instance.gameModeType == GameModeType.SURVIVAL_MODE)
        {
            if (isAIPlayer)
            {
                //spawn next player
                GameManager.Instance.CurrentGameKillCount++;
                GameManager.Instance.SpawnNextAIPlayer();
            }
            else
            {
                if (GameManager.Instance.CurrentGameKillCount<5 && GameManager.Instance.CurrentGameKillCount > 0)
                {
                    int totalCoins = PlayerPrefs.GetInt("Coins", 0) + 250;
                    PlayerPrefs.SetInt("Coins", totalCoins);
                    UIManager.Instance.UpdateCoinsStatus(PlayerPrefs.GetInt("Coins"));
                }
                else if(GameManager.Instance.CurrentGameKillCount >=5)
                {
                    int totalCoins = PlayerPrefs.GetInt("Coins", 0) + 500;
                    PlayerPrefs.SetInt("Coins", totalCoins);
                    UIManager.Instance.UpdateCoinsStatus(PlayerPrefs.GetInt("Coins"));
                }
                GameManager.Instance.CurrentGameKillCount = 0;
                UIManager.Instance.GameOverPopupForSurvivalMode.SetActive(true);
                AdsManager.Instance.ShowInterstitialAdWithDelay();
                Destroy(GameManager.Instance.currentGameEnvironment);
                Destroy(PhotonManager.instance._playerObj);
                Destroy(GameManager.Instance.AIPlayer);
                UIManager.Instance.QuitConfirmationPopup.SetActive(false);
                UIManager.Instance.GameUI.SetActive(false);
                UIManager.Instance.MainScreen.SetActive(true);
                GameManager.Instance.UICamera.SetActive(false);
                GameManager.Instance.UICamera.SetActive(true);
                GameManager.Instance.gameModeType = GameModeType.NONE;
            }
        }

        else if (GameManager.Instance.gameModeType == GameModeType.TIMER_MODE)
        {
            if (isAIPlayer)
            {
                //spawn next player
                GameManager.Instance.CurrentGameKillCount++;
                GameManager.Instance.SpawnNextAIPlayer();
            }
            else
            {
                int rewardedCoins = GameManager.Instance.CurrentGameKillCount * 20;
                int totalCoins = PlayerPrefs.GetInt("Coins", 0) + rewardedCoins;
                PlayerPrefs.SetInt("Coins", totalCoins);
                UIManager.Instance.UpdateCoinsStatus(PlayerPrefs.GetInt("Coins"));
                GameManager.Instance.CurrentGameKillCount = 0;
                StopCoroutine(GameManager.Instance.TimerForTimerMode);
                UIManager.Instance.TimeTextForTimerMode.gameObject.SetActive(false);
                GameObject adbtn = UIManager.Instance.GameOverPopupForTimerMode.transform.Find("PopUp").Find("AdButton").gameObject;
                adbtn.GetComponent<Button>().interactable = false;
                adbtn.GetComponentInChildren<TMPro.TMP_Text>().alpha = 0.4f;
                UIManager.Instance.GameOverPopupForTimerMode.SetActive(true);
                AdsManager.Instance.ShowInterstitialAdWithDelay();
                Destroy(GameManager.Instance.currentGameEnvironment);
                Destroy(PhotonManager.instance._playerObj);
                Destroy(GameManager.Instance.AIPlayer);
                UIManager.Instance.QuitConfirmationPopup.SetActive(false);
                UIManager.Instance.GameUI.SetActive(false);
                UIManager.Instance.MainScreen.SetActive(true);
                GameManager.Instance.UICamera.SetActive(false);
                GameManager.Instance.UICamera.SetActive(true);
                GameManager.Instance.gameModeType = GameModeType.NONE;
            }
        }
        Destroy(gameObject);
    }
    

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(PlayerName);
        }
        else
        {
            PlayerName = (string)stream.ReceiveNext();
        }
    }
}

