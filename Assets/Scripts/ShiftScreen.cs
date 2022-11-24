using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShiftScreen : MonoBehaviour
{
    public GameObject MainScreen;
    public GameObject Secreen2;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Onclickbtn()
    {
        FindObjectOfType<AudioManager>().Play("click");
        MainScreen.SetActive(false);
        Secreen2.SetActive(true);
    }

    public void OnclickbtnPopUp()
    {
        //MainScreen.SetActive(false);
        FindObjectOfType<AudioManager>().Play("click");
        FindObjectOfType<UIManager>().PanelFadeIn();
        Secreen2.SetActive(true);
    }
}
