using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Pillow : MonoBehaviour
{
    public PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (gameObject.tag == "Hit" && !player.hasHit)
        {
            if (other.tag == "Player")
            {
                if (player.view.IsMine)
                {
                    Debug.Log("hittt"+other.GetComponent<PlayerController>().PlayerName);
                    other.GetComponent<PlayerController>().Damage();
                    player.hasHit = true;
                }
            }
        }
    }
}
