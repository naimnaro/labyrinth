using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class vlliage_1_off : MonoBehaviour
{
    public Transform playerdetect; // Reference to the player's transform.
    public float detectionDistance = 1f; // Define the detection distance.
    public GameObject village_title_text; //

    


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

            if (distance <= detectionDistance)
            {
                
                village_title_text.SetActive(false);
                




            }
        }

    }
}
