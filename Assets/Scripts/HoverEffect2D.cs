using UnityEngine;

public class HoverEffect2D : MonoBehaviour
{
    [Header("Hover Settings")]
    public float amplitude = 0.5f;   // How far up and down it moves
    public float frequency = 1f;     // How fast it moves

    private Vector3 startPos;

    private void Start()
    {
        startPos = transform.localPosition;
    }

    private void Update()
    {
        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.localPosition = new Vector3(startPos.x, startPos.y + yOffset, startPos.z);
    }
}
