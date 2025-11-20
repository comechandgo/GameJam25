using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class 过场脚本 : MonoBehaviour
{
    public Image guochangimage; 
    public Image theButton;
    float imageMoveSpeed = 100f;
    float alpha = 0f;
    // Start is called before the first frame update
    void Start()
    {
        UpdateAlpha();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("鼠标按下");
            theButton.rectTransform.localPosition *= 0.8f;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("鼠标抬起");
            theButton.rectTransform.localPosition /= 0.8f;
            alpha = 1f;
            UpdateAlpha();
        }
    }
    //这是一个单独设置透明度的方法，使用这个方法可以调整透明度
    public void UpdateAlpha()
    {
        Color nowColor = guochangimage.color;
        nowColor.a = alpha;
        guochangimage.color = nowColor;
    }
}
//更新了注释

