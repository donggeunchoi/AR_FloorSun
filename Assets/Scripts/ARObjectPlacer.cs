using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEngine.PlayerLoop;


public class ARObjectPlacer : MonoBehaviour
{
    [Header("ARRaycast")]
    [SerializeField]
    ARRaycastManager raycastManager;

    [Header("Object")]
    [SerializeField]
    GameObject placePrefab;

    List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Update()
    {
        if (Input.touchCount <= 0)
            return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase != TouchPhase.Began)
            return;

        if(raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose pose = hits[0].pose;
            Instantiate(placePrefab,pose.position,pose.rotation);
        }
    }
}
