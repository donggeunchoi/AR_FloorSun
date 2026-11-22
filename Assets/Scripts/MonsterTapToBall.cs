using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTapToBall : MonoBehaviour
{
    [Header("공 프리펩")]
    [SerializeField]
    GameObject ballPrefab;

    [Header("위치값")]
    [SerializeField]
    Vector3 cameraOffset;

    Camera arCamera;
    bool isBallSpawned;

    // Start is called before the first frame update
    void Start()
    {
        arCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (arCamera == null || ballPrefab == null || isBallSpawned == null || Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
            return;

        Ray ray = arCamera.ScreenPointToRay(touch.position);

        if(!Physics.Raycast(ray, out RaycastHit hit))
            return;

        if (hit.transform != transform)
            return;
        
        if (!hit.transform.IsChildOf(transform))
            return;

        Vector3 spawnPostion = arCamera.transform.TransformPoint(cameraOffset);
        Instantiate(ballPrefab, spawnPostion, arCamera.transform.rotation);
        isBallSpawned = true;
    }
}
