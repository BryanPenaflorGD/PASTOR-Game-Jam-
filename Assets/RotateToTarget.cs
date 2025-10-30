using UnityEngine;

public class RotateToTarget : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;          // how fast the head moves
    public float stopDistance = 0.1f;     // how close it gets before stopping

    [Header("Rotation Settings")]
    public float rotationSpeed = 20f;

    [Header("Target Reference")]
    public Transform target; // if null → uses mouse cursor

    void Update()
    {
        Vector3 targetPos;

        if (target == null)
        {
            // Get mouse world position
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = Mathf.Abs(Camera.main.transform.position.z); // distance from camera
            targetPos = Camera.main.ScreenToWorldPoint(mousePos);
        }
        else
        {
            targetPos = target.position;
        }

        // Compute direction and rotation
        Vector3 direction = targetPos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Move toward the target smoothly
        float distance = direction.magnitude;

        if (distance > stopDistance)
        {
            Vector3 moveDir = direction.normalized;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
    }
}
