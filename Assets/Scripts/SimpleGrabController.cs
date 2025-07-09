using UnityEngine;

public class SimpleGrabController : MonoBehaviour
{
    private bool isGrabbing = false;
    private Transform handTransform;

    private Vector3 grabOffsetPosition;
    private Quaternion grabOffsetRotation;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void StartGrab(Transform hand)
    {
        isGrabbing = true;
        handTransform = hand;

        // Save offsets
        grabOffsetPosition = handTransform.InverseTransformPoint(transform.position);
        grabOffsetRotation = Quaternion.Inverse(handTransform.rotation) * transform.rotation;

        // Make sure Rigidbody doesn't interfere
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.interpolation = RigidbodyInterpolation.Interpolate;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
    }

    public void EndGrab()
    {
        isGrabbing = false;
        handTransform = null;

        // Optional: re-enable physics if you want it to fall after dropping
        if (rb != null)
        {
            rb.isKinematic = false;
        }
    }

    void FixedUpdate()
    {
        if (isGrabbing && handTransform != null)
        {
            transform.position = handTransform.TransformPoint(grabOffsetPosition);
            transform.rotation = handTransform.rotation * grabOffsetRotation;
        }
    }
}
