using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    private void Start()
    {
        // Start the coroutine to load the other scene after 3 seconds
        StartCoroutine(LoadOtherScene());
    }

    private IEnumerator LoadOtherScene()
    {
        // Wait for 3 seconds
        yield return new WaitForSeconds(1f);

        // Load the other scene
        SceneManager.LoadScene("village1");
    }
}