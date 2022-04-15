using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSystem : MonoBehaviour
{
    [SerializeField]
    private Wave[] waves;
    [SerializeField]
    private EnemySpawner enemySpawner;
    [SerializeField]
    private PlayerGold playerGold;
    [SerializeField]
    private GameOver gameOver;
    [SerializeField]
    private Reload reload;

    int addGold = 0;
    private int currentWaveIndex = -1;
    public int CurrentWave => currentWaveIndex + 1;
    public int MaxWave => waves.Length;

    public void StartWave()
    {
        if(currentWaveIndex < MaxWave - 1)
        {
            currentWaveIndex++;
            enemySpawner.StartWave(waves[currentWaveIndex]);
            reload.Reloading();
            if(CurrentWave <= 1){
                return;
            }
            // 이자
            addGold = (int)(playerGold.CurrentGold/10);
            // 이자가 5 이상이면 5로 설정
            if(addGold > 5)
            {
                addGold = 5;
            }

            if(enemySpawner.currentEnemyCount == 0){
                addGold += 3;
            }
            playerGold.CurrentGold += (5 + addGold);
        }

        if(currentWaveIndex == 10){
            GameObject boss = GameObject.FindGameObjectWithTag("Boss");
            if(boss != null){
                gameOver.gameOver();
            }
        }
    }
}

// 구조체, 클래스를 직렬화하는 명령
// 메모리 상에 존재하는 오브젝트 정보를 string / byte데이터로 변환
[System.Serializable]
public struct Wave
{
    public float spawnTime;
    public int maxEnemyCount;
    public GameObject[] enemyPrefabs;
}
