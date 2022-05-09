using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum SystemType { Money = 0, Build, TowerMax, ItemMax, zero, towerClicked, itemClicked}

public class SystemTextViewer : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textSystem;
    private TMPAlpha tmpAlpha;

    private void Awake()
    {
        textSystem = GetComponent<TextMeshProUGUI>();
        tmpAlpha = GetComponent<TMPAlpha>();
    }

    public void PrintText(SystemType type)
    {
        switch (type)
        {
            case SystemType.Money:
                textSystem.text = "타워가 부족합니다.";
                break;
            case SystemType.Build:
                textSystem.text = "타워를 지을 수 없습니다.";
                break;
            case SystemType.TowerMax:
                textSystem.text = "더 이상 타워를 배치 할 수 없습니다.";
                break;
            case SystemType.ItemMax:
                textSystem.text = "아이템 최대 소지 수는 3개입니다.";
                break;
            case SystemType.zero:
                textSystem.text = "";
                break;
            case SystemType.towerClicked:
                textSystem.text = "타워를 지을 곳을 선택해주세요.";
                break;
            case SystemType.itemClicked:
                textSystem.text = "아이템을 장착할 타워를 선택해주세요.";
                break;  
        }
    }

    public void MovePrint(bool status){
        Color color = textSystem.color;
        color.a = 255;
        textSystem.color = color;
        if(status == true){
            textSystem.text = "타워를 움직일 곳을 선택해주세요.";
        }else{
            textSystem.text = "";
        }
    }
}
