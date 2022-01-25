using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderPositionAutoSetter : MonoBehaviour
{
    [SerializeField]
    private Vector3 distance = Vector3.down * 20.0f;
    private Transform targetTransform;
    private RectTransform rectTransform;

    public void Setup(Transform target)
    {
        // Slider UI가 쫓아다닐 target설정
        targetTransform = target;
        rectTransform = GetComponent<RectTransform>();
    }

    private void LateUpdate()
    {
        // 적이 파괴되어 쫓아다닐 대상이 사라지면 피통도 삭제
        if(targetTransform == null)
        {
            Destroy(gameObject);
            return;
        }

        // 오브젝트 월드 좌표를 기준으로 좌표값을 구함
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(targetTransform.position);
        // 화면 내에서 좌표 + distance만큼 떨어진 거리를 체력 UI위치로 설정
        rectTransform.position = screenPosition + distance;
    }
}
