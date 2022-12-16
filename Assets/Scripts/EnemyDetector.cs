using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    public List<Collider> TriggerList = new List<Collider>();

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
        if (other.tag == "Player")
        {
            //if the object is not already in the list
            if (!TriggerList.Contains(other))
            {
                //add the object to the list
                TriggerList.Add(other);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            //if the object is in the list
            if (TriggerList.Contains(other))
            {
                //remove it from the list
                TriggerList.Remove(other);
            }
        }
    }
}
