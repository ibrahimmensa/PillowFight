using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public int xPos;
   // public int zPos;
    public int enemyCount;
    private int waitTime = 1;
    public int enemyCountMax=4;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnemySpawn());
    }
    IEnumerator EnemySpawn()
    {
        while(enemyCount<enemyCountMax)
        {
            xPos = Random.Range(0, -2);
            Instantiate(enemy, new Vector3(xPos, 0.772f, -0.728f), Quaternion.Euler(0f, 180f, 0f));
            yield return new WaitForSeconds(waitTime);
            enemyCount += 1;
            waitTime += 1;
            enemyCountMax += 1;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
