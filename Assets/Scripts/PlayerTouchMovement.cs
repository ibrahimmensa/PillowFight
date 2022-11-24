using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.EnhancedTouch;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
using System.Collections;
using System.Collections.Generic;
using Photon;
using Photon.Pun;
using UnityEngine.UI;



public class PlayerTouchMovement : MonoBehaviourPun
{
    public Image healthText;
    [SerializeField]
    private Vector2 JoystickSize = new Vector2(300, 300);
    [SerializeField]
    private FloatingJoystick Joystick;
    [SerializeField]
    public Rigidbody rb;
    public float speed;
    private Finger MovementFinger;
    private Vector2 MovementAmount;
    public Rigidbody _rigidbody;
    public Animator animator;
    private float Health = 100;
    private float MinHealth = 0;
    private float MaxHealth = 100;
    private object PhotonTargets;
    PhotonView view;
    public float speedPlayer = 1;
    public Canvas PlayerCanvas;
    public object Photontargets { get; private set; }

    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        if(photonView.IsMine)
        {
            SceneHandler.Instance.MainPlayer = this;
            SceneHandler.Instance.SetUpRefrences();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("Hit"+collision.transform.name);
            
        
        if (collision.gameObject.tag == "Enemy")
        {
            if (view.IsMine)
            {
                view.RPC("Damage", RpcTarget.All);
            }
        }
        

    }
    [PunRPC]
    void Damage()
    {
        Health -= 20;
        healthText.fillAmount = Health / MaxHealth;
        if(Health<=0)
        {
            die();
        }
    }
   
    private void Start()
    {
        
        view = GetComponent<PhotonView>();
        if (!view.IsMine)
        {
            Destroy(GetComponentInChildren<Cinemachine.CinemachineVirtualCamera>().gameObject);
        }
    }
    private void Update()
    {
        //Player.Move(scaledMovement);
       
        //if (view.IsMine)
        //{

        //    Vector3 scaledMovement = speed * Time.deltaTime * new Vector3(MovementAmount.x, 0, MovementAmount.y);

        //    Player.transform.LookAt(Player.transform.position + scaledMovement, Vector3.up);
        //    if (MovementFinger != null)
        //        animator.SetBool("Walking", true);
        //    else
        //        animator.SetBool("Walking", false);
        //    Player.velocity = new Vector3(MovementAmount.x * speed, 0, MovementAmount.y * speed);
        //}
       // healthText.text = Health.ToString();
    }
   
   
    public void attack()
    {

        if (view.IsMine)
        {
            //view.RPC("attackCall", RpcTarget.All, view.ViewID);
            animator.SetTrigger("Attack");
        }

        
    }
    [PunRPC]
    public void attackCall(int idphoton)
    {
        if (idphoton == view.ViewID)
        {
            animator.SetTrigger("Attack");
        }

    }
    
    //public void defend()
    //{
    //    if (ETouch.Touch.onFingerDown)
    //        animator.SetBool("Defend", true);
    //    else
    //        animator.SetBool("Defend", false);
    //}
    public virtual void OnPhotonSerializerView(PhotonStream stream,PhotonInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(Health);
        }
        else if(stream.IsReading)
        {
            Health = (float)stream.ReceiveNext();
        }
    }
    public void die()
    {

        Destroy(gameObject);


    }

    public void Move(float Horizontal, float Vertical)
    {
        Vector3 dir = new Vector3(Horizontal, 0, Vertical);
        rb.velocity = new Vector3(Horizontal * speedPlayer, rb.velocity.y, Vertical * speedPlayer);
        if (dir != Vector3.zero)
        {
            transform.LookAt(transform.position + dir);
        }
        //var input = new Vector2(Horizontal, Vertical);
        //Vector2 inputDir = input.normalized;
        //if (inputDir != Vector2.zero)
        //{
        //    transform.eulerAngles = Vector3.up * Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg;
        //    transform.Translate(transform.forward * (inputDir.magnitude * speedPlayer) * Time.deltaTime, Space.Self);
        //}
        
    }
}

