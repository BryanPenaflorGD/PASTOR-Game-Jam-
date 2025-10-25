using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow2D : MonoBehaviour
{

    [Header("Target")]
    public Transform target;

    [Header("Follow Settings")]
    public float followSpeed = 5f;
    public Vector2 offset;

    [Header("Vertical Lock")]
    public bool lockYAxis = false;

    private Vector3 targetPos;
    void LateUpdate()
    {
        if (target == null)
            return;

        targetPos = new Vector3(target.position.x + offset.x,
                                target.position.y + offset.y,
                                transform.position.z);

        if (lockYAxis)
            targetPos.y = transform.position.y;

        transform.position = Vector3.Lerp(transform.position, targetPos, followSpeed * Time.deltaTime);
    }
}
