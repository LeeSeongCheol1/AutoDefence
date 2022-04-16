using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Synergy : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI synergyText;
    [SerializeField]
    private TextMeshProUGUI totalSynergy;

    int[] synergy = new int[8];
    float[] synergyBuff = new float[8];

    //버프배열  0 : 공격력/1 : 크리티컬/2 : 공속/3 : 범위/4 : 이감
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
            case "Archer":
                synergy[1]++;
                if(synergy[1] == 2){
                    synergyBuff[3] += 30;
                }else if(synergy[1] == 4){
                    synergyBuff[1] += 30;
                }
                break;
            case "Hunter":
                synergy[2]++;
                if(synergy[2] == 2){
                    synergyBuff[3] += 30;
                }else if(synergy[2] == 4){
                    synergyBuff[0] += 100;
                }
                break;
            case "Warrior":
                synergy[3]++;
                if(synergy[3] == 2){
                    synergyBuff[3] += 30;
                }else if(synergy[3] == 4){
                    synergyBuff[4] += 100;
                }
                break;
            case "Glacia":
                synergy[4]++;
                if(synergy[4] == 2){
                    synergyBuff[3] += 30;
                }else if(synergy[4] == 4){
                    synergyBuff[3] += 100;
                }
                break;
            case "Magician":
                synergy[5]++;
                if(synergy[5] == 2){
                    synergyBuff[3] += 30;
                }else if(synergy[5] == 4){
                    synergyBuff[3] += 100;
                }
                break;
            case "Pirate":
                synergy[6]++;
                if(synergy[6] == 2){
                    synergyBuff[3] += 30;
                }else if(synergy[6] == 4){
                    synergyBuff[3] += 100;
                }
                break;
            case "Priest":
                synergy[7]++;
                if(synergy[7] == 2){
                    synergyBuff[3] += 30;
                }else if(synergy[7] == 4){
                    synergyBuff[3] += 100;
                }
                break;
        }

        for(int i =0;i<8;i++){
            if(synergy[i]>0){
                switch(i){
                    case 0 :
                        bufftext += "Assasin : "+synergy[i]+"\n";
                        break;
                    case 1 :
                        bufftext += "Archer : "+synergy[i]+"\n";
                        break;
                    case 2 :
                        bufftext += "Hunter : "+synergy[i]+"\n";
                        break;
                    case 3 :
                        bufftext += "Warrior : "+synergy[i]+"\n";
                        break;
                    case 4 :
                        bufftext += "Glacia : "+synergy[i]+"\n";
                        break;
                    case 5 :
                        bufftext += "Magician : "+synergy[i]+"\n";
                        break;
                    case 6 :
                        bufftext += "Pirate : "+synergy[i]+"\n";
                        break;
                    case 7 :
                        bufftext += "Priest : "+synergy[i]+"\n";
                        break;
                }
            }
        }
        
        totalSynergy.text = "\n받고있는 효과 // \n공격력 : "+synergyBuff[0]+" 크리티컬 확률 : "+synergyBuff[1]+"\n공격 속도 : "+synergyBuff[2]+" 공격 범위 : "+synergyBuff[3];
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
            case "Archer":
                synergy[1]--;
                if(synergy[1] == 1){
                    synergyBuff[3] -= 30;
                }else if(synergy[1] == 3){
                    synergyBuff[3] -= 100;
                }
                break;
            case "Hunter":
                synergy[2]--;
                if(synergy[2] == 1){
                    synergyBuff[3] -= 30;
                }else if(synergy[2] == 3){
                    synergyBuff[3] -= 100;
                }
                break;
            case "Warrior":
                synergy[3]--;
                if(synergy[3] == 1){
                    synergyBuff[3] -= 30;
                }else if(synergy[3] == 3){
                    synergyBuff[3] -= 100;
                }
                break;
            case "Glacia":
                synergy[4]--;
                if(synergy[4] == 1){
                    synergyBuff[3] -= 30;
                }else if(synergy[4] == 3){
                    synergyBuff[3] -= 100;
                }
                break;
            case "Magician":
                synergy[5]--;
                if(synergy[5] == 1){
                    synergyBuff[3] -= 30;
                }else if(synergy[5] == 3){
                    synergyBuff[3] -= 100;
                }
                break;
            case "Pirate":
                synergy[6]--;
                if(synergy[6] == 1){
                    synergyBuff[3] -= 30;
                }else if(synergy[6] == 3){
                    synergyBuff[3] -= 100;
                }
                break;
            case "Priest":
                synergy[7]--;
                if(synergy[7] == 1){
                    synergyBuff[3] -= 30;
                }else if(synergy[7] == 3){
                    synergyBuff[3] -= 100;
                }
                break;
        }

        for(int i =0;i<8;i++){
            if(synergy[i]>0){
                switch(i){
                    case 0 :
                        bufftext += "Assasin : "+synergy[i]+"\n";
                        break;
                    case 1 :
                        bufftext += "Archer : "+synergy[i]+"\n";
                        break;
                    case 2 :
                        bufftext += "Hunter : "+synergy[i]+"\n";
                        break;
                    case 3 :
                        bufftext += "Warrior : "+synergy[i]+"\n";
                        break;
                    case 4 :
                        bufftext += "Glacia : "+synergy[i]+"\n";
                        break;
                    case 5 :
                        bufftext += "Magician : "+synergy[i]+"\n";
                        break;
                    case 6 :
                        bufftext += "Pirate : "+synergy[i]+"\n";
                        break;
                    case 7 :
                        bufftext += "Priest : "+synergy[i]+"\n";
                        break;


                }
            }
        }
        
        totalSynergy.text = "\n받고있는 효과 // \n공격력 : "+synergyBuff[0]+" 크리티컬 확률 : "+synergyBuff[1]+" 공격 속도 : "+synergyBuff[2]+" 공격 범위 : "+synergyBuff[3];
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