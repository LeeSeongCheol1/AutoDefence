using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossDestroyType { Kill = 0,Arrive}

public class Boss : MonoBehaviour
{
    [SerializeField]
    private GameObject[] item;
    private int wayPointCount;
    private Transform[] wayPoints;
    private int currentIndex;
    private Movement2D movement2D;
    private BossSpawner enemySpawner;
    private int itemtype;

    public void Setup(BossSpawner enemySpawner,Transform[] wayPoints)
    {
        movement2D = GetComponent<Movement2D>();
        this.enemySpawner = enemySpawner;

        wayPointCount = wayPoints.Length;
        this.wayPoints = new Transform[wayPointCount];
        this.wayPoints = wayPoints;

        transform.position = wayPoints[currentIndex].position;

        StartCoroutine("OnMove"); //온루틴 코루틴함수 실행

    }

    private IEnumerator OnMove()
    {
        NextMoveTo();

        while (true)
        {
            transform.Rotate(Vector3.forward * 10);

            if (Vector3.Distance(transform.position, wayPoints[currentIndex].position) < 0.02f * movement2D.MoveSpeed)
            {
                NextMoveTo();
            }

            yield return null;
        }
    }

    private void NextMoveTo()
    {
            if(currentIndex == 4)
            {
                currentIndex = 0;
            }
            transform.position = wayPoints[currentIndex].position;
            currentIndex++;
            Vector3 direction = (wayPoints[currentIndex].position - transform.position).normalized;
            movement2D.MoveTo(direction);
    }

    public void Stunned(int time){
        StartCoroutine("Stun",time);
    }

    private IEnumerator Stun(int time){
        movement2D.MoveSpeed = 0;
        yield return new WaitForSeconds(time);
        movement2D.ResetMoveSpeed();
    }

    public void OnDie(BossDestroyType type)
    {
        enemySpawner.DestroyEnemy(type,this);
        itemtype = Random.Range(0, 3);
        Instantiate(item[itemtype], transform.position, Quaternion.identity);
    }
}