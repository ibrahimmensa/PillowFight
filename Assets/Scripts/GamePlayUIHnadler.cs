using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayUIHnadler : MonoBehaviour
{
    public Joystick JoystickMov;
    public Animator animator;
    public PlayerController PlayerRef;
    public float speedPlayer;
    public float rotationSpeed;
    // Start is called before the first frame update
    private void OnEnable()
    {
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(PlayerRef!=null&&SceneHandler.Instance.IsGamePlay)
        {
            if(JoystickMov.Horizontal!=0&&JoystickMov.Vertical!=0)
            {
                animator.SetBool("Walking", true);
                //PlayerRef.transform.Translate(new Vector3(JoystickMov.Horizontal, 0, JoystickMov.Vertical)*speedPlayer);
                //PlayerRef.GetComponent<Rigidbody>().velocity = new Vector3(JoystickMov.Horizontal * speedPlayer, 0, JoystickMov.Vertical * speedPlayer);
                //float rotate = JoystickMov.Horizontal * rotationSpeed * Time.deltaTime;
                //float translate = JoystickMov.Vertical * speedPlayer * Time.deltaTime;
                //PlayerRef.transform.Translate(0, 0, translate);
                //PlayerRef.transform.Rotate(0, rotate, 0);
                //PlayerRef.transform.rotation = Quaternion.LookRotation(PlayerRef.GetComponent<Rigidbody>().velocity);
                //PlayerRef.transform.rotation = Quaternion.LookRotation(PlayerRef.transform.position + Vector3.up * 2f - Camera.main.transform.position, Vector3.up);
                //Vector2 convertedXY = ConvertWithCamera(Camera.main.transform.position, JoystickMov.Horizontal, JoystickMov.Vertical);
                //Vector3 direction = new Vector3(convertedXY.x, 0, convertedXY.y).normalized;
                //Vector3 lookAtPosition = PlayerRef.transform.position + direction;
                //PlayerRef.transform.LookAt(lookAtPosition);
                //Vector3 lookAtPosition = PlayerRef.transform.position + Quaternion.Euler;
                PlayerRef.Move(JoystickMov.Horizontal, JoystickMov.Vertical);
            }
            else
            {
                animator.SetBool("Walking", false);
            }
        }
    }
    public void SetUpUI()
    {
        PlayerRef = SceneHandler.Instance.MainPlayer;
        animator = PlayerRef.gameObject.GetComponent<Animator>();
    }
    public void attack()
    {

        //if (view.IsMine)
        //{
            //view.RPC("attackCall", RpcTarget.All, view.ViewID);
            animator.SetTrigger("Attack");
        //}


    }
  

    private Vector2 ConvertWithCamera(Vector3 cameraPos, float hor, float ver)
    {
        Vector2 joyDirection = new Vector2(hor, ver).normalized;
        Vector2 camera2DPos = new Vector2(cameraPos.x, cameraPos.z);
        Vector2 playerPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 cameraToPlayerDirection = (Vector2.zero - camera2DPos).normalized;
        float angle = Vector2.SignedAngle(cameraToPlayerDirection, new Vector2(0, 1));
        Vector2 finalDirection = RotateVector(joyDirection, -angle);
        return finalDirection;
    }

    public Vector2 RotateVector(Vector2 v, float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        float _x = v.x * Mathf.Cos(radian) - v.y * Mathf.Sin(radian);
        float _y = v.x * Mathf.Sin(radian) + v.y * Mathf.Cos(radian);
        return new Vector2(_x, _y);
    }
}
