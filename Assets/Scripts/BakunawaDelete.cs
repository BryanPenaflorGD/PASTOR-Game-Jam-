using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BakunawaDelete : MonoBehaviour
{
    // Reference to the object you want to destroy (assign in Inspector)
    public GameObject targetObject;
    public GameObject targetObject1;

    // Delay before destroying
    public float destroyDelay = 5f;

    void Start()
    {
        // Start the coroutine if a target is assigned
        if (targetObject != null)
        {
            StartCoroutine(DestroyAfterDelay());
        }
        else
        {
            Debug.LogWarning("No target object assigned to destroy!");
        }
    }

    IEnumerator DestroyAfterDelay()
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(destroyDelay);

        // Destroy the referenced object
        Destroy(targetObject);
        Destroy(targetObject1);

        StartCoroutine(ReturnMainMenu());
    }

    IEnumerator ReturnMainMenu()
    {
        yield return new WaitForSeconds(destroyDelay);

        SceneManager.LoadScene("MainMenu");
    }
}
