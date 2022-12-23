using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PillowController : MonoBehaviour
{
    public PlayerController player;

    public int DamagePower = 20;

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
        if (!player.hasHit)
        {
            if (other.tag == "Player")
            {
                if (player.view.IsMine || GameManager.Instance.gameModeType == GameModeType.SURVIVAL_MODE || GameManager.Instance.gameModeType == GameModeType.TIMER_MODE)
                {
                    Debug.Log("hittt"+other.GetComponent<PlayerController>().PlayerName);
                    other.GetComponent<PlayerController>().Damage(DamagePower);
                    player.hasHit = true;
                }
            }
        }
    }
}