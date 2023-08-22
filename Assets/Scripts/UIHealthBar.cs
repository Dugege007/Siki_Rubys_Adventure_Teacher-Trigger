using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    public Image mask;
    float originalSize;

    public static UIHealthBar instance { get; private set; }

    public bool hasTask;
    //public bool ifCompleteTask;
    public int fixedNum;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //将遮罩的宽度赋值给原始尺寸变量
        originalSize = mask.rectTransform.rect.width;
    }

    void Update()
    {
        
    }

    /// <summary>
    /// 设置当前UI血条的显示值
    /// </summary>
    /// <param name="fillPercent">填充百分比</param>
    public void SetValue(float fillPercent)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize * fillPercent);
    }
}
