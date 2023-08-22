using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RubyController : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;

    //最大生命值
    public int maxHealth = 100;
    //当前生命值
    private int currentHealth;
    public int health { get { return currentHealth; } }

    //ruby的无敌时间
    public float timeInvincible = 2.0f;//无敌时间常量
    private bool isInvincible;
    //当前计时器
    private float invincibleTimer;

    //移动速度
    public float speed = 8;

    private Vector2 lookDirection = new Vector2(1, 0);
    private Animator animator;

    public GameObject projectilePrefab;

    public AudioSource audioSource;
    public AudioSource walkAudioSource;

    public AudioClip playerHit;
    public AudioClip attackSound;
    public AudioClip walkSound;

    private Vector3 respawnPosition;

    //显示提示
    public Text tipsText;
    private bool isTipsDisplay;
    private float tipsDisplayTimer;

    // Start is called before the first frame update
    void Start()
    {
        //调整帧率
        Application.targetFrameRate = 120;
        rigidbody2d = GetComponent<Rigidbody2D>();

        currentHealth = maxHealth;

        animator = GetComponent<Animator>();

        //audioSource = GetComponent<AudioSource>();

        //重生点位置
        respawnPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //控制移动
        //玩家输入监听。传入水平和垂直的轴向系数
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        //当前玩家输入的某个轴向值不为0
        //Mathf.Approximately() 判断是否近似相等
        if (!Mathf.Approximately(move.x, 0) || !Mathf.Approximately(move.y, 0))
        {
            //将当前运动状态值传入lookDirection
            lookDirection.Set(move.x, move.y);
            //将lookDirection转化为单位向量
            lookDirection.Normalize();

            isTipsDisplay = true;

            if (!walkAudioSource.isPlaying)
            {
                walkAudioSource.Play();
                walkAudioSource.clip = walkSound;
            }
        }
        else
        {
            isTipsDisplay = false;

            walkAudioSource.Stop();
        }

        DisplayTips();

        //动画控制
        //将lookDirection的值传入Animator中的Parameters中的Look X Look Y
        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        //将move向量的模长传入Animator中的Parameters中的Speed
        animator.SetFloat("Speed", move.magnitude);

        //移动
        Vector2 position = transform.position;
        //position.x = position.x + speed * horizontal * Time.deltaTime;
        //position.y = position.y + speed * vertical * Time.deltaTime;
        ////transform.position = position;

        position = position + speed * move * Time.deltaTime;

        //使挂载此脚本的物体实时移动
        //Vector2 position = transform.position;
        //position.x = position.x + 0.1f;
        //transform.position = position;

        rigidbody2d.MovePosition(position);

        //无敌时间
        if (isInvincible)
        {
            invincibleTimer = invincibleTimer - Time.deltaTime;
            if (invincibleTimer <= 0)
            {
                isInvincible = false;
            }
        }

        //修理机器人 攻击
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Launch();
        }

        //检测是否与NPC对话
        if (Input.GetKeyDown(KeyCode.F))
        {
            //射线检测
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));      //Vector2.up为Y方向上的单位向量

            //判断射线是否碰撞到NPC
            if (hit.collider != null)
            {
                Debug.Log("当前射线检测碰撞的游戏物体是" + hit.collider.gameObject);
                NPCDialog npcDialog = hit.collider.GetComponent<NPCDialog>();
                if (npcDialog != null)
                {
                    npcDialog.DisplayDialog();
                }
            }
        }
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
            {
                return;
            }

            //受到伤害
            isInvincible = true;
            invincibleTimer = timeInvincible;
            animator.SetTrigger("Hit");
            PlaySound(playerHit);
        }

        //限制血量范围 Mathf.Clamp(需要限制的值, 最小值, 最大值)
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        Debug.Log("当前血量：" + currentHealth + "/最大血量：" + maxHealth);

        if (currentHealth<=0)
        {
            Respawn();
        }

        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    public float GetRubyHealthValue()
    {
        return currentHealth;
    }

    private void Launch()
    {
        if (!UIHealthBar.instance.hasTask)
        {
            return;
        }

        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.6f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 600);
        animator.SetTrigger("Launch");
        PlaySound(attackSound);
    }

    public void PlaySound(AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }

    private void Respawn()
    {
        ChangeHealth(maxHealth);
        transform.position = respawnPosition;
    }

    private void DisplayTips()
    {
        if (isTipsDisplay)
        {
            tipsText.text = " ";

            tipsDisplayTimer = 5;
        }
        else
        {
            if (tipsDisplayTimer >= 0)
            {
                tipsDisplayTimer -= Time.deltaTime;
                if (tipsDisplayTimer < 0)
                {
                    tipsText.text = "按 WASD 或 ↑↓←→  移动";
                }
            }
        }
    }
}
