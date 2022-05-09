using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private GameObject cancelPrefab;
    public int curritem = 0;
    public int[] items = new int[9];
    [SerializeField]
    private Button[] buttons;
    [SerializeField]
    private Item[] item;
    [SerializeField]
    private Image[] inventory;
    [SerializeField]
    private SystemTextViewer systemTextViewer;

    public bool inventoryClicked = false;
    public int clickedInventory = 0;
    public GameObject tempPrefab;
    int temp = 0;

    private void Awake(){
        updateItem();
    }

    public void updateItem(){
        if(curritem == 0){
            for(int j = curritem; j<9;j++){
                buttons[j].interactable = false;
                inventory[j].sprite = item[3].itemImage;
            }
            return;
        }else{
            for(int i = 0; i<curritem; i++){
                buttons[i].interactable = true;
                inventory[i].sprite = item[items[i]].itemImage;
            }
        }

        for(int j = curritem; j<9;j++){
                buttons[j].interactable = false;
                inventory[j].sprite = item[3].itemImage;
            }
    }

    public void arrangeItems(int num){
        for(int i =num; i<curritem; i++){
            temp = items[i+1];
            items[i] = temp;
        }
        curritem -= 1;
        updateItem();
    }

    public void getItem(Transform itemTransform){
        Items clickeditem = itemTransform.GetComponent<Items>();
        items[curritem] = clickeditem.type;
        curritem++;
        updateItem();
    }

    public void inventory0Clicked(){
        inventoryClicked = true;
        clickedInventory = 0;
        tempPrefab = Instantiate(cancelPrefab, new Vector3(-5.16f, 4.0f, 0), Quaternion.identity);
        systemTextViewer.PrintText(SystemType.itemClicked);
    }

    public void inventory1Clicked(){
        inventoryClicked = true;
        clickedInventory = 1;
        tempPrefab = Instantiate(cancelPrefab, new Vector3(-5.16f, 4.0f, 0), Quaternion.identity);
        systemTextViewer.PrintText(SystemType.itemClicked);
    }

    public void inventory2Clicked(){
        inventoryClicked = true;
        clickedInventory = 2;
        tempPrefab = Instantiate(cancelPrefab, new Vector3(-5.16f, 4.0f, 0), Quaternion.identity);
        systemTextViewer.PrintText(SystemType.itemClicked);
    }

    public void inventory3Clicked(){
        inventoryClicked = true;
        clickedInventory = 3;
        tempPrefab = Instantiate(cancelPrefab, new Vector3(-5.16f, 4.0f, 0), Quaternion.identity);
        systemTextViewer.PrintText(SystemType.itemClicked);
    }

    public void inventory4Clicked(){
        inventoryClicked = true;
        clickedInventory = 4;
        tempPrefab = Instantiate(cancelPrefab, new Vector3(-5.16f, 4.0f, 0), Quaternion.identity);
        systemTextViewer.PrintText(SystemType.itemClicked);
    }

    public void inventory5Clicked(){
        inventoryClicked = true;
        clickedInventory = 5;
        tempPrefab = Instantiate(cancelPrefab, new Vector3(-5.16f, 4.0f, 0), Quaternion.identity);
        systemTextViewer.PrintText(SystemType.itemClicked);
    }

    public void inventory6Clicked(){
        inventoryClicked = true;
        clickedInventory = 6;
        tempPrefab = Instantiate(cancelPrefab, new Vector3(-5.16f, 4.0f, 0), Quaternion.identity);
        systemTextViewer.PrintText(SystemType.itemClicked);
    }

    public void inventory7Clicked(){
        inventoryClicked = true;
        clickedInventory = 7;
        tempPrefab = Instantiate(cancelPrefab, new Vector3(-5.16f, 4.0f, 0), Quaternion.identity);
        systemTextViewer.PrintText(SystemType.itemClicked);
    }

    public void inventory8Clicked(){
        inventoryClicked = true;
        clickedInventory = 8;
        tempPrefab = Instantiate(cancelPrefab, new Vector3(-5.16f, 4.0f, 0), Quaternion.identity);
        systemTextViewer.PrintText(SystemType.itemClicked);
    }   
}
