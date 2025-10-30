using UnityEngine;
using TMPro;
using System.Collections;

public class creditsPanel : MonoBehaviour
{
    [Header("Credits Settings")]
    [TextArea(10, 50)]
    public string creditsContent;

    [Header("References")]
    public TextMeshProUGUI creditsText;
    public RectTransform viewport;

    [Header("Scroll Settings")]
    public float scrollSpeed = 50f;
    public bool autoStart = false;

    [Header("Fade Settings")]
    [Range(0f, 0.5f)] public float fadeZone = 0.2f; // Portion of viewport height for fading
    public bool enableFade = true;
    public float fadeDuration = 0.6f;

    private RectTransform creditsRect;
    private bool isScrolling = false;
    private bool isResetting = false;
    private bool hasLoopedOnce = false;

    private float bottomLimit;
    private float topLimit;
    private Color baseColor;

    void Awake()
    {
        creditsText.text = creditsContent;
        creditsRect = creditsText.GetComponent<RectTransform>();
        baseColor = creditsText.color;
    }

    void Start()
    {
        SetupLimits();
        ResetCredits();
        SetTextAlpha(1f);

        if (autoStart)
            StartCredits();
    }

    void Update()
    {
        if (!isScrolling || isResetting) return;

        creditsRect.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        if (enableFade)
            ApplyEdgeFade();

        // Trigger loop when fully offscreen top
        if (creditsRect.anchoredPosition.y >= topLimit)
            StartCoroutine(FadeOutAndReset());
    }

    public void StartCredits()
    {
        StopAllCoroutines();
        ResetCredits();
        SetTextAlpha(1f);
        isScrolling = true;
        hasLoopedOnce = false;
    }

    public void StopCredits()
    {
        StopAllCoroutines();
        isScrolling = false;
        ResetCredits();
        SetTextAlpha(1f);
        hasLoopedOnce = false;
    }

    private void ResetCredits()
    {
        creditsRect.anchoredPosition = new Vector2(creditsRect.anchoredPosition.x, bottomLimit);
    }

    private void SetupLimits()
    {
        float viewportHeight = viewport.rect.height;
        float textHeight = creditsRect.rect.height;
        bottomLimit = -textHeight - 10f;
        topLimit = viewportHeight + 10f;
    }

    private void ApplyEdgeFade()
    {
        float viewportHeight = viewport.rect.height;
        float y = creditsRect.anchoredPosition.y;
        float normalizedY = Mathf.InverseLerp(bottomLimit, topLimit, y);

        float fadeInStart = fadeZone;
        float fadeOutStart = 1f - fadeZone;
        float alpha = 1f;

        if (normalizedY < fadeInStart)
            alpha = Mathf.InverseLerp(0f, fadeInStart, normalizedY);
        else if (normalizedY > fadeOutStart)
            alpha = Mathf.InverseLerp(1f, fadeOutStart, normalizedY);

        SetTextAlpha(alpha);
    }

    private void SetTextAlpha(float a)
    {
        Color c = baseColor;
        c.a = a;
        creditsText.color = c;
    }

    private IEnumerator FadeOutAndReset()
    {
        isResetting = true;

        // Fade out while still scrolling
        float startAlpha = creditsText.color.a;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            SetTextAlpha(Mathf.Lerp(startAlpha, 0f, t / fadeDuration));
            creditsRect.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
            yield return null;
        }

        // Fully invisible now — reset position
        SetTextAlpha(0f);
        ResetCredits();

        // Prevent flash at the very first loop
        if (!hasLoopedOnce)
        {
            yield return new WaitForSeconds(0.05f); // brief delay avoids visual pop
            hasLoopedOnce = true;
        }

        // Fade back in smoothly
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            SetTextAlpha(Mathf.Lerp(0f, 1f, t / fadeDuration));
            yield return null;
        }

        SetTextAlpha(1f);
        isResetting = false;
    }
}
