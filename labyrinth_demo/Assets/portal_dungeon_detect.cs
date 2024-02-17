using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class portal_dungeon_detect : MonoBehaviour
{
    public Transform playerdetect; // Reference to the player's transform.
    public float detectionDistance = 1f; // Define the detection distance.

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (playerdetect != null)
        {
            float distance = Vector3.Distance(playerdetect.position, transform.position);

            if (distance <= detectionDistance && Input.GetKeyDown(KeyCode.W))
            {
                
                SceneManager.LoadScene("select_dugeon");

            }
        }

    }
}
