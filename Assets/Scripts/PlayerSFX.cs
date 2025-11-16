using UnityEngine;

public class PlayerSFX : MonoBehaviour
{
    public AudioSource source;

    [Header("Footstep Clips")]
    public AudioClip[] walkClips;

    [Header("Other Clips")]
    public AudioClip jump;
    public AudioClip land;

    public float stepInterval = 0.8f; // Your correct rhythm
    private float nextStepTime = 0f;

    public void PlayWalk()
    {
        if (Time.time < nextStepTime) return;
        if (walkClips == null || walkClips.Length == 0) return;

        // Pick random clip
        AudioClip clip = walkClips[Random.Range(0, walkClips.Length)];

        // Randomize pitch (slight variation)
        source.pitch = Random.Range(0.9f, 1.1f);

        source.PlayOneShot(clip);

        nextStepTime = Time.time + stepInterval;
    }

    public void PlayJump()
    {
        if (jump != null)
            source.volume = 0.3f;
            source.PlayOneShot(jump);
    }

    public void PlayLand()
    {
        if (land != null)
            source.PlayOneShot(land);
    }
}
