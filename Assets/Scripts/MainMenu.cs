using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro; // Added for TMP support

public class MainMenu : MonoBehaviour
{
    [Header("UI References")]
    public CanvasGroup menuCanvas;       // Assign your CanvasGroup
    public RectTransform[] buttons;      // Assign your Start, Exit, etc.
    public TextMeshProUGUI titleText;    // Assign your title text (TMP)
    public Image moonFlash;              // Assign your flash/star image

    [Header("Animation Settings")]
    public float fadeDuration = 1f;
    public float slideDistance = 30f;
    public float slideSpeed = 3f;

    [Header("Title Effect Settings")]
    public float delayBeforeTitle = 0.5f;   // Delay before title appears
    public float titleFadeDuration = 0.8f;  // Fade duration for title
    public float flashDuration = 0.6f;      // Duration of the star pulse
    public float flashScale = 1.5f;         // How large the pulse expands

    [Header("Moon Floating Effect")]
    public RectTransform moonTransform;     // Assign your moon image here
    public float floatAmplitude = 10f;      // How high it moves up/down
    public float floatSpeed = 1f;           // Speed of the floating motion

    void Start()
    {
        // Hide menu at start
        menuCanvas.alpha = 0f;
        foreach (var btn in buttons)
            btn.anchoredPosition -= new Vector2(0, slideDistance);

        // Hide title and flash at start
        if (titleText != null) titleText.alpha = 0f;
        if (moonFlash != null)
        {
            moonFlash.color = new Color(1, 1, 1, 0);
            moonFlash.transform.localScale = Vector3.zero;
        }

        // Start floating moon effect
        if (moonTransform != null)
            StartCoroutine(FloatMoon());

        // Play main animation
        StartCoroutine(FadeInMenu());
    }

    IEnumerator FadeInMenu()
    {
        float time = 0f;
        Vector2[] startPositions = new Vector2[buttons.Length];
        Vector2[] endPositions = new Vector2[buttons.Length];

        for (int i = 0; i < buttons.Length; i++)
        {
            startPositions[i] = buttons[i].anchoredPosition;
            endPositions[i] = startPositions[i] + new Vector2(0, slideDistance);
        }

        // Fade + slide in buttons
        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float t = time / fadeDuration;
            menuCanvas.alpha = Mathf.Lerp(0f, 1f, t);

            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].anchoredPosition = Vector2.Lerp(startPositions[i], endPositions[i], t);
            }

            yield return null;
        }

        menuCanvas.alpha = 1f;

        // Wait before the star pulse effect
        yield return new WaitForSeconds(delayBeforeTitle);

        // Play the moon/star pulse effect first
        yield return StartCoroutine(StarPulseEffect());

        // Then fade in the title
        yield return StartCoroutine(FadeInTitle());
    }

    IEnumerator StarPulseEffect()
    {
        if (moonFlash == null) yield break;

        float time = 0f;
        Vector3 originalScale = Vector3.one;

        // Pulse the star (scale up and fade out)
        while (time < flashDuration)
        {
            time += Time.deltaTime;
            float t = time / flashDuration;

            float scale = Mathf.Lerp(0f, flashScale, Mathf.Sin(t * Mathf.PI));
            moonFlash.transform.localScale = originalScale * scale;

            float alpha = Mathf.Sin(t * Mathf.PI);
            moonFlash.color = new Color(1, 1, 1, alpha);

            yield return null;
        }

        moonFlash.color = new Color(1, 1, 1, 0);
        moonFlash.transform.localScale = Vector3.zero;
    }

    IEnumerator FadeInTitle()
    {
        if (titleText == null) yield break;

        float time = 0f;

        while (time < titleFadeDuration)
        {
            time += Time.deltaTime;
            float t = time / titleFadeDuration;
            titleText.alpha = Mathf.SmoothStep(0, 1, t);
            yield return null;
        }

        titleText.alpha = 1f;
    }

   
    IEnumerator FloatMoon()
    {
        Vector3 startPos = moonTransform.anchoredPosition;
        while (true)
        {
            float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
            moonTransform.anchoredPosition = new Vector2(startPos.x, newY);
            yield return null;
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting...");
    }
}
