using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class warriorProjectile : MonoBehaviour
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
        
        if(randomProjectile(16)){
           if(collision.CompareTag("Enemy")){
            enemyHP = collision.GetComponent<EnemyHP>();
            enemyHP.TakeDamage(damage + 100);
            enemyHP.enemy.Stunned(2);
            Vector3 pos = Camera.main.WorldToScreenPoint(collision.transform.position);
            Destroy(gameObject);
            }else if(collision.CompareTag("Boss")){
            bossHP = collision.GetComponent<BossHP>();
            bossHP.TakeDamage(damage + 100);
            bossHP.enemy.Stunned(2);
            Destroy(gameObject);
            } 
        }else{
            if(collision.CompareTag("Enemy")){
            collision.GetComponent<EnemyHP>().TakeDamage(damage);
            Vector3 pos = Camera.main.WorldToScreenPoint(collision.transform.position);
            Destroy(gameObject);
            }else if(collision.CompareTag("Boss")){
            collision.GetComponent<BossHP>().TakeDamage(damage);
            Destroy(gameObject);
            }
        }   
    }

    private bool randomProjectile(int percent){
    int per = Random.Range(0,101);
        if(per <= percent){
            return true;
        }else{
            return false;
        }
    }
}