using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] characters;
    public int selectedcharacter = 0;

    public void NextCharacter()
    {
        FindObjectOfType<AudioManager>().Play("click");
        characters[selectedcharacter].SetActive(false);
        selectedcharacter = (selectedcharacter + 1) % characters.Length;
        characters[selectedcharacter].SetActive(true);
    }
    public void previousCharacter()
    {
        FindObjectOfType<AudioManager>().Play("click");
        characters[selectedcharacter].SetActive(false);
        selectedcharacter--;
        if(selectedcharacter<0)
        {
            selectedcharacter += characters.Length;
        }
        characters[selectedcharacter].SetActive(true);
    }
    public void StartGame()
    {
        FindObjectOfType<AudioManager>().Play("click");
        PlayerPrefs.SetInt("selectedcharacter", selectedcharacter);
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
