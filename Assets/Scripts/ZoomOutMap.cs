using UnityEngine;

public class ZoomOutMap : MonoBehaviour
{
    public Transform player;        // Reference to player
    public Vector3 offset = new Vector3(0, 20, -20);  // Camera offset from player
    public float smoothSpeed = 5f;  // Follow smoothing

    void LateUpdate()
    {
        if (player == null) return;

        // Follow the player smoothly
        Vector3 targetPos = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * smoothSpeed);

        // Always look at the player from above
        transform.LookAt(player);
    }
}
