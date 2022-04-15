using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOver : MonoBehaviour
{
    [SerializeField]
    private Button gameOverbtn;

    public void gameOver(){
        Time.timeScale = 0f;
        gameOverbtn .gameObject.SetActive(true);
    }
}
