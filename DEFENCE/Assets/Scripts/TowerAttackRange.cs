using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttackRange : MonoBehaviour
{   /*
    private void Awake()
    {
        OffAttackRange();
    }
    */

    public void OnAttackRange(Vector3 position, float range)
    {
        gameObject.SetActive(true);

        // 공격 범위 크기 ( 지름이기때문에 * 2 )
        float diameter = range * 2.0f;
        transform.localScale = Vector3.one * diameter;
        transform.position = position;
    }

    public void OffAttackRange()
    {
        gameObject.SetActive(false);
    }
}
