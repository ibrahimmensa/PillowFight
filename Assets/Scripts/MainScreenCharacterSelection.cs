using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScreenCharacterSelection : MonoBehaviour
{
    public GameObject[] characters;
   
    // Start is called before the first frame update
    void Start()
    {
        
       
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public void selectCharacter()
    {
        int selectedCharacter = PlayerPrefs.GetInt("selectedcharacter");

        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].SetActive(false);
        }
        characters[selectedCharacter].SetActive(true);
    }
}
