using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneHandler : Singleton<SceneHandler>
{
    public bool IsGamePlay=false;
    public PlayerTouchMovement MainPlayer;
    public GamePlayUIHnadler UIRef;
    // Start is called before the first frame update
    private void OnEnable()
    {
        IsGamePlay = true;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetUpUI()
    {
        UIRef.SetUpUI();
    }
    public void SetUpRefrences()
    {
        SetUpUI();
    }
}
