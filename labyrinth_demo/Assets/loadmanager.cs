using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class loadmanager : MonoBehaviour
{
    public GameObject firebaseload;
    public GameObject enemyload1;
    private void Start()
    {
        firebaseload.SetActive(false);
        enemyload1.SetActive(false);
        // Start the coroutine to load the other scene after 3 seconds
        StartCoroutine(LoadOtherstats());
    }

    private IEnumerator LoadOtherstats()
    {
        // Wait for 3 seconds
        yield return new WaitForSeconds(0.1f);

        // Load the other scene
        firebaseload.SetActive(true);
        enemyload1.SetActive(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("village1");
        }
    }
}