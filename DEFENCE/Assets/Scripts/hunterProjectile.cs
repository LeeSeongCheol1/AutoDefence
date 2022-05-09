using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hunterProjectile : MonoBehaviour
{
    private Movement2D movement2D;
    private Transform target;
    private float damage;
    private bool cri;
    EnemyHP enemyHP;
    BossHP bossHP;
    float hpPercent;

    public void Setup(Transform target,float damage,bool critical)
    {
        movement2D = GetComponent<Movement2D>();
        this.target = target;
        this.damage = damage;
        this.cri = critical;
    }

    private void Update()
    {
        if(target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!(collision.CompareTag("Enemy") || collision.CompareTag("Boss"))){
            return;
        }
        if (collision.transform != target) return;
        
        if(collision.CompareTag("Enemy")){
            enemyHP = collision.GetComponent<EnemyHP>();
            enemyHP.TakeDamage(damage);
            Vector3 pos = Camera.main.WorldToScreenPoint(collision.transform.position);
            Destroy(gameObject);
            hpPercent = enemyHP.currentHP/enemyHP.maxHP;
            if(hpPercent<=0.05f){
                enemyHP.enemy.OnDie(EnemyDestroyType.Kill);
            }
        }else if(collision.CompareTag("Boss")){
            bossHP = collision.GetComponent<BossHP>();
            bossHP.TakeDamage(damage);
            Destroy(gameObject);
            hpPercent = bossHP.currentHP/bossHP.maxHP;
            if(hpPercent<=0.05f){
                bossHP.enemy.OnDie(BossDestroyType.Kill);
            }
        }
              
    }
}
