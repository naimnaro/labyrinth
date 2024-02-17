using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PRESSF_detect : MonoBehaviour
{
    public Transform knightdetect;
    public Transform knightdetect2;

    public float detectionDistance = 2f; // Define the detection distance.
    public GameObject PRESSF_text;
    // Start is called before the first frame update
    void Start()
    {
        PRESSF_text.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
        DetectAndTogglePrompt(knightdetect);
        DetectAndTogglePrompt(knightdetect2);


    }

    void DetectAndTogglePrompt(Transform detectedObject)
    {
        if (detectedObject != null)
        {
            float distance = Vector3.Distance(detectedObject.position, transform.position);
            

            if (distance <= detectionDistance)
            {
                
                PRESSF_text.SetActive(true);
            }
            else
            {
                
                // Check if both knightdetect and knightdetect2 are not within the detection distance
                if (Vector3.Distance(knightdetect.position, transform.position) > detectionDistance
                    && Vector3.Distance(knightdetect2.position, transform.position) > detectionDistance)
                {
                    PRESSF_text.SetActive(false);
                }
            }
        }
        else
        {
            
            PRESSF_text.SetActive(false);
        }
    }
}