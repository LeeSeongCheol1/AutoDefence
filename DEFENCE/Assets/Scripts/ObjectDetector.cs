using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField]
    private TowerSpawner towerSpawner;
    [SerializeField]
    private TowerDataViewer towerDataViewer;

    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;
    private Transform hitTransform = null;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        // 마우스가 UI에 머물러 있을 때는 아래 코드 실행 x
        if( EventSystem.current.IsPointerOverGameObject() == true)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                hitTransform = hit.transform;
                if (hit.transform.CompareTag("Tile"))
                {
                    towerSpawner.SpawnTower(hit.transform);
                }else if (hit.transform.CompareTag("Tower"))
                {
                    towerDataViewer.OnPanel(hit.transform);
                }else if(hit.transform.CompareTag("ExtraTile")){
                    towerSpawner.spawnExtraTower(hit.transform);
                }
            }
        }else if (Input.GetMouseButtonUp(0))
        {
            // 마우스를 눌렀을 때 선택한 오브젝트가 없거나 타워가 아니라면
            if(hitTransform == null || hitTransform.CompareTag("Tower") == false)
            {
                // 정보 패널 비활성화
                towerDataViewer.OffPanel();
            }
        }
    }
}
