using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScreenAnchor : MonoBehaviour
{
    Camera arCamera;

    [Header("위치값")]
    [SerializeField]
    Vector3 cameraOffset;

    // Start is called before the first frame update
    void Start()
    {
        arCamera = Camera.main;

        if(arCamera == null)
        {
            Debug.LogError("AR카메라 찾을 수 없음");
            enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (arCamera == null)
            return;

        transform.position = arCamera.transform.TransformPoint(cameraOffset);
        transform.rotation = arCamera.transform.rotation;
    }
}
