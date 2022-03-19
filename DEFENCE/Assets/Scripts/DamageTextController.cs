﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageTextController : MonoBehaviour
{
    private static DamageTextController _instance = null;

    public static DamageTextController Instance {
        get {
            if(_instance == null) {

                _instance = GameObject.FindObjectOfType<DamageTextController>();

                if (_instance == null) {
                    Debug.LogError("There's no active DamageTextController Object");
                }
            }

            return _instance;
        }
    }

    public Canvas canvas;
    public GameObject dmgTxt;

    public void CreateDamageText(Vector3 hitPoint, int hitDamage) {
        GameObject damageText = Instantiate(dmgTxt, hitPoint, Quaternion.identity, canvas.transform);
        damageText.GetComponent<Text>().text = hitDamage.ToString();
    }
}