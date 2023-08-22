using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;
    public GameObject hitEffectParticle;

    public void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }

    private void Update()
    {
        //判断子弹移动距离是否过大
        if (transform.position.magnitude>50)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("当前子弹碰撞到的游戏物体是：" + collision.gameObject);
        GameObject particle = Instantiate(hitEffectParticle, transform.position + Vector3.up * 0.2f, Quaternion.identity);

        EnemyController enemyController = collision.gameObject.GetComponent<EnemyController>();
        if (enemyController!=null)
        {
            enemyController.Fix();
        }

        Destroy(gameObject);
        Destroy(particle, 1f);
    }
}
