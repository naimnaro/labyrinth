using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class village_1 : MonoBehaviour
{
   
    public Transform playerdetect; // Reference to the player's transform.
    public float detectionDistance = 1f; // Define the detection distance.
    public GameObject village_title_text; 
    private Animator textAnimator;


    // Start is called before the first frame update
    void Start()
    {
        textAnimator = village_title_text.GetComponent<Animator>();
        village_title_text.SetActive(false);


    }
 
    // Update is called once per frame
    void Update()
    {
        if (playerdetect != null)
        {
            float distance = Vector3.Distance(playerdetect.position, transform.position);

            if (distance <= detectionDistance)
            {
                village_title_text.SetActive(true);
                textAnimator.SetBool("fadein", true);
                Invoke("SetFadeinToFalse", 1f);

            }
        }
    }

    void SetFadeinToFalse()
    {
        textAnimator.SetBool("fadein", false);
       
    }

    
}
