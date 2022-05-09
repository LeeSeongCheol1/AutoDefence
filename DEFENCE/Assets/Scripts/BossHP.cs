using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHP : MonoBehaviour
{
    [SerializeField]
    public float maxHP;
    public float currentHP;
    public bool isDie = false;
    public Boss enemy;
    private SpriteRenderer spriteRenderer;

    public float MaxHP => maxHP;
    public float CurrentHP => currentHP;

    private void Awake()
    {
        currentHP = maxHP;
        enemy = GetComponent<Boss>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage(float damage)
    {
        // 죽어있으면 실행 x(바로 메소드 탈출)
        if (isDie == true) return;

        currentHP -= damage;
        // 기존에 실행되어있는거 고려해서 중단하고 다시 시작
        StopCoroutine("HitAlphaAnimation");
        StartCoroutine("HitAlphaAnimation");

        if (currentHP <= 0)
        {
            isDie = true;
            enemy.OnDie(BossDestroyType.Kill);
        }
    }
    

    private IEnumerator HitAlphaAnimation()
    {
        // 맞는 표현을 위해서 색상을 color변수에 저장
        Color color = spriteRenderer.color;

        // 투명도를 우선 40%로 생성
        color.a = 0.4f;
        spriteRenderer.color = color;

        // 0.05초동안 대기
        yield return new WaitForSeconds(0.05f);

        // 투명도를 0으로 설정
        color.a = 1.0f;
        spriteRenderer.color = color;    
    }
}
