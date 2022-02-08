using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CameraRoomChange : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI bossText;
    [SerializeField]
    private GameObject bossVector;
    [SerializeField]
    private GameObject enemyVector;

    bool count = false;

    public void cameraMoveBossRoom(){
        if(count == false){
            transform.position = bossVector.transform.position;
            bossText.text = "되돌아가기";
            count = true;
        }else{
            transform.position = enemyVector.transform.position;
            bossText.text = "보스방";
            count = false;
        }
        
    }

}
