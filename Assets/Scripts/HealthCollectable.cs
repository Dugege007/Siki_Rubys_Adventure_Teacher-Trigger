using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectable : MonoBehaviour
{
    public AudioClip audioClip;
    public GameObject effectParticle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("发生碰撞的对象是：" + collision);
        RubyController rubyController = collision.GetComponent<RubyController>();
        //当前发生触发检测的游戏物体对象身上是否有rubyController脚本
        if (rubyController != null)
        {
            //ruby是否满血
            if (rubyController.health < rubyController.maxHealth)
            {
                //ruby是不满血状态
                rubyController.ChangeHealth(50);
                rubyController.PlaySound(audioClip);
                Instantiate(effectParticle, transform.position + Vector3.up * 0.2f, Quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
