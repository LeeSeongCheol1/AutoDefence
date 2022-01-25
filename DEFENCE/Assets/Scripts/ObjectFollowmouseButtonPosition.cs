﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollowmouseButtonPosition : MonoBehaviour
{
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // 마우스 좌표를 기준으로 월드 상의 좌표를 구함
        Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y);
        transform.position = mainCamera.ScreenToWorldPoint(position);

        // z위치를 0으로 설정
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}
