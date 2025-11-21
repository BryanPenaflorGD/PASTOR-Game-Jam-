using UnityEngine;

public class FootstepFX : MonoBehaviour
{
    public ParticleSystem footstepFX;
    public Rigidbody2D rb; // or CharacterController for 3D
    public float minSpeedToPlay = 0.1f;

    private bool isPlaying = false;

    void Update()
    {
        bool isWalking = Mathf.Abs(rb.velocity.x) > minSpeedToPlay;

        if (isWalking && !isPlaying)
        {
            footstepFX.Play();
            isPlaying = true;
        }
        else if (!isWalking && isPlaying)
        {
            footstepFX.Stop();
            isPlaying = false;
        }
    }
}