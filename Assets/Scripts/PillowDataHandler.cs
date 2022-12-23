using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Pillow
{
    public int Power;
    public int Grip;
    public int Weight;
}

[System.Serializable]
public class PillowsData
{
    public string Name;
    public string Material;
    public int priceInCoins;
    public int priceInAds;
    public Pillow[] Upgrades;
}

public class PillowDataHandler : MonoBehaviour
{
    public PillowsData[] TotalPillows;

    // Start is called before the first frame update
    void Start()
    {
        
    }
}
