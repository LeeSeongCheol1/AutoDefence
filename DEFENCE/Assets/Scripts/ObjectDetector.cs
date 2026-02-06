using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField] public TowerSpawner towerSpawner;
    [SerializeField] private TowerDataViewer towerDataViewer;
    [SerializeField] private SystemTextViewer systemTextViewer;
    [SerializeField] private UnityEngine.UI.Button cancelButton;
    [SerializeField] public Reload reload;
    [SerializeField] private Synergy synergy;
    [SerializeField] public Inventory inventory;

    private Camera mainCamera;
    public bool moveStatus = false;
    private TowerWeapon currentMovingTower;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(0)) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
        {
            if (moveStatus)
                HandleMoveMode(hit.transform);
            else
                HandleNormalMode(hit.transform);
        }
    }

    private void HandleNormalMode(Transform hitTransform)
    {
        systemTextViewer.PrintText(SystemType.zero);

        switch (hitTransform.tag)
        {
            case "Tower":
                HandleTowerClick(hitTransform);
                break;
            case "Cancel":
                HandleCancelClick();
                break;
            case "ExtraTile":
                reload.btnClicked = false;
                towerSpawner.spawnExtraTower(hitTransform);
                towerSpawner.UpdateTowerText();
                break;
            case "Tile":
                reload.btnClicked = false;
                towerSpawner.SpawnTower(hitTransform);
                towerSpawner.UpdateTowerText();
                break;
            case "Item":
                inventory.getItem(hitTransform);
                Destroy(hitTransform.gameObject);
                break;
        }
    }

    private void HandleMoveMode(Transform hitTransform)
    {
        if (currentMovingTower == null)
        {
            EndMoveStatus();
            return;
        }

        switch (hitTransform.tag)
        {
            case "Tower":
                SwapTowers(hitTransform.GetComponent<TowerWeapon>());
                break;
            case "ExtraTile":
                MoveToStorage(hitTransform);
                break;
            case "Tile":
                MoveToTile(hitTransform);
                break;
        }
    }

    private void HandleTowerClick(Transform hitTransform)
    {
        if (inventory.inventoryClicked)
        {
            TowerWeapon tower = hitTransform.GetComponent<TowerWeapon>();
            if (tower.itemnum > 2)
            {
                systemTextViewer.PrintText(SystemType.ItemMax);
                return;
            }
            tower.equipItems(inventory.items[inventory.clickedInventory]);
            Destroy(inventory.tempPrefab);
            inventory.inventoryClicked = false;
            inventory.arrangeItems(inventory.clickedInventory);
        }
        else
        {
            if (towerSpawner.isOnTowerButton)
            {
                systemTextViewer.PrintText(SystemType.Build);
                return;
            }
            towerDataViewer.OnPanel(hitTransform);
        }
    }

    private void HandleCancelClick()
    {
        if (inventory.inventoryClicked)
        {
            Destroy(inventory.tempPrefab);
            inventory.inventoryClicked = false;
        }
        else
        {
            Destroy(towerSpawner.tempPrefab);
            towerSpawner.isOnTowerButton = false;
            if (reload.clickednum >= 0 && reload.clickednum < reload.buttons.Length)
                reload.buttons[reload.clickednum].interactable = true;
            reload.btnClicked = false;
        }
    }

    private void SwapTowers(TowerWeapon targetTower)
    {
        if (currentMovingTower.disable)
        {
            targetTower.disable = true;
            synergy.removeSynergy(targetTower.towersynergy);

            currentMovingTower.disable = false;
            synergy.chkSynergy(currentMovingTower.towersynergy);
            currentMovingTower.ChangeState(WeaponState.SearchTarget);
        }
        else if (targetTower.disable)
        {
            currentMovingTower.disable = true;
            synergy.removeSynergy(currentMovingTower.towersynergy);

            targetTower.disable = false;
            synergy.chkSynergy(targetTower.towersynergy);
            targetTower.ChangeState(WeaponState.SearchTarget);
        }

        Vector3 tempPos = currentMovingTower.transform.position;
        currentMovingTower.transform.position = targetTower.transform.position;
        targetTower.transform.position = tempPos;

        EndMoveStatus();
        towerSpawner.UpdateTowerText();
    }

    private void MoveToStorage(Transform extraTileTr)
    {
        currentMovingTower.transform.position = extraTileTr.position + Vector3.back;

        if (!currentMovingTower.disable)
        {
            towerSpawner.towernum--;
            synergy.removeSynergy(currentMovingTower.towersynergy);
        }

        currentMovingTower.ownerTile.IsBuildTower = false;
        currentMovingTower.disable = true;

        EndMoveStatus();
        towerSpawner.UpdateTowerText();
    }

    private void MoveToTile(Transform tileTr)
    {
        Tile tile = tileTr.GetComponent<Tile>();

        if (tile.IsBuildTower)
        {
            systemTextViewer.PrintText(SystemType.TowerMax);
            return;
        }

        if (towerSpawner.towernum >= towerSpawner.maxnum && currentMovingTower.disable)
        {
            systemTextViewer.PrintText(SystemType.TowerMax);
            return;
        }

        currentMovingTower.transform.position = tileTr.position + Vector3.back;
        currentMovingTower.ownerTile.IsBuildTower = false;

        currentMovingTower.ownerTile = tile;
        currentMovingTower.ownerTile.IsBuildTower = true;

        if (currentMovingTower.disable)
        {
            currentMovingTower.disable = false;
            currentMovingTower.ChangeState(WeaponState.SearchTarget);
            synergy.chkSynergy(currentMovingTower.towersynergy);
            towerSpawner.towernum++;
        }

        EndMoveStatus();
        towerSpawner.UpdateTowerText();
    }

    public void StartMove(TowerWeapon tower)
    {
        currentMovingTower = tower;
        moveStatus = true;
        systemTextViewer.MovePrint(true);
    }

    public void EndMoveStatus()
    {
        moveStatus = false;
        systemTextViewer.MovePrint(false);
        cancelButton.gameObject.SetActive(false);
        towerDataViewer.OffPanel();

        if (currentMovingTower != null)
        {
            currentMovingTower.EndMoveStatus();
            currentMovingTower = null;
        }
    }

    public void cancelbuttonClicked()
    {
        EndMoveStatus();
    }
}