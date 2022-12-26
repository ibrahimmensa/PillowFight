using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PillowController : MonoBehaviour
{
    public PlayerController player;

    public int power;
    public int grip;
    public int weight;

    public int DamagePower = 0;

    // Start is called before the first frame update
    void Start()
    {
        int pillowNumber = PlayerPrefs.GetInt("SelectedPillow" , 0);
        int upgradeLevel= PlayerPrefs.GetInt("UpgradeLevelPillow" + pillowNumber, 0);
        
        power = UIManager.Instance.pillowDataHandler.TotalPillows[pillowNumber].Upgrades[upgradeLevel].Power;
        grip = UIManager.Instance.pillowDataHandler.TotalPillows[pillowNumber].Upgrades[upgradeLevel].Grip;
        weight = UIManager.Instance.pillowDataHandler.TotalPillows[pillowNumber].Upgrades[upgradeLevel].Weight;
        if (GameManager.Instance.gameModeType == GameModeType.SURVIVAL_MODE || GameManager.Instance.gameModeType == GameModeType.TIMER_MODE)
        {
            DamagePower = power + (grip / 20) + (weight / 100);
        }
        else if(GameManager.Instance.gameModeType == GameModeType.MULTIPLAYER)
        {
            DamagePower = (power/2) + (grip / 20) + (weight / 200);
        }
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
