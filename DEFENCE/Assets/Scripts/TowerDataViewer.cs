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
    private TextMeshProUGUI bossButtonText;
    [SerializeField]
    private Button moveButton;
    [SerializeField]
    private SystemTextViewer systemTextViewer;
    [SerializeField]
    private BossSpawner bossSpawner;
    [SerializeField]
    private Button cancelButton;

    private TowerWeapon currentTower;

    private void Awake()
    {
        OffPanel();
    }

    public void OnPanel(Transform towerWeapon)
    {
        // 타워 정보 저장
        currentTower = towerWeapon.GetComponent<TowerWeapon>();
        // 타워 정보 갱신
        UpdateTowerData();
        // 타워 정보 패널 On
        gameObject.SetActive(true);
        // 타워 오브젝트 주변에 공격 범위 On
        towerAttackRange.OnAttackRange(currentTower.transform.position, currentTower.Range);
        cancelButton.gameObject.SetActive(true);
    }

    public void OffPanel()
    {
        gameObject.SetActive(false);
        towerAttackRange.OffAttackRange();
    }

    private void UpdateTowerData()
    {
        if(currentTower.WeaponType == WeaponType.Cannon || currentTower.WeaponType == WeaponType.Laser)
        {
            float add = currentTower.AddedDamage+currentTower.synergyDamage;
            imageTower.rectTransform.sizeDelta = new Vector2(88, 59);
            textDamege.text = currentTower.minDamage+" ~ "+currentTower.maxDamage+" + "+"<color=red>"+add.ToString("F1")+"</color>";
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
        textSellCost.text = currentTower.SellCost.ToString();

        // 업그레이드가 불가능해지면 비활성화
        buttonUpgrade.interactable = currentTower.Level < currentTower.MaxLevel ? true : false;

        if(currentTower.disable == true){
            bossButton.interactable = false;
        }
        
        if(bossSpawner.bossAtk == true){
            if(currentTower.bossmode == true){
                bossButtonText.text = "원래 자리로";
                bossButton.interactable = true;
                moveButton.interactable = false;
            }else{
                bossButtonText.text = "보스 방으로 이동";
                bossButton.interactable = false;
                moveButton.interactable = true;
            }
        }else{
            bossButtonText.text = "보스 방으로 이동";
            bossButton.interactable = true;
            moveButton.interactable = true;
        }
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

    public void OnClickMoveTower(){
        currentTower.MoveTower();
        OffPanel();
    }
}
