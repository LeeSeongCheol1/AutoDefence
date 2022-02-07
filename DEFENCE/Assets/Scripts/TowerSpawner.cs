﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSpawner : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate[] towerTemplate;

    /*
    [SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private int towerBuildGold = 50;
    */

    [SerializeField]
    private BossSpawner bossSpawner;
    [SerializeField]
    private EnemySpawner enemySpawner;
    [SerializeField]
    private PlayerGold playerGold;
    [SerializeField]
    private SystemTextViewer systemTextViewer;
    private bool isOnTowerButton = false;
    private GameObject followTowerClone = null;
    private int towerType;

    public void ReadyToSpawnTower(int type)
    {
        towerType = type;
        // 버튼 중복 방지
        if(isOnTowerButton == true)
        {
            return;
        }

        // 골드가 없으면 건설 x
        if(towerTemplate[towerType].weapon[0].cost > playerGold.CurrentGold)
        {
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }

        // 타워 이미 클릭했다고 판정, 두번 클릭시 중복x를 위해 클릭했다고 판정
        isOnTowerButton = true;

        // 마우스
        followTowerClone = Instantiate(towerTemplate[towerType].followTowerPrefab);

        StartCoroutine("OnTowerCancelSystem");
    }

    public void spawnExtraTower(Transform tileTransform){
        if(isOnTowerButton == false){
            return;
        }

        Tile tile = tileTransform.GetComponent<Tile>();

        // 현재 타일 위치에 타워가 건설되어있으면 건설 x
        if(tile.IsBuildTower == true)
        {
            systemTextViewer.PrintText(SystemType.Build);
            return;
        }

        isOnTowerButton = false;
        // 타워가 지워져있음으로 설정
        tile.IsBuildTower = true;
        // 타워 건설에 필요한 골드만큼 감소
        // playerGold.CurrentGold -= towerBuildGold;
        playerGold.CurrentGold -= towerTemplate[towerType].weapon[0].cost;
        // 선택한 타일 위치에 타워 건설
        Vector3 position = tileTransform.position + Vector3.back;
        // GameObject clone = Instantiate(towerPrefab, position, Quaternion.identity);
        GameObject clone = Instantiate(towerTemplate[towerType].towerPrefab, position, Quaternion.identity);
        // 여분의 자리에만 두는것이기떄문에 공격은 x인상태로 설정
        clone.GetComponent<TowerWeapon>().disable = true;
        clone.GetComponent<TowerWeapon>().Setup(this,bossSpawner, enemySpawner,playerGold, tile);
        // 새로 배치되는 타워가 버프 타워 주변에 배치될 경우
        // 버프 효과를 받을 수 있도록 모든 버프 타워의 버프 효과 갱신
        OnBuffAllBuffTowers();
        // 배치 다 하고나면 임시 타워 삭제
        Destroy(followTowerClone);

        StopCoroutine("OnTowerCancelSystem");
    }

    public void SpawnTower(Transform tileTransform)
    {
        // 타워를 클릭한 상태가 아니라면 타워 건설x
        if(isOnTowerButton == false)
        {
            return;
        }

        /*
        // 타워를 건설할 돈이 없으면 건설 x
        // if(towerBuildGold > playerGold.CurrentGold)
        if(towerTemplate.weapon[0].cost > playerGold.CurrentGold)
        {
            systemTextViewer.PrintText(SystemType.Money);
            return;
        }
        */

        Tile tile = tileTransform.GetComponent<Tile>();

        // 현재 타일 위치에 타워가 건설되어있으면 건설 x
        if(tile.IsBuildTower == true)
        {
            systemTextViewer.PrintText(SystemType.Build);
            return;
        }

        isOnTowerButton = false;
        // 타워가 지워져있음으로 설정
        tile.IsBuildTower = true;
        // 타워 건설에 필요한 골드만큼 감소
        // playerGold.CurrentGold -= towerBuildGold;
        playerGold.CurrentGold -= towerTemplate[towerType].weapon[0].cost;
        // 선택한 타일 위치에 타워 건설
        Vector3 position = tileTransform.position + Vector3.back;
        // GameObject clone = Instantiate(towerPrefab, position, Quaternion.identity);
        GameObject clone = Instantiate(towerTemplate[towerType].towerPrefab, position, Quaternion.identity);
        // 타워 무기에 enemySpanwer 정보 전달
        clone.GetComponent<TowerWeapon>().Setup(this,bossSpawner, enemySpawner,playerGold, tile);

        // 새로 배치되는 타워가 버프 타워 주변에 배치될 경우
        // 버프 효과를 받을 수 있도록 모든 버프 타워의 버프 효과 갱신
        OnBuffAllBuffTowers();

        // 배치 다 하고나면 임시 타워 삭제
        Destroy(followTowerClone);

        StopCoroutine("OnTowerCancelSystem");
    }

    private IEnumerator OnTowerCancelSystem()
    {
        while (true)
        {
            // esc키나 오른쪽 마우스 클릭 시 건설 취소
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                isOnTowerButton = false;
                Destroy(followTowerClone);
                break;
            }
            yield return null;
        }
    }

    public void OnBuffAllBuffTowers()
    {
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        Debug.Log(towers);

        for(int i = 0; i<towers.Length; ++i)
        {
            TowerWeapon weapon = towers[i].GetComponent<TowerWeapon>();

            if(weapon.WeaponType == WeaponType.Buff)
            {
                weapon.OnBuffAroundTower();
            }
        }
    }
}
