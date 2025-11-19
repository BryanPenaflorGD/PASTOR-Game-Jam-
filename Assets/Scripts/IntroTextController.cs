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
    public float fadeSpeed = 2f;
    public float breatherTime = 1f; // UPDATED: pause before title
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
            if (introFinished)
            {
                SceneManager.LoadScene(nextSceneName);
                return;
            }

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
            // After last text → fade out last image and text
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

    // UPDATED: Fade out last image + text, wait breather, then fade in title
    IEnumerator FadeOutThenTitle()
    {
        isTransitioning = true;

        // Fade out last image and text
        StartCoroutine(FadeImage(introImages[imageIndex], 0));
        yield return StartCoroutine(FadeText(0));

        // Wait for breather
        yield return new WaitForSeconds(breatherTime);

        // Fade in title
        yield return StartCoroutine(FadeTitle());

        titleShown = true;
        isTransitioning = false;
    }

    IEnumerator FadeTitle()
    {
        isTransitioning = true;
        Color c = titleText.color;

        while (c.a < 1f)
        {
            c.a += fadeSpeed * Time.deltaTime;
            titleText.color = c;
            yield return null;
        }

        isTransitioning = false;
        introFinished = true;
    }
}
