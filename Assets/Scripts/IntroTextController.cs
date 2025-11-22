using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class IntroTextController : MonoBehaviour
{
    [Header("Intro Text")]
    [TextArea(3, 10)]
    public string[] storyTexts;
    public TextMeshProUGUI textUI;

    [Header("Intro Images")]
    public Image[] introImages;

    [Header("Title")]
    public TextMeshProUGUI titleText;

    [Header("Settings")]
    public float fadeSpeed = 3f;
    public float breatherTime = 1f;
    public float titleHoldTime = 2.5f;  // NEW — how long the title stays before fading out
    public string nextSceneName = "MainMenu";

    private int textIndex = 0;
    private int imageIndex = 0;
    private int clickCount = 0;

    private bool isTransitioning = false;
    private bool introFinished = false;
    private bool titleShown = false;

    void Start()
    {
        foreach (var img in introImages)
        {
            var c = img.color;
            c.a = 0;
            img.color = c;
        }

        var ct = titleText.color;
        ct.a = 0;
        titleText.color = ct;

        textUI.text = "";

        StartCoroutine(FadeImage(introImages[0], 1));
        StartCoroutine(ShowText(storyTexts[0]));
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !isTransitioning)
        {
            if (introFinished) return; // No more clicking after title

            clickCount++;

            if (!titleShown)
            {
                if (clickCount >= 3)
                {
                    clickCount = 0;
                    SwitchImage();
                }

                NextText();
            }
        }
    }

    void SwitchImage()
    {
        if (imageIndex < introImages.Length - 1)
        {
            StartCoroutine(FadeImage(introImages[imageIndex], 0));
            imageIndex++;
            StartCoroutine(FadeImage(introImages[imageIndex], 1));
        }
    }

    void NextText()
    {
        textIndex++;

        if (textIndex >= storyTexts.Length)
        {
            StartCoroutine(FadeOutThenTitle());
            return;
        }

        StartCoroutine(ShowText(storyTexts[textIndex]));
    }

    IEnumerator ShowText(string newText)
    {
        isTransitioning = true;
        yield return StartCoroutine(FadeText(0));

        textUI.text = newText;

        yield return StartCoroutine(FadeText(1));
        isTransitioning = false;
    }

    IEnumerator FadeText(float target)
    {
        Color c = textUI.color;

        while (!Mathf.Approximately(c.a, target))
        {
            c.a = Mathf.MoveTowards(c.a, target, fadeSpeed * Time.deltaTime);
            textUI.color = c;
            yield return null;
        }
    }

    IEnumerator FadeImage(Image img, float target)
    {
        isTransitioning = true;
        Color c = img.color;

        while (!Mathf.Approximately(c.a, target))
        {
            c.a = Mathf.MoveTowards(c.a, target, fadeSpeed * Time.deltaTime);
            img.color = c;
            yield return null;
        }

        isTransitioning = false;
    }

    IEnumerator FadeOutThenTitle()
    {
        isTransitioning = true;

        StartCoroutine(FadeImage(introImages[imageIndex], 0));
        yield return StartCoroutine(FadeText(0));

        yield return new WaitForSeconds(breatherTime);

        yield return StartCoroutine(FadeTitle()); // now auto handles fade out + next scene

        titleShown = true;
        isTransitioning = false;
    }

    // UPDATED: Fade in title → hold → fade out → load next scene
    IEnumerator FadeTitle()
    {
        isTransitioning = true;
        Color c = titleText.color;

        // Fade IN
        while (c.a < 1f)
        {
            c.a += fadeSpeed * Time.deltaTime;
            titleText.color = c;
            yield return null;
        }

        // Hold title visible
        yield return new WaitForSeconds(titleHoldTime);

        // Fade OUT title
        while (c.a > 0f)
        {
            c.a -= fadeSpeed * Time.deltaTime;
            titleText.color = c;
            yield return null;
        }

        // Auto go to next scene
        SceneManager.LoadScene(nextSceneName);

        introFinished = true;
        isTransitioning = false;
    }
}
