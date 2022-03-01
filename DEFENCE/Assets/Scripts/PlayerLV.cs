using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerLV : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI currentLevel;
    [SerializeField]
    private TextMeshProUGUI currentExperience;
    [SerializeField]
    private SystemTextViewer systemTextViewer;
    [SerializeField]
    private Button buttonUpgrade;
    [SerializeField]
    private TowerSpawner towerSpawner;

    GameObject Reload;
    GameObject PlayerGold;
    bool first = true;

    int[] level = new int[] { 0, 2, 6, 10, 20, 36, 56, 86 };
    private int currentLV = 1;
    private int currentExp = 0;

    public int CurrentLV
    {
        set => currentLV = Mathf.Max(0, value);
        get => currentLV;
    }

    public int CurrentEXP
    {
        set => currentExp = Mathf.Max(0, value);
        get => currentExp;
    }

    private void Awake()
    {
        currentLevel.text = "플레이어 레벨 : " + 1;
        currentExperience.text = "Exp : "+currentExp + " / " + level[currentLV];
        PlayerGold = GameObject.Find("PlayerStats");
        Reload = GameObject.Find("RELOAD");
    }

    public void levelUp(int exp)
    {

        if(currentLV == 8)
        {
            currentLevel.text = "플레이어 레벨 : MAX";
            currentExperience.text = "Exp : MAX";
            towerSpawner.UpdateTowerText();
            return;
        }

        currentExp += exp;

        if(currentExp >= level[currentLV])
        {
            if(first == true){
                first = false;
                currentExp = 0;
                return;
            }
            towerSpawner.maxnum += 1;
            Reload.GetComponent<Reload>().reloadUpdate();
            int overExp = 0;
            overExp = currentExp - level[currentLV];
            currentExp = overExp;
            currentLV++;
        }

        if (currentLV == 8)
        {
            buttonUpgrade.interactable = false;
            currentLevel.text = "플레이어 레벨 : MAX";
            currentExperience.text = "Exp : MAX";
            towerSpawner.UpdateTowerText();
            return;
        }

        currentLevel.text = "플레이어 레벨 : " + currentLV;
        currentExperience.text = "Exp : " + currentExp + " / " + level[currentLV];
        towerSpawner.UpdateTowerText();

    }

    public void expPurchase()
    {
        bool isSuccess = PlayerGold.GetComponent<PlayerGold>().purchaseOX(4);

        if(isSuccess == true)
        {
            levelUp(4);
        }
        else
        {
            systemTextViewer.PrintText(SystemType.Money);
        }
    }

}
