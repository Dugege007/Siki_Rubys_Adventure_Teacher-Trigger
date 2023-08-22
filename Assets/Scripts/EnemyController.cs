using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rigidbody2d;

    //是否在垂直轴向
    public bool vertical;
    //移动方向
    private int direction = 1;
    //方向改变时间间隔
    public float changeTime = 3.0f;
    //计时器
    private float timer;

    private Animator animator;

    //当前机器人是否损坏
    private bool broken;

    public ParticleSystem smokeEffect;

    private AudioSource audioSource;
    public AudioClip fixedSound;
    public AudioClip[] hitSounds;

    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        timer = changeTime;
        //获取Animator
        animator = GetComponent<Animator>();
        //先执行一次判断
        //animator.SetFloat("MoveXY", direction);
        //animator.SetBool("Vertical", vertical);
        PlayMoveAnimation();
        broken = true;
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!broken)
        {
            //已修好，不再移动
            return;
        }

        timer -= Time.deltaTime;

        Vector2 position = rigidbody2d.position;

        if (timer < 0)
        {
            direction = -direction;
            //animator.SetFloat("MoveXY", direction);
            PlayMoveAnimation();
            timer = changeTime;
        }

        if (vertical)
        {
            position.y = position.y + Time.deltaTime * speed * direction;
        }
        else
        {
            position.x = position.x + Time.deltaTime * speed * direction;
        }

        rigidbody2d.MovePosition(position);
    }

    /// <summary>
    /// 触发检测
    /// </summary>
    /// <param name="collision">对象的碰撞组件</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        RubyController rubyController = collision.gameObject.GetComponent<RubyController>();
        if (rubyController != null)
        {
            rubyController.ChangeHealth(-20);
        }
    }

    /// <summary>
    /// 控制移动动画的方法
    /// </summary>
    private void PlayMoveAnimation()
    {
        if (vertical)//垂直轴向动画的控制
        {
            animator.SetFloat("MoveX", 0);
            animator.SetFloat("MoveY", direction);
        }
        else//水平轴向动画的控制
        {
            animator.SetFloat("MoveX", direction);
            animator.SetFloat("MoveY", 0);
        }
    }

    public void Fix()
    {
        broken = false;
        rigidbody2d.simulated = false;//使该物体不再发生碰撞效果
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
        int randomNum = Random.Range(0, 2);
        //float randomNum2 = Random.Range(1f, 2f);
        audioSource.Stop();
        audioSource.PlayOneShot(hitSounds[randomNum]);
        audioSource.PlayOneShot(fixedSound);
        Invoke("PlayFixedSound", 0.2f);
        UIHealthBar.instance.fixedNum++;
    }

    private void PlayFixedSound()
    {
        audioSource.PlayOneShot(fixedSound);
    }
}
