using UnityEngine;
using System.Collections.Generic;

public class BakunawaBodyMovement : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform target;            // The segment or head in front
    public float followDistance = 1f;   // Desired spacing from the target
    public float moveSmoothness = 12f;  // How tightly it follows the path

    private Queue<Vector3> pathHistory = new Queue<Vector3>();
    private float distanceTraveled = 0f;
    private Vector3 lastTargetPos;

    void Start()
    {
        if (target != null)
            lastTargetPos = target.position;
    }

    void Update()
    {
        if (target == null) return;

        // --- Record target's path continuously ---
        float dist = Vector3.Distance(lastTargetPos, target.position);
        if (dist > 0.001f)
        {
            distanceTraveled += dist;
            pathHistory.Enqueue(target.position);
            lastTargetPos = target.position;
        }

        // --- Keep only points within a certain path length ---
        float neededLength = followDistance * 10f; // keep a small trail buffer
        while (pathHistory.Count > 1 && Vector3.Distance(pathHistory.Peek(), target.position) > neededLength)
            pathHistory.Dequeue();

        // --- Find a point on the path that is 'followDistance' away from target ---
        Vector3[] points = pathHistory.ToArray();
        Vector3 desiredPos = target.position;

        float cumulativeDist = 0f;
        for (int i = points.Length - 1; i > 0; i--)
        {
            float segDist = Vector3.Distance(points[i], points[i - 1]);
            cumulativeDist += segDist;

            if (cumulativeDist >= followDistance)
            {
                float overshoot = cumulativeDist - followDistance;
                desiredPos = Vector3.Lerp(points[i], points[i - 1], overshoot / segDist);
                break;
            }
        }

        // --- Smoothly move toward that point ---
        transform.position = Vector3.Lerp(transform.position, desiredPos, moveSmoothness * Time.deltaTime);

        // --- Flip based on target direction ---
        if (target.localScale.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * Mathf.Sign(target.localScale.x);
            transform.localScale = scale;
        }
    }
}
