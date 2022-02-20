using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Synergy : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI synergyText;

    int[] synergy = new int[6];
    float[] synergyBuff = new float[5];

    public void chkSynergy(string TowerSynergy){
        
        string bufftext = "";
        
        switch(TowerSynergy){
            case "Assasin":
                synergy[0]++;
                if(synergy[0] == 2){
                    synergyBuff[1] += 30;
                }else if(synergy[0] == 4){
                    synergyBuff[0] += 100;
                }
                break;
            case "Warrior":
                synergy[1]++;
                break;
        }

        for(int i =0;i<6;i++){
            if(synergy[i]>0){
                switch(i){
                    case 0 :
                        bufftext += "Assasin : "+synergy[i];
                        break;
                    case 1 :
                        bufftext += "Warrior : "+synergy[i];
                        break;
                }
            }
        }
        
        bufftext += "\n받고있는 효과 // \n공격력 : "+synergyBuff[0]+" 크리티컬 확률 : "+synergyBuff[1]+" 공격 속도 : "+synergyBuff[2]+" 공격 범위 : "+synergyBuff[3];
        synergyText.text = bufftext;

        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        for(int i = 0; i<towers.Length;i++){
            TowerWeapon weapon = towers[i].GetComponent<TowerWeapon>();
            weapon.synergyDamage = synergyBuff[0];
            weapon.synergyCritical = synergyBuff[1];
            weapon.synergyRate = synergyBuff[2];
            weapon.synergyRange = synergyBuff[3];
            weapon.synergySlow = synergyBuff[4];
        }

    }

    public void removeSynergy(string TowerSynergy){
        
        string bufftext = "";
        
        switch(TowerSynergy){
            case "Assasin":
                synergy[0]--;
                if(synergy[0] == 1){
                    synergyBuff[1] -= 30;
                }else if(synergy[0] == 3){
                    synergyBuff[0] -= 100;
                }
                break;
            case "Warrior":
                synergy[1]--;
                break;
        }

        for(int i =0;i<6;i++){
            if(synergy[i]>0){
                switch(i){
                    case 0 :
                        bufftext += "Assasin : "+synergy[i];
                        break;
                    case 1 :
                        bufftext += "Warrior : "+synergy[i];
                        break;
                }
            }
        }
        
        bufftext += "\n받고있는 효과 // \n공격력 : "+synergyBuff[0]+" 크리티컬 확률 : "+synergyBuff[1]+" 공격 속도 : "+synergyBuff[2]+" 공격 범위 : "+synergyBuff[3];
        synergyText.text = bufftext;

        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        for(int i = 0; i<towers.Length;i++){
            TowerWeapon weapon = towers[i].GetComponent<TowerWeapon>();
            weapon.synergyDamage = synergyBuff[0];
            weapon.synergyCritical = synergyBuff[1];
            weapon.synergyRate = synergyBuff[2];
            weapon.synergyRange = synergyBuff[3];
            weapon.synergySlow = synergyBuff[4];
        }

    }

}

