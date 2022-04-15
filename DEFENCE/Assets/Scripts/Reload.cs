using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Reload : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate[] towerTemplate_tier1;
    [SerializeField]
    private TowerTemplate[] towerTemplate_tier2;
    [SerializeField]
    private TowerTemplate[] towerTemplate_tier3;
    [SerializeField]
    private PlayerLV playerLV;
    [SerializeField]
    private TowerSpawner towerSpawner;
    [SerializeField]
    private Image[] btns;
    [SerializeField]
    private TextMeshProUGUI[] texts;
    [SerializeField]
    private Sprite[] sprites_tier1;
    [SerializeField]
    private Sprite[] sprites_tier2;
    [SerializeField]
    private Sprite[] sprites_tier3;
    [SerializeField]
    private SystemTextViewer systemTextViewer;
    [SerializeField]
    private TextMeshProUGUI[] synergyTexts;
    [SerializeField]
    public Button[] buttons;

    GameObject PlayerGold;
    int ranum1 = 0;

    int tier1 = 0;
    int tier2 = 0;
    int tier3 = 0;
    int tier4 = 0;

    int[] tier = new int[8];
    int[] type = new int[8];
    public int clickednum = -1;

    private void Awake()
    {
        PlayerGold = GameObject.Find("PlayerStats");
        reloadUpdate();
        Reloading();
    }

    public void reloadUpdate()
    {
        switch (playerLV.CurrentLV)
        {
            case 1:
                tier1 = 100;
                break;
            case 2:
                tier1 = 100;
                break;
            case 3:
                tier1 = 75;
                tier2 = 25;
                break;
            case 4:
                tier1 = 55;
                tier2 = 30;
                tier3 = 15;
                break;
            case 5:
                tier1 = 45;
                tier2 = 33;
                tier3 = 22;
                tier4 = 0;
                break;
            case 6:
                tier1 = 25;
                tier2 = 40;
                tier3 = 35;
                tier4 = 0;
                break;
            case 7:
                tier1 = 20;
                tier2 = 30;
                tier3 = 50;
                tier4 = 0;
                break;
            case 8:
                tier1 = 15;
                tier2 = 20;
                tier3 = 65;
                tier4 = 0;
                break;
        }
    }

    public void reloadPurchase(){
        bool isSuccess = PlayerGold.GetComponent<PlayerGold>().purchaseOX(2);

        if(isSuccess == true){
            Reloading();
        }else
        {
            systemTextViewer.PrintText(SystemType.Money);
        }
            
    }
    public void Reloading()
    {
        for (int i = 0; i < 6; i++)
        {
            buttons[i].interactable = true;
            reloadUpdate();
            type[i] = Random.Range(0, 8);
            ranum1 = Random.Range(1, 101);
            if (ranum1 <= tier1)
            {
                tier[i] = 1;
            }else if(ranum1 <= (tier1 + tier2))
            {
                tier[i] = 2;
            }else
            {
                tier[i] = 3;
            }


            switch(tier[i]){
                case 1:
                synergyTexts[i].text = towerTemplate_tier1[type[i]].weapon[0].towerSynergy;
                texts[i].text = "$" + towerTemplate_tier1[type[i]].weapon[0].cost;
                btns[i].sprite = sprites_tier1[type[i]];    
                break;
                case 2:
                synergyTexts[i].text = towerTemplate_tier2[type[i]].weapon[0].towerSynergy;
                texts[i].text = "$" + towerTemplate_tier2[type[i]].weapon[0].cost;
                btns[i].sprite = sprites_tier2[type[i]];    
                break;
                case 3:
                synergyTexts[i].text = towerTemplate_tier3[type[i]].weapon[0].towerSynergy;
                texts[i].text = "$" + towerTemplate_tier3[type[i]].weapon[0].cost;
                btns[i].sprite = sprites_tier3[type[i]];    
                break;
            }
        }
    }

    public void btn1Clicked()
    {
        towerSpawner.ReadyToSpawnTower(tier[0], type[0]);
        buttons[0].interactable = false;
        clickednum = 0;
    }

    public void btn2Clicked()
    {
        
        towerSpawner.ReadyToSpawnTower(tier[1], type[1]);
        buttons[1].interactable = false;
        clickednum = 1;
    }

    public void btn3Clicked()
    {
        towerSpawner.ReadyToSpawnTower(tier[2], type[2]);
        buttons[2].interactable = false;
        clickednum = 2;
    }

    public void btn4Clicked()
    {
        towerSpawner.ReadyToSpawnTower(tier[3], type[3]);
        buttons[3].interactable = false;
        clickednum = 3;
    }

    public void btn5Clicked()
    {
        towerSpawner.ReadyToSpawnTower(tier[4], type[4]);
        buttons[4].interactable = false;
        clickednum = 4;
    }

    public void btn6Clicked()
    {
        towerSpawner.ReadyToSpawnTower(tier[5], type[5]);
        buttons[5].interactable = false;
        clickednum = 5;
    }
}
