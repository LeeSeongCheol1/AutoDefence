using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TowerDataViewer : MonoBehaviour
{
    [SerializeField]
    private Image imageTower;
    [SerializeField]
    private TextMeshProUGUI textDamege;
    [SerializeField]
    private TextMeshProUGUI textRate;
    [SerializeField]
    private TextMeshProUGUI textRange;
    [SerializeField]
    private TextMeshProUGUI textLevel;
    [SerializeField]
    private TextMeshProUGUI textUpgradeCost;
    [SerializeField]
    private TextMeshProUGUI textSellCost;
    [SerializeField]
    private TowerAttackRange towerAttackRange;
    [SerializeField]
    private Button buttonUpgrade;
    [SerializeField]
    private Button bossButton;
    [SerializeField]
    private SystemTextViewer systemTextViewer;
    [SerializeField]
    private TextMeshProUGUI bossRoomText;
    [SerializeField]
    private BossSpawner bossSpawner;

    private TowerWeapon currentTower;

    private void Awake()
    {
        OffPanel();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OffPanel();
        }
    }

    public void OnPanel(Transform towerWeapon)
    {
        // 타워 정보 저장
        currentTower = towerWeapon.GetComponent<TowerWeapon>();
        // 타워 정보 패널 On
        gameObject.SetActive(true);
        // 타워 정보 갱신
        UpdateTowerData();
        // 타워 오브젝트 주변에 공격 범위 On
        towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);

    }

    public void OffPanel()
    {
        gameObject.SetActive(false);
        towerAttackRange.OffAttackRange();
    }

    private void UpdateTowerData()
    {

        if(currentTower.bossmode == true){
            bossRoomText.text = "원래 자리로 이동";
        }else{
            if(currentTower.disable == true){
                bossRoomText.text = "필드 타워만 보스 가능!";
            }else if(bossSpawner.bossAtk == true){
                bossRoomText.text = "다른 타워가 전투 중!";
            } 
        }

        // 타워가 보스방에있는데 클릭하면 보스방으로 이동하는 버튼을 비활성화
        if(bossSpawner.bossAtk == true){
            // 타워가 보스방에 있지않는 타워라면 
            if(currentTower.bossmode == false){
                bossButton.interactable = false;
            }else{  
                // 반대로 보스방에 있는 타워라면
                bossButton.interactable = true;
            }
        }


        if(currentTower.WeaponType == WeaponType.Cannon || currentTower.WeaponType == WeaponType.Laser)
        {
            imageTower.rectTransform.sizeDelta = new Vector2(88, 59);
            textDamege.text = currentTower.minDamage+" ~ "+currentTower.maxDamage+" + "+"<color=red>"+currentTower.AddedDamage.ToString("F1")+"</color>";
        }
        else
        {
            imageTower.rectTransform.sizeDelta = new Vector2(59, 59);

            if(currentTower.WeaponType == WeaponType.Slow)
            {
                textDamege.text = "Slow : " + currentTower.Slow * 100 + "%";
            }
            else if(currentTower.WeaponType == WeaponType.Buff)
            {
                textDamege.text = "Buff : " + currentTower.Buff * 100 + "%";
            }
        }

        imageTower.sprite = currentTower.TowerSprite;
        // textDamege.text = "Damage : " + currentTower.Damage;
        textRate.text = "Rate : " + currentTower.Rate;
        textRange.text = "Range : " + currentTower.Range;
        textLevel.text = "Level : " + currentTower.Level;
        textUpgradeCost.text = currentTower.UpgradeCost.ToString();
        textSellCost.text = currentTower.SellCost.ToString();

        // 업그레이드가 불가능해지면 비활성화
        buttonUpgrade.interactable = currentTower.Level < currentTower.MaxLevel ? true : false;
    }

    public void OnClickEventTowerUpgrade()
    {
        // 타워 업그레이드 시도 (성공 : true, 실패 : false)
        bool isSuccess = currentTower.Upgrade();

        if(isSuccess == true)
        {
            // 업그레이드되면  정보 갱신
            UpdateTowerData();
            // 공격범위도 갱신
            towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
        }
        else
        {
            systemTextViewer.PrintText(SystemType.Money);
        }
    }

    public void OnClickEventTowerSell()
    {
        currentTower.Sell();
        // 공격범위 Off
        OffPanel();
    }

    public void OnClickEventMoveBossRoom(){
        currentTower.MoveBossScene();
        OffPanel();
    }
}
