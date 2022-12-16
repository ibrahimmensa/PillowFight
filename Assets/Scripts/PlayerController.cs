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
    public float speedPlayer = 1;
    public Canvas PlayerCanvas;
    public GameObject Pillow;
    public bool hasHit = false;
    public bool hasBlock = false;
    public EnemyDetector EnemeyDetectionTriggerForAI;
    float distanceFromNearestPlayer = 10000;

    public PlayerState playerState = PlayerState.IDLE;


    public void SetPlayerName(string name)
    {
        PlayerName = name;
    }

    private void Start()
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

    [PunRPC]
    void setAIPlayer()
    {
        isAIPlayer = true;
        EnemeyDetectionTriggerForAI.gameObject.SetActive(true);
        speedPlayer = 0.25f;
        view.OwnershipTransfer = OwnershipOption.Takeover;
    }

    private void Update()
    {
        if (view.IsMine)
        {
            if (isAIPlayer)
            {
                GameObject currentEnemy = NearestPlayer();
                if (currentEnemy != null)
                {
                    if (distanceFromNearestPlayer <= 0.5f)
                    {
                        attack();
                    }
                    else
                    {
                        if (playerState == PlayerState.IDLE || playerState == PlayerState.WALKING)
                        {
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

    [PunRPC]
    public void attackCall()
    {
        hasHit = false;
        Pillow.tag = "Hit";
        playerState = PlayerState.ATTACK;
    }

    void attackDoneWithDelay()
    {
        Pillow.tag = "Pillow";
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
        if (view.IsMine)
        {
            if (playerState == PlayerState.IDLE || playerState == PlayerState.WALKING)
            {
                view.RPC("blockCall", RpcTarget.All);
                animator.SetBool("Walking", false);
                //animator.SetTrigger("Block");
                Invoke("blockDoneWithDelay", 1.5f);
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
        view.RPC("blockDoneCall", RpcTarget.All);
    }

    [PunRPC]
    public void blockDoneCall()
    {
        hasBlock = false;
        playerState = PlayerState.IDLE;
    }

    public void Damage()
    {
        if (!hasBlock)
        {
            view.RPC("DamageRPC", RpcTarget.All);
        }
    }


    [PunRPC]
    public void DamageRPC()
    {
        //hurt animation
        Health -= 20;
        healthText.fillAmount = Health / MaxHealth;
        if (Health <= 0)
        {
            die();
        }
    }

    public void die()
    {
        Destroy(gameObject);
        if (view.IsMine && !isAIPlayer)
        {
            UIManager.Instance.DiePopup.SetActive(true);
            AdsManager.Instance.ShowInterstitialAdWithDelay();
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LeaveLobby();
        }
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

