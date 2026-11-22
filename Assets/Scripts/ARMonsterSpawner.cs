using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARMonsterSpawner : MonoBehaviour
{
    [Header("ARPlaneManager")]
    [SerializeField]
    ARPlaneManager planeManager;

    [Header("몬스터 프리펩")]
    [SerializeField]
    GameObject monsterPrefab;

    [Header("위치값")]
    [SerializeField]
    float groundOffset;

    bool hasSpawned;

    // Update is called once per frame
    void Update()
    {
        if (hasSpawned || planeManager == null || monsterPrefab == null)
            return;

        foreach( ARPlane plane in planeManager.trackables)
        {
            if (plane.trackingState != TrackingState.Tracking)
                continue;

            if (plane.alignment != PlaneAlignment.HorizontalUp)
                continue;

            Vector3 spawnPosition = plane.transform.TransformPoint(plane.center);
            spawnPosition += Vector3.up * groundOffset;
            Instantiate(monsterPrefab, spawnPosition, Quaternion.identity);
            hasSpawned = true;
            break;

        }
    }
}
