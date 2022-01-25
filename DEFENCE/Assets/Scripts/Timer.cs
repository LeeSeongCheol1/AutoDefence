using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Timer : MonoBehaviour
{

    GameObject waveStart;
    GameObject EXP;
    public float LimitTime = 60;

    [SerializeField]
    private TextMeshProUGUI text_Timer;

    private void Awake()
    {
        waveStart = GameObject.Find("StartWave");
        EXP = GameObject.Find("LEVEL");
    }
        
    void Update()
    {
        if (LimitTime <= 0)
        {
            waveStart.GetComponent<WaveSystem>().StartWave();
            LimitTime = 60;
            EXP.GetComponent<PlayerLV>().levelUp(2);
        }
        LimitTime -= Time.deltaTime;
        text_Timer.text = "다음 웨이브까지 :  " + Mathf.Round(LimitTime);
    }
}
