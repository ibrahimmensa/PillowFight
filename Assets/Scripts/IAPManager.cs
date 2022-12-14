using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAPManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CoinsPurchased(int coins)
    {
        Debug.Log("Purchase Complete.........coins: "+coins);
        //int totalCoins;
        //totalCoins = PlayerPrefs.GetInt("Coins", 0) + coins;
        //PlayerPrefs.SetInt("Coins", totalCoins);
        //MenuHandler.Instance.mainMenu.updateStats();
    }
}
