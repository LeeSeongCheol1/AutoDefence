using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class setting : MonoBehaviour
{
    public void gameSetting(){
        Time.timeScale = 0;
        SceneManager.LoadScene("SettingScene");
    }
}
