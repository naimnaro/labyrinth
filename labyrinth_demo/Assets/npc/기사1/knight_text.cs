using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knight_text : MonoBehaviour
{
    public Transform playerdetect; // Reference to the player's transform.
    public float detectionDistance = 2f; // Define the detection distance.
    public GameObject knight_text1; // Reference to the chatbox GameObject.

    private bool chatActive = false;

    void Start()
    {
        knight_text1.SetActive(false);
    }

    void Update()
    {
        if (playerdetect != null)
        {
            float distance = Vector3.Distance(playerdetect.position, transform.position);

            if (distance <= detectionDistance && Input.GetKeyDown(KeyCode.F))
            {
                if (!chatActive)
                {
                    knight_text1.SetActive(true);
                    chatActive = true;
                    Debug.Log("Object clicked - chatbox turned on!");

                    // Start a coroutine to turn off the chatbox after 5 seconds.
                    StartCoroutine(TurnOffChatboxAfterDelay(3f));
                }
            }
        }
    }

    // Coroutine to turn off the chatbox after a specified delay.
    IEnumerator TurnOffChatboxAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Turn off the chatbox and reset chatActive.
        knight_text1.SetActive(false);
        chatActive = false;
        Debug.Log("Chatbox turned off after delay.");
    }
}
