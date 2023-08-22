using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialog : MonoBehaviour
{
    public GameObject dialogBox;

    //对话框显示时长
    public float displayTime = 4.0f;
    //计时器
    private float timerDisplay;

    public Text dialogText;
    public AudioSource audioSource;
    public AudioClip comepleteTaskClip;

    private bool hasPlayed;
    public int openDialogNum;

    void Start()
    {
        dialogBox.SetActive(false);
        timerDisplay = -1;
        openDialogNum = 0;
        hasPlayed = false;
    }


    void Update()
    {
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                dialogBox.SetActive(false);
            }
        }
    }

    public void DisplayDialog()
    {
        timerDisplay = displayTime;
        dialogBox.SetActive(true);

        UIHealthBar.instance.hasTask = true;

        if (openDialogNum>=1)
        {
            if (openDialogNum % 2 != 0)
            {
                dialogText.text = "按 空格键 发射修理工具\n祝你好运";
            }
            else
            {
                dialogText.text = "一共有五个坏掉的机器人\n帮我修理好他们";
            }
        }

        if (openDialogNum>=6)
        {
            dialogText.text = "还在等什么，快去吧";
            openDialogNum = 0;
        }

        openDialogNum++;

        if (UIHealthBar.instance.fixedNum >= 5)
        {
            //已经完成任务，需要修改对话内容
            dialogText.text = "哦，伟大的Dugege，\n\n谢谢你，你真的太棒了";

            if (hasPlayed == false)
            {
                audioSource.PlayOneShot(comepleteTaskClip);
                hasPlayed = true;
            }
        }
    }
}
