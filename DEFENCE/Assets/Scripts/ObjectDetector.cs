using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;  

public class ObjectDetector : MonoBehaviour
{
    [SerializeField]
    public TowerSpawner towerSpawner;
    [SerializeField]
    private TowerDataViewer towerDataViewer;
    [SerializeField]
    private SystemTextViewer systemTextViewer;
    [SerializeField]
    private Button cancelButton;
    [SerializeField]
    public Reload reload;
    [SerializeField]
    private Synergy synergy;
    [SerializeField]
    public Inventory inventory;

    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;
    private Transform hitTransform = null;

    public bool moveStatus = false;

    GameObject gameObject1;
    GameObject gameObject2;
    GameObject[] cancelObj;
    Tile tile;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    async void Update()
    {
        // 마우스가 UI에 머물러 있을 때는 아래 코드 실행 x
        if (Input.GetMouseButtonDown(0))
        {
            systemTextViewer.PrintText(SystemType.zero);
            if(moveStatus == false){
                ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    hitTransform = hit.transform;
                    if (hit.transform.CompareTag("Tower"))
                    {
                        if(inventory.inventoryClicked){
                            TowerWeapon tower = hit.transform.GetComponent<TowerWeapon>();
                            if(tower.itemnum>2){
                                systemTextViewer.PrintText(SystemType.ItemMax);
                                return;
                            }
                                tower.equipItems(inventory.items[inventory.clickedInventory]);
                                Destroy(inventory.tempPrefab);
                                inventory.inventoryClicked = false;
                                inventory.arrangeItems(inventory.clickedInventory);
                        }else{
                            if(towerSpawner.isOnTowerButton == true)
                            {
                                systemTextViewer.PrintText(SystemType.Build);
                                return;
                            }
                            towerDataViewer.OnPanel(hit.transform);
                        }
                    }else if(hit.transform.CompareTag("Cancel")){
                        if(inventory.inventoryClicked){
                            Destroy(inventory.tempPrefab);
                            inventory.inventoryClicked = false;
                        }else{
                        Destroy(towerSpawner.tempPrefab);
                        towerSpawner.isOnTowerButton = false;
                        reload.buttons[reload.clickednum].interactable = true;
                        reload.btnClicked = false;
                        }
                    }else if(hit.transform.CompareTag("ExtraTile")){
                        reload.btnClicked = false;
                        towerSpawner.spawnExtraTower(hit.transform);
                    }else if (hit.transform.CompareTag("Tile"))
                    {
                        reload.btnClicked = false;
                        towerSpawner.SpawnTower(hit.transform);
                    }else if(hit.transform.CompareTag("Item")){
                        inventory.getItem(hit.transform);
                        Destroy(hit.transform.gameObject);
                    }
                }
            }else if(moveStatus == true)
            {
                ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if(Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    hitTransform = hit.transform;
                    if (hit.transform.CompareTag("Tower"))
                    {
                        hitTransform.tag = "moveTower2";
                        gameObject1 = GameObject.FindGameObjectWithTag("moveTower1");
                        gameObject2 = GameObject.FindGameObjectWithTag("moveTower2");

                        TowerWeapon weapon1 = gameObject1.GetComponent<TowerWeapon>();
                        TowerWeapon weapon2 = gameObject2.GetComponent<TowerWeapon>();

                        if(weapon1.disable == true){
                            weapon2.disable = true;
                            synergy.removeSynergy(weapon2.towersynergy);
                            weapon1.disable = false;
                            synergy.chkSynergy(weapon1.towersynergy);
                            weapon1.ChangeState(WeaponState.SearchTarget);
                        }else if(weapon2.disable == true){
                            weapon1.disable = true;
                            synergy.removeSynergy(weapon1.towersynergy);
                            weapon2.disable = false;
                            synergy.chkSynergy(weapon2.towersynergy);
                            weapon2.ChangeState(WeaponState.SearchTarget);
                        }

                        Vector3 vec = gameObject1.transform.position;
                        gameObject1.transform.position = gameObject2.transform.position;
                        gameObject2.transform.position = vec;

                        gameObject1.tag = "Tower";
                        gameObject2.tag = "Tower";

                        weapon1.EndMoveStatus();
                    }else if(hit.transform.CompareTag("ExtraTile")){
                        gameObject1 = GameObject.FindGameObjectWithTag("moveTower1");
                        gameObject1.transform.position = hit.transform.position + Vector3.back;
                        gameObject1.tag = "Tower";
                        TowerWeapon weapon1 = gameObject1.GetComponent<TowerWeapon>();
                        weapon1.EndMoveStatus();
                        if(weapon1.disable == false){
                            towerSpawner.towernum--;
                            synergy.removeSynergy(weapon1.towersynergy);
                        }
                        weapon1.ownerTile.IsBuildTower = false;
                        weapon1.disable = true;
                    }else if (hit.transform.CompareTag("Tile"))
                    {
                        gameObject1 = GameObject.FindGameObjectWithTag("moveTower1");
                        TowerWeapon weapon1 = gameObject1.GetComponent<TowerWeapon>();
                        tile = hit.transform.GetComponent<Tile>();
                        if(tile.IsBuildTower == true){
                                systemTextViewer.PrintText(SystemType.TowerMax);
                                return;
                            }
                        if(towerSpawner.towernum >= towerSpawner.maxnum && weapon1.disable == true){
                            systemTextViewer.PrintText(SystemType.TowerMax);
                            return;
                        }
                        gameObject1 = GameObject.FindGameObjectWithTag("moveTower1");
                        gameObject1.transform.position = hit.transform.position + Vector3.back; 
                        weapon1.ownerTile.IsBuildTower = false;
                        gameObject1.tag = "Tower";
                        weapon1.EndMoveStatus();
                        weapon1.ownerTile = tile;
                        weapon1.ownerTile.IsBuildTower = true;
                        if(weapon1.disable == true){
                            weapon1.disable = false;
                            weapon1.ChangeState(WeaponState.SearchTarget);
                            synergy.chkSynergy(weapon1.towersynergy);
                            towerSpawner.towernum++;
                            towerSpawner.UpdateTowerText();
                        }
                    }
                }
            }
        }else if (Input.GetMouseButtonUp(0))
        {
            // 마우스를 눌렀을 때 선택한 오브젝트가 없거나 타워가 아니라면
            if(hitTransform == null || hitTransform.CompareTag("Tower") == false)
            {
                // 정보 패널 비활성화
                towerDataViewer.OffPanel();
                cancelButton.gameObject.SetActive(false);
            }
        }
        towerSpawner.UpdateTowerText();
    }

    public void cancelbuttonClicked(){
        towerDataViewer.OffPanel();
        cancelButton.gameObject.SetActive(false);
        moveStatus = false;
    }
}
