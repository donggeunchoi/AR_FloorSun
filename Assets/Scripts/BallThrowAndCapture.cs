using UnityEngine;

public class BallThrowAndCapture : MonoBehaviour
{
    [SerializeField] private float minimumSwipeDistance;
    [SerializeField] private float swipeSpeedMultiplier;
    [SerializeField] private float minimumThrowSpeed;
    [SerializeField] private float maximumThrowSpeed;
    [SerializeField] private float horizontalAimStrength;
    [SerializeField] private float verticalAimStrength;

    private Camera arCamera;
    private Rigidbody ballRigidbody;
    private BallScreenAnchor screenAnchor;
    private Vector2 touchStartPosition;
    private float touchStartTime;
    private bool isTouchingBall;
    private bool hasBeenThrown;
    private bool hasCapturedMonster;

    private void Awake()
    {
        arCamera = Camera.main;
        ballRigidbody = GetComponent<Rigidbody>();
        screenAnchor = GetComponent<BallScreenAnchor>();
    }

    private void Update()
    {
        if (arCamera == null || ballRigidbody == null || Input.touchCount == 0 || hasBeenThrown) return;

        Touch touch = Input.GetTouch(0);

        if (touch.phase == TouchPhase.Began)
        {
            Ray ray = arCamera.ScreenPointToRay(touch.position);
            if (Physics.Raycast(ray, out RaycastHit hit) && (hit.transform == transform || hit.transform.IsChildOf(transform)))
            {
                isTouchingBall = true;
                touchStartPosition = touch.position;
                touchStartTime = Time.time;
            }
        }

        if (isTouchingBall && touch.phase == TouchPhase.Ended)
        {
            ThrowBall(touch.position);
        }

        if (touch.phase == TouchPhase.Canceled) 
            isTouchingBall = false;
    }

    private void ThrowBall(Vector2 releasePosition)
    {
        isTouchingBall = false;

        Vector2 swipe = releasePosition - touchStartPosition;

        if (swipe.magnitude < minimumSwipeDistance) 
            return;

        float duration = Mathf.Max(Time.time - touchStartTime, 0.05f);
        float throwSpeed = Mathf.Clamp(swipe.magnitude / duration * swipeSpeedMultiplier, minimumThrowSpeed, maximumThrowSpeed);
        Vector3 direction = arCamera.transform.forward + arCamera.transform.right * (swipe.x / Screen.height * horizontalAimStrength) + arCamera.transform.up * (swipe.y / Screen.height * verticalAimStrength);

        if (screenAnchor != null) 
            screenAnchor.enabled = false;

        ballRigidbody.isKinematic = false;
        ballRigidbody.useGravity = true;
        ballRigidbody.velocity = direction.normalized * throwSpeed;
        hasBeenThrown = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasCapturedMonster) 
            return;

        MonsterTapToBall monster = collision.collider.GetComponentInParent<MonsterTapToBall>();
        if (monster == null) 
            return;

        hasCapturedMonster = true;
        Destroy(monster.gameObject);
        Destroy(gameObject);
    }
}