using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrobberRotation : MonoBehaviour
{
    float currentThrobberRotation = 0;
    public float ThrobberRotationSpeed = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnEnable()
    {
        transform.rotation = Quaternion.identity;
        currentThrobberRotation = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        UpdateThrobberRotation();
    }

    //This function gets called like the "FixedUpdate" unity function
    private void UpdateThrobberRotation()
    {
        currentThrobberRotation += ThrobberRotationSpeed;
        transform.rotation = Quaternion.Euler(0, 0, -currentThrobberRotation);
    }
}
