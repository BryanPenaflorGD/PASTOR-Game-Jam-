using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class AutoRestartWithFade : MonoBehaviour
{
    [Header("Game Over Settings")]
    public GameObject gameOverUI;          // Your "You Died" panel
    public Image backgroundOverlay;        // Fullscreen black image for fade
    public float restartDelay = 3f;        // Time before restart
    public string playerTag = "Player";    // Tag for player
    public string enemyTag = "Enemy";      // Tag for enemies
    public float fadeDuration = 1.5f;      // Fade-in speed

    private bool isGameOver = false;
    private CanvasGroup uiGroup;

    void Start()
    {
        
        if (gameOverUI != null)
        {
            uiGroup = gameOverUI.GetComponent<CanvasGroup>();
            if (uiGroup == null)
                uiGroup = gameOverUI.AddComponent<CanvasGroup>();

            uiGroup.alpha = 0f;
            gameOverUI.SetActive(false);
        }

        
        if (backgroundOverlay != null)
        {
            Color c = backgroundOverlay.color;
            c.a = 0f;
            backgroundOverlay.color = c;
            backgroundOverlay.gameObject.SetActive(false);
        }
    }

    
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(enemyTag) && !isGameOver)
        {
            GameOver();
        }
    }

    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(enemyTag) && !isGameOver)
        {
            GameOver();
        }
    }

    
    void Update()
    {
        if (!isGameOver && PlayerIsDead())
        {
            GameOver();
        }
    }

    bool PlayerIsDead()
    {
        var player = GameObject.FindWithTag(playerTag);
        return player == null;
    }

    void GameOver()
    {
        isGameOver = true;

        if (backgroundOverlay != null)
            backgroundOverlay.gameObject.SetActive(true);

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
            StartCoroutine(FadeInEffect());
        }

        
        Time.timeScale = 0f;

        
        StartCoroutine(RestartSceneAfterDelay());
    }

    IEnumerator FadeInEffect()
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);

            // Fade in UI text/panel
            if (uiGroup != null)
                uiGroup.alpha = alpha;

            // Fade dark background
            if (backgroundOverlay != null)
            {
                Color c = backgroundOverlay.color;
                c.a = Mathf.Lerp(0f, 0.6f, alpha); // 0.6f = target opacity
                backgroundOverlay.color = c;
            }

            yield return null;
        }
    }

    IEnumerator RestartSceneAfterDelay()
    {
        yield return new WaitForSecondsRealtime(restartDelay);
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}



