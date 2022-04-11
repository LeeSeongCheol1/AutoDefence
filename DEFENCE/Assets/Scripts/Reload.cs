using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Reload : MonoBehaviour
{
    [SerializeField]
    private TowerTemplate[] towerTemplate;
    [SerializeField]
    private PlayerLV playerLV;
    [SerializeField]
    private TowerSpawner towerSpawner;
    [SerializeField]
    private Image[] btns;
    [SerializeField]
    private TextMeshProUGUI[] texts;
    [SerializeField]
    private Sprite[] sprites;
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

    int[] tier = new int[6];
    int[] type = new int[6];
    public int clickednum = 0;

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
                tier3 = 20;
                tier4 = 2;
                break;
            case 6:
                tier1 = 25;
                tier2 = 40;
                tier3 = 30;
                tier4 = 5;
                break;
            case 7:
                tier1 = 20;
                tier2 = 30;
                tier3 = 35;
                tier4 = 15;
                break;
            case 8:
                tier1 = 15;
                tier2 = 20;
                tier3 = 40;
                tier4 = 25;
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
            type[i] = Random.Range(0, 4);
            ranum1 = Random.Range(1, 101);
            if (ranum1 <= tier1)
            {
                tier[i] = 1;
            }else if(ranum1 <= (tier1 + tier2))
            {
                tier[i] = 2;
            }else if (ranum1 <= (tier1 + tier2 + tier3))
            {
                tier[i] = 3;
            }
            else
            {
                tier[i] = 4;
            }
            synergyTexts[i].text = towerTemplate[type[i]].weapon[0].towerSynergy;
            texts[i].text = "$" + towerTemplate[type[i]].weapon[0].cost;
            btns[i].sprite = sprites[type[i]];    
        }
    }

    public void btn1Clicked()
    {
        towerSpawner.ReadyToSpawnTower(type[0]);
        buttons[0].interactable = false;
        clickednum = 0;
        
    }

    public void btn2Clicked()
    {
        towerSpawner.ReadyToSpawnTower(type[1]);
        buttons[1].interactable = false;
        clickednum = 1;
    }

    public void btn3Clicked()
    {
        towerSpawner.ReadyToSpawnTower(type[2]);
        buttons[2].interactable = false;
        clickednum = 2;
    }

    public void btn4Clicked()
    {
        towerSpawner.ReadyToSpawnTower(type[3]);
        buttons[3].interactable = false;
        clickednum = 3;
    }

    public void btn5Clicked()
    {
        towerSpawner.ReadyToSpawnTower(type[4]);
        buttons[4].interactable = false;
        clickednum = 4;
    }

    public void btn6Clicked()
    {
        towerSpawner.ReadyToSpawnTower(type[5]);
        buttons[5].interactable = false;
        clickednum = 5;
    }
}
