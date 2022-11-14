using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WeaponType { Cannon = 0, Laser, Slow, Buff,}
public enum WeaponState { SearchTarget = 0, TryAttackCannon, TryAttackLaser,}

public class TowerWeapon : MonoBehaviour
{
    [SerializeField]
    public bool disable;
    [SerializeField]
    public bool bossmode;

    // Inspector View의 표시되는 변수들을 용도별로 구분하기 위한 어트리뷰트
    [Header("Commons")]
    [SerializeField]
    private TowerTemplate towerTemplate;
    [SerializeField]
    private Transform spawnPoint;
    [SerializeField]
    private WeaponType weaponType;

    [Header("Cannon")]
    [SerializeField]
    private GameObject projectilePrefab;

    [Header("Laser")]
    [SerializeField]
    private LineRenderer lineRenderer;  // 레이저로 사용되는 선
    [SerializeField]
    private Transform hitEffect;    // 타격 효과
    [SerializeField]
    private LayerMask targetLayer;  // 광선에 부딫히는 레이어

    private int level = 0;
    private WeaponState weaponState = WeaponState.SearchTarget;
    private Transform attackTarget = null;
    private SpriteRenderer spriteRenderer;
    private TowerSpawner towerSpawner;
    private EnemySpawner enemySpawner;
    private BossSpawner bossSpawner;
    private PlayerGold playerGold;
    public Tile ownerTile;
    private SystemTextViewer systemTextViewer;
    private ObjectDetector objectDetector;
    private float addedDamage;
    private int buffLevel;  // 버프를 받는지 여부 설정
    int[] y = new int[2] {1,2};    // 겹치는 타워 배열 번호 저장하는 배열(업그레이드 시 3개있는지 확인)
    int z = 0;  // 겹치는 타워 갯수를 저장

    public Sprite TowerSprite => towerTemplate.weapon[level].sprite;
    public float minDamage => towerTemplate.weapon[level].minDamage;
    public float maxDamage => towerTemplate.weapon[level].maxDamage;
    public float Rate => towerTemplate.weapon[level].rate;
    public float Range => towerTemplate.weapon[level].range;
    public int UpgradeCost => Level < MaxLevel ? towerTemplate.weapon[level + 1].cost : 0;
    public int SellCost => towerTemplate.weapon[level].sell;
    public int MaxLevel => towerTemplate.weapon.Length;
    public float Slow => towerTemplate.weapon[level].slow;
    public float Buff => towerTemplate.weapon[level].buff;
    public WeaponType WeaponType => weaponType;
    public float Critical => towerTemplate.weapon[level].critical;

    public float synergyDamage;
    public float synergyCritical;
    public float synergyRate;
    public float synergyRange;
    public float synergySlow;
    private Synergy syn;
    
    private string towerIdentity => towerTemplate.weapon[level].speciesIdentity; 
    public string towersynergy => towerTemplate.weapon[level].speciesIdentity; 
    public GameObject[] towerarr;
    private Vector3 vec;
    public Vector3 vec1;
    public Vector3 vec2;

    public int[] items = new int[3]; 
    public int itemnum = 0;
    public string towertype;

    /*
    public float Damage => attackDamage;
    public float Rate => attackRate;
    public float Range => attackRange;
    */

    private void Awake(){
        if(gameObject.tag == "Enemy"){
            return;
        }
        objectDetector = GameObject.Find("ObjectDetector").GetComponent<ObjectDetector>();
        systemTextViewer = GameObject.Find("TextSystem").GetComponent<SystemTextViewer>();
    }

    public int Level => level + 1;
    public float AddedDamage
    {
        set => addedDamage = Mathf.Max(0, value);
        get => addedDamage;
    }
    public int BuffLevel
    {
        set => buffLevel = Mathf.Max(0, value);
        get => buffLevel;
    }

    public void Setup(TowerSpawner towerSpawner,BossSpawner bossSpawner,EnemySpawner enemySpawner,PlayerGold playerGold,Tile ownerTile)
    {
        spriteRenderer = GetComponent<SpriteRenderer>();    // 타워 외형 가져오기
        this.towerSpawner = towerSpawner;
        this.bossSpawner = bossSpawner;
        this.enemySpawner = enemySpawner;
        this.playerGold = playerGold;
        this.ownerTile = ownerTile;

        if (weaponType == WeaponType.Cannon || weaponType == WeaponType.Laser)
        {
            ChangeState(WeaponState.SearchTarget);
        }
    }

    public void ChangeState(WeaponState newState)
    {
        StopCoroutine(weaponState.ToString());
        weaponState = newState;
        StartCoroutine(weaponState.ToString());
    }

    private void Update()
    {
            
        if(attackTarget != null)
        {
            RotateToTarget();
        }
    }

    private void RotateToTarget()
    {
        float rotateSpeed = 5.0f;
        float dx = attackTarget.position.x - transform.position.x;
        float dy = attackTarget.position.y - transform.position.y;

        float degree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0.0f, 0.0f, degree), rotateSpeed * Time.deltaTime);
    }

    private IEnumerator SearchTarget()
    {
        while (true)
        {
            if(disable == true){
            break;
        }

            /*
            float closestDistSqr = Mathf.Infinity;
            for(int i = 0; i<enemySpawner.EnemyList.Count; ++i)
            {
                float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);

                //if(distance <= attackRange && distance <= closestDistSqr)
                if(distance <= towerTemplate.weapon[level].range && distance <= closestDistSqr)
                {
                    closestDistSqr = distance;
                    attackTarget = enemySpawner.EnemyList[i].transform;
                }
            }
            */
        attackTarget = FindClosestAttackTarget();

            if(attackTarget != null)
            {
                if (weaponType == WeaponType.Cannon)
                {
                    ChangeState(WeaponState.TryAttackCannon);
                }else if(weaponType == WeaponType.Laser)
                {
                    ChangeState(WeaponState.TryAttackLaser);
                }
            }
            yield return null;
        }
    }

    public void OnBuffAroundTower()
    {
        // 현재 맵에 배치된 "Tower"태그를 가진 모든 오브젝트 탐색
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");

        for(int i = 0; i<towers.Length; ++i)
        {
            TowerWeapon weapon = towers[i].GetComponent<TowerWeapon>();

            // 이미 버프를 받고 있고, 현재의 레벨보다 높은 버프면 패스
            if(weapon.BuffLevel > Level)
            {
                continue;
            }

            // 현재 버프타워와 다른 타워의 거리를 검사해서 범위 안에 타워가 있으면
            if (Vector3.Distance(weapon.transform.position,transform.position) <= towerTemplate.weapon[level].range)
            {
                // 공격이 가능한 캐논, 레이저 타워라면
                if(weapon.WeaponType == WeaponType.Cannon || weapon.WeaponType == WeaponType.Laser)
                {
                    // 버프에 의해 공격력 증가
                    weapon.AddedDamage = weapon.maxDamage * (towerTemplate.weapon[level].buff);
                    // 타워가 받고 있는 버프 레벨 설정
                    weapon.BuffLevel = Level;
                }
            }
        }
    }

    private IEnumerator TryAttackCannon()
    {
        while (true)
        {

        if(IsPossibleToAttackTarget() == false)
        {
            ChangeState(WeaponState.SearchTarget);
            break;
        }

            // attackRate 시간만큼 대기
            // yield return new WaitForSeconds(attackRate);
            yield return new WaitForSeconds(10/(towerTemplate.weapon[level].rate));

            // 발사체 생성
            SpawnProjectile();
        }
    }

    private IEnumerator TryAttackLaser()
    {
        // 타격 효과 활성화
        EnableLaser();

        while (true)
        {
            // target이 공격하는게 가능한지 검사
            if(IsPossibleToAttackTarget() == false)
            {
                // 레이저 비활성화
                DisableLaser();
                ChangeState(WeaponState.SearchTarget);
                break;
            }
            // 레이저 공격 
            SpawnLaser();

            yield return null;
        }
    }

    private Transform FindClosestAttackTarget()
    {
        // 제일 가까이 있는 적을 찾기위해 최초 거리를 최대한 크게 설정
        float closestDistSqr = Mathf.Infinity;

        if(bossmode == false)
        {
            // EnemySpawner의 EnemyList에있는 모든 적 검사
            for(int i = 0; i<enemySpawner.EnemyList.Count; ++i)
            {
                float distance = Vector3.Distance(enemySpawner.EnemyList[i].transform.position, transform.position);
                // 현재 검사중인 적과의 거리가 공격 범위 내에 있고, 현재까지 검사한 적보다 거리가 가깝다면
                if (distance <= towerTemplate.weapon[level].range+synergyRange&& distance <= closestDistSqr)
                {
                closestDistSqr = distance;
                attackTarget = enemySpawner.EnemyList[i].transform;
                }
            }
        }else{
            for(int i = 0; i<bossSpawner.EnemyList.Count; ++i)
            {
                float distance = Vector3.Distance(bossSpawner.EnemyList[i].transform.position, transform.position);
                // 현재 검사중인 적과의 거리가 공격 범위 내에 있고, 현재까지 검사한 적보다 거리가 가깝다면
                if (distance <= towerTemplate.weapon[level].range+synergyRange && distance <= closestDistSqr)
                {
                    closestDistSqr = distance;
                    attackTarget = bossSpawner.EnemyList[i].transform;
                }
            }
        }

        return attackTarget;
    }

    private bool IsPossibleToAttackTarget()
    {
        // target이 있는지 검사
        if(attackTarget == null)
        {
            return false;
        }

        // target이 공격 범위 안에 있는지 검사(공격 범위를 넘어가면 새로운 적 탐색)
        float distance = Vector3.Distance(attackTarget.position, transform.position);
        if(distance > towerTemplate.weapon[level].range+synergyRange)
        {
            attackTarget = null;
            return false;
        }
        return true;
    }

    private void SpawnProjectile()
    {
        GameObject clone = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        // clone.GetComponent<Projectile>().Setup(attackTarget,towerTemplate.weapon[level].damage);
        int damage = (int)(Random.Range(minDamage,maxDamage+1)+synergyDamage);
        bool cri = randomOX(Critical);

        if (cri)
        {
            damage = (int)(maxDamage + AddedDamage) * 2;
        }
        else {
            damage = damage + (int)AddedDamage;
        }
        
        switch(towertype){
            case "Hunter":
                clone.GetComponent<hunterProjectile>().Setup(attackTarget, damage,cri);
                break;
            case "Warrior":
                clone.GetComponent<warriorProjectile>().Setup(attackTarget, damage,cri);
                break;
            default:
                clone.GetComponent<Projectile>().Setup(attackTarget, damage,cri);
                break;
        }
    }

    private bool randomOX(float criticalRate)
    {
        int percent = Random.Range(0, 101);
        int Rate = (int)(criticalRate+synergyCritical);
        if(percent <= Rate)
        {   
            return true;
        }
        else
        {
            return false;
        }
    }

    private void EnableLaser()
    {
        lineRenderer.gameObject.SetActive(true);
        hitEffect.gameObject.SetActive(true);
    }

    private void DisableLaser()
    {
        lineRenderer.gameObject.SetActive(false);
        hitEffect.gameObject.SetActive(false);
    }

    private void SpawnLaser()
    {
        Vector3 direction = attackTarget.position - spawnPoint.position;
        RaycastHit2D[] hit = Physics2D.RaycastAll(spawnPoint.position, direction, towerTemplate.weapon[level].range, targetLayer);

        for(int i =0;i<hit.Length; ++i)
        {
            if(hit[i].transform == attackTarget)
            {
                // 선의 시작지점
                lineRenderer.SetPosition(0, spawnPoint.position);
                // 선의 목표지점
                lineRenderer.SetPosition(1, new Vector3(hit[i].point.x, hit[i].point.y, 0) + Vector3.back);
                // 타격 효과 위치 설정
                hitEffect.position = hit[i].point;
                // 적 체력 감소(1초에 damage만큼 감소)
                // attackTarget.GetComponent<EnemyHP>().TakeDamage(towerTemplate.weapon[level].damage * Time.deltaTime);

                // 공격력 = 타워 기본 공격력 + 버프에 의해 추가된 공격력
                float damage = towerTemplate.weapon[level].minDamage + AddedDamage +synergyDamage;
                if(bossmode == true){
                    attackTarget.GetComponent<BossHP>().TakeDamage(damage * Time.deltaTime);
                }else{
                    attackTarget.GetComponent<EnemyHP>().TakeDamage(damage * Time.deltaTime);
                }
            }
        }
    }

    public bool Upgrade()
    {
        gameObject.tag = "chkedTower";
        bool chk = upgradeChk();
        gameObject.tag = "Tower";

        if(chk == false){
            return false;
        }

        for(int i = 0; i<2;i++){
            if(towerarr[y[i]].GetComponent<TowerWeapon>().disable == false){
                towerSpawner.towernum--;
                GameObject synergy = GameObject.FindGameObjectWithTag("Synergy");
                synergy.GetComponent<Synergy>().removeSynergy(towerTemplate.weapon[level].speciesIdentity);
            }
            towerarr[y[i]].GetComponent<TowerWeapon>().ownerTile.IsBuildTower = false;
            Destroy(towerarr[y[i]]);
            towerSpawner.UpdateTowerText();
        }
        level++;
        // 타워 외형 변경(Sprite)
        spriteRenderer.sprite = towerTemplate.weapon[level].sprite;
        // 무기 속정이 레이저라면
        if(weaponType == WeaponType.Laser)
        {
            // 레이저는 업그레이드에 따라 굵기를 증가
            lineRenderer.startWidth = 0.05f + level * 0.05f;
            lineRenderer.endWidth = 0.05f;
        }

        // 타워가 업그레이드 될 때 모든 버프 타워의 버프 효과 갱신
        // 현재 타워가 버프 타워윈 경우, 현재 타워가 공격 타워인 경우
        towerSpawner.OnBuffAllBuffTowers();
    
        return true; 
    }

    public bool upgradeChk(){
        z = 0; 

        towerarr = GameObject.FindGameObjectsWithTag("Tower");
        for(int i = 0; i<towerarr.Length;i++){
            TowerWeapon towerWeapon = towerarr[i].GetComponent<TowerWeapon>();
            if(towerWeapon.towerIdentity == towerIdentity){
                y[z] = i;
                z++;
                if(z == 2){
                    return true;
                }
            }
        }

        return false;
    }

    public void Sell()
    {
        if(disable == false){
            towerSpawner.towernum--;
            towerSpawner.UpdateTowerText();
            GameObject synergy = GameObject.FindGameObjectWithTag("Synergy");
            synergy.GetComponent<Synergy>().removeSynergy(towerTemplate.weapon[level].speciesIdentity);
        }
        // 골드 증가
        playerGold.CurrentGold += towerTemplate.weapon[level].sell;
        // 현재 타일에 다시 건설 가능하게 설정
        ownerTile.IsBuildTower = false;
        // 타워 파괴
        Destroy(gameObject);
    }

    public void MoveBossScene(){

        if(bossmode == false){
            vec = this.gameObject.transform.position;
            GameObject bossTile = GameObject.FindGameObjectWithTag("BossTile");
            bossSpawner.bossAtk = true;
            bossmode = true;
            this.gameObject.transform.position = bossTile.transform.position;
        }else if(bossmode == true){
            bossSpawner.bossAtk = false;
            bossmode = false;
            this.gameObject.transform.position = vec;
        }
    }

    public void MoveTower(){
        gameObject.tag = "moveTower1";
        objectDetector.moveStatus = true;
        systemTextViewer.MovePrint(true);
        StartCoroutine("MoveTowerCancel");
    }

    private IEnumerator MoveTowerCancel()
    {
        while (true)
        {
            // esc키나 오른쪽 마우스 클릭 시 건설 취소
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
            {
                gameObject.tag = "Tower";
                objectDetector.moveStatus = false;
                systemTextViewer.MovePrint(false);
                break;
            }
            yield return null;
        }
    }

    public void EndMoveStatus(){
        gameObject.tag = "Tower";
        objectDetector.moveStatus = false;
        systemTextViewer.MovePrint(false);
        StopCoroutine("MoveTowerCancel");
    }

    public void equipItems(int type){
        switch(type){
            case 0: 
                items[itemnum] = type;
                synergyDamage += 50;
                itemnum++;
                break;
            case 1:
                items[itemnum] = type;
                synergyRate -= 0.15f;
                itemnum++;
                break;
            case 2:
                items[itemnum] = type;
                synergyRange += 1;
                itemnum++;
                break;
        }
        systemTextViewer.PrintText(SystemType.zero);
    }
}
