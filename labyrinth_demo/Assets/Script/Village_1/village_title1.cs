using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class village_title1 : MonoBehaviour
{
    public float detectionDistance = 1f;
    public GameObject village_title_text;
    private Animator textAnimator;

    public Transform village_on;
    public Transform village_off;

    bool istexton = false;
    // Start is called before the first frame update
    void Start()
    {
        textAnimator = village_title_text.GetComponent<Animator>();
        village_title_text.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        if (village_on != null)
        {
            float distance = Vector3.Distance(village_on.position, transform.position);

            if (distance <= detectionDistance)
            {
                if (istexton == false)
                {
                    istexton = true;
                    village_title_text.SetActive(true);
                    textAnimator.SetBool("fadein", true);
                    Invoke("SetFadeinToFalse", 1f);

                }
                

            }
        }
        if (village_off != null)
        {
            float distance = Vector3.Distance(village_off.position, transform.position);

            if (distance <= detectionDistance)
            {
                if (istexton == true)
                {
                    istexton = false;
                    textAnimator.SetBool("fadeout", true);
                    Invoke("SetFadeout", 1f);


                }


            }
        }
    }
    void SetFadeinToFalse()
    {
        textAnimator.SetBool("fadein", false);

    }

    void SetFadeout()
    {
        textAnimator.SetBool("fadeout", false);
        village_title_text.SetActive(false);

    }
}
