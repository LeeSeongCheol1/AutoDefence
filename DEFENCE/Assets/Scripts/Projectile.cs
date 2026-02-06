using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Movement2D movement2D;
    private Transform target;
    private float damage;
    private bool cri;

    public void Setup(Transform target, float damage, bool critical)
    {
        movement2D = GetComponent<Movement2D>();
        this.target = target;
        this.damage = damage;
        this.cri = critical;
    }

    private void Update()
    {
        if (target != null)
        {
            Vector3 direction = (target.position - transform.position).normalized;
            movement2D.MoveTo(direction);
        }
        else
        {
            // [변경] 파괴 대신 풀로 반환
            ProjectilePool.Instance.Return(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!(collision.CompareTag("Enemy") || collision.CompareTag("Boss"))) return;
        if (collision.transform != target) return;

        if (collision.CompareTag("Enemy"))
            collision.GetComponent<EnemyHP>().TakeDamage(damage);
        else if (collision.CompareTag("Boss"))
            collision.GetComponent<BossHP>().TakeDamage(damage);

        // [변경] 파괴 대신 풀로 반환
        PoolingManager.Instance.Return(gameObject);
    }
}