using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class SpawnPlayers : MonoBehaviour
{
    private GameObject PlayerPrefab;

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
    public float minz;
    public float maxz;
    public int i = 4;
    //public Vector3[] spawnPos;
    public GameObject[] characterPrefabs;
    private void Start()
    {
       
     Vector3[] SpawnRot;//= new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), Random.Range(minz, maxz));
        //Random.Range(0, i)
        SpawnRot = new Vector3[i];
        SpawnRot[0] = new Vector3(-0.138f, 0.666f, -3.059f);
        
        SpawnRot[1] = new Vector3(-0.112f, 0.666f, -0.704f);
        
        SpawnRot[2] = new Vector3(-2.058f, 0.666f, -0.704f);
        
        SpawnRot[3] = new Vector3(-2.058f, 0.666f, -3.057f);
        int selectedCharacter = PlayerPrefs.GetInt("selectedcharacter");
        PlayerPrefab = characterPrefabs[selectedCharacter];
        PhotonNetwork.Instantiate(PlayerPrefab.name, SpawnRot[Random.Range(0, i)],Quaternion.identity);
    }
}
