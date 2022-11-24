using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Health : MonoBehaviourPunCallbacks,IPunObservable
{
    public int health = 100;
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //throw new System.NotImplementedException();
        if (stream.IsWriting)
            stream.SendNext(health);
        else
            health = (int)stream.ReceiveNext();
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(health);
    }
}
