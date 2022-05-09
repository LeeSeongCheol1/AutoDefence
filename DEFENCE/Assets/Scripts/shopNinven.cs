using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   
using TMPro;

public class shopNinven : MonoBehaviour
{
    [SerializeField]
    private GameObject shop;
    [SerializeField]
    private GameObject inventory;
    [SerializeField]
    private ObjectDetector detector;
    [SerializeField]
    private Reload reload;
    GameObject cancelObj;

    private void Awake(){
        shopClicked();
    }

    public void shopClicked(){
        shop.gameObject.SetActive(true);
        inventory.gameObject.SetActive(false);
        cancelObj = GameObject.FindGameObjectWithTag("Cancel");
        Destroy(cancelObj);
        detector.inventory.inventoryClicked = false;
        reload.checkPurchase();
    }

    public void inventoryClicked(){
        if(detector.towerSpawner.isOnTowerButton = true){
            detector.reload.buttons[detector.reload.clickednum].interactable = true;
        }
        shop.gameObject.SetActive(false);
        inventory.gameObject.SetActive(true);
        detector.cancelbuttonClicked();
        detector.towerSpawner.isOnTowerButton = false;
        cancelObj = GameObject.FindGameObjectWithTag("Cancel");
        Destroy(cancelObj);
    }
}