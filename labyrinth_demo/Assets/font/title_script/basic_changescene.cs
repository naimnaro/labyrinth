using UnityEngine;
using UnityEngine.SceneManagement;

public class basic_changescene : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene("Login");
        }
    }
}