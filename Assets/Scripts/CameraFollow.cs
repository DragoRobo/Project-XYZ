using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;    // the player
    public float smoothSpeed = 0.125f; // how smooth the camera moves
    public Vector3 offset;      // offset from the player (optional)

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, transform.position.z);
    }
}