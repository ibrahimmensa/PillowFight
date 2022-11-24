using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public float fadeTime = 1f;
    public CanvasGroup canvasGroup;
    public RectTransform recttransform;



    public void PanelFadeIn()
    {
        Debug.Log("Working");
        canvasGroup.alpha = 0f;
        recttransform.transform.localPosition = new Vector3(0f, -1000f, 0f);
        recttransform.DOAnchorPos(new Vector2(0f, 0f), fadeTime, false).SetEase(Ease.OutElastic);
        canvasGroup.DOFade(1, fadeTime);
    }
    public void PanelFadeOut()
    {
        canvasGroup.alpha = 1f;
        recttransform.transform.localPosition = new Vector3(0f, 0f, 0f);
        recttransform.DOAnchorPos(new Vector2(0f, -1000f), fadeTime, false).SetEase(Ease.InOutQuint);
        canvasGroup.DOFade(0, fadeTime);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
