using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testrange : MonoBehaviour
{
    public float detectionRange = 2f; // Define the detection range.
    public LayerMask playerLayer; // Set the player layer in the Inspecto
    // Start is called before the first frame update
    void Start()
    {
       

    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRange, playerLayer);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("PLAYER"))
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    Debug.Log("box clicked !!");
                }
            }
        }




    }
        
    
}
