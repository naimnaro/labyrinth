using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testrange_2 : MonoBehaviour
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
        if (playerdetect != null && Input.GetKeyDown(KeyCode.F))
        {
            float distance = Vector3.Distance(playerdetect.position, transform.position);

            if (distance <= detectionDistance)
            {
                Debug.Log("Object clicked!!");
            }
        }

    }
}
