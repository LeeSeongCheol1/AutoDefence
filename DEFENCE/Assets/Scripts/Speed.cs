using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Speed : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI speedTXT;

    bool fastmode = false;

    public void fastbuttonClicked(){
        if(fastmode == false){
            fastmode = true;
            Time.timeScale = 2f;
            speedTXT.text = "x2";
        }else{
            fastmode = false;
            Time.timeScale = 1f;
            speedTXT.text = "x1";
        }
    }

}
