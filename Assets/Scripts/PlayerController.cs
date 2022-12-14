using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
using System.Collections;
using System.Collections.Generic;
using Photon;
using Photon.Pun;
using UnityEngine.UI;



public class PlayerController : MonoBehaviourPun, IPunObservable
{
    public string PlayerName="";
    public Image healthText;
    //[SerializeField]
    //private Vector2 JoystickSize = new Vector2(300, 300);
    [SerializeField]
    private Joystick Joystick;
    [SerializeField]
    public Rigidbody rb;
    public float speed;
    //private Finger MovementFinger;
    //private Vector2 MovementAmount;
    public Animator animator;
    public float Health = 100;
    private float MinHealth = 0;
    private float MaxHealth = 100;
    //private object PhotonTargets;
    public PhotonView view;
    public float speedPlayer = 1;
    public Canvas PlayerCanvas;
    public GameObject Pillow;
    public bool hasHit = false;
    //public object Photontargets { get; private set; }
    

    public void SetPlayerName(string name)
    {
        PlayerName = name;
    }

    private void Start()
    {
        if (view.IsMine)
        {
            Joystick = UIManager.Instance.JoyStick.GetComponent<FixedJoystick>();
        }
    }

    private void Update()
    {
        if (view.IsMine)
        {
            if (Joystick.Horizontal != 0 && Joystick.Vertical != 0)
            {
                animator.SetBool("Walking", true);
                Move(Joystick.Horizontal, Joystick.Vertical);
            }
            else
            {
                animator.SetBool("Walking", false);
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


    public void attack()
    {
        if (view.IsMine)
        {
            view.RPC("attackCall", RpcTarget.All);
            animator.SetTrigger("Attack");
            Invoke("attackDoneWithDelay", 1.5f);
        }
    }

    [PunRPC]
    public void attackCall()
    {
        hasHit = false;
        Pillow.tag = "Hit";
    }

    void attackDoneWithDelay()
    {
        view.RPC("attackDoneCall", RpcTarget.All);
    }

    [PunRPC]
    public void attackDoneCall()
    {
        Pillow.tag = "Pillow";
        hasHit = false;
    }


    //private void OnCollisionEnter(Collision other)
    //{
    //    Debug.Log("hittt "+other.gameObject.name);
    //    if (other.gameObject.tag == "Hit")
    //    {
    //        Debug.Log("hittt outside");
    //        if (view.IsMine)
    //        {
    //            Debug.Log("hittt");
    //            view.RPC("Damage", RpcTarget.All, PlayerName);
    //        }
    //    }
    //}

    public void Damage()
    {
        view.RPC("DamageRPC", RpcTarget.All);
    }


    [PunRPC]
    public void DamageRPC()
    {
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
        if (view.IsMine)
        {
            UIManager.Instance.DiePopup.SetActive(true);
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

