using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class BallDragAndCapture : MonoBehaviour
{
    [Header("위치값")]
    [SerializeField]
    float planeOffset;

    Camera arCamera;
    ARRaycastManager arRaycastManager;
    BallScreenAnchor screenAnchor;
    Rigidbody ballRigidBody;
    bool isDragging;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake()
    {
        arCamera = Camera.main;
        arRaycastManager = FindObjectOfType<ARRaycastManager>();
        screenAnchor = GetComponent<BallScreenAnchor>();
        ballRigidBody = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        if (arCamera == null || arRaycastManager == null || Input.touchCount == 0)
            return;

        Touch touch = Input.GetTouch(0);

        if(touch.phase == TouchPhase.Began)
        {
            Ray ray = arCamera.ScreenPointToRay(touch.position);
            if (Physics.Raycast(ray, out RaycastHit hit) && (hit.transform == transform || hit.transform.IsChildOf(transform)))
            {
                isDragging = true;
                if (screenAnchor != null)
                    screenAnchor.enabled = false;
            }

            if(isDragging && (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary))
            {
                hits.Clear();
                if(arRaycastManager.Raycast(touch.position,hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose hitPose = hits[0].pose;
                    Vector3 targetPosition = hitPose.position + hitPose.up * planeOffset;

                    if (ballRigidBody != null)
                        ballRigidBody.MovePosition(targetPosition);
                    else
                        transform.position = targetPosition;
                }
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                isDragging = false;

        }


    }

    private void OnTriggerEnter(Collider other)
    {
        MonsterTapToBall monster = other.GetComponent<MonsterTapToBall>();

        if (monster != null)
            return;

        Destroy(monster.gameObject);
        Destroy(gameObject);
    }
}
