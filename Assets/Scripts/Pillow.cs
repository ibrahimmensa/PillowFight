using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Pillow : MonoBehaviour
{
    // Start is called before the first frame update
    //private PlayerTouchMovement reff;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        Debug.Log("Hit");
    //        //if (reff.photonView.IsMine)
    //        //{
    //        //    reff.photonView.RPC("Damage", RpcTarget.All);
    //        //}
    //    }


    //}
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Player")
        {
            Debug.Log("Hit");
        }
    }
}