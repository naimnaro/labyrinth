using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class select_managers : MonoBehaviour
{
    public UnityEngine.UI.Button test_button;
    public UnityEngine.UI.Button dungeon1_button;

    private int state = 0;

    public GameObject test_option;
    public GameObject dungeon1_option;
    // Start is called before the first frame update
    void Start()
    {
        test_option.SetActive(false);
        dungeon1_option.SetActive(false);
        test_button.onClick.AddListener(test_click);
        dungeon1_button.onClick.AddListener(dungeon1_click);


       

    }

    private void dungeon1_click()
    {
        state = 1;
    }

    private void test_click()
    {
        state = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (state == 0){
            test_option.SetActive(true);         
        }
        else
        {
            test_option.SetActive(false);
        }
        if (state == 1)
        {
            dungeon1_option.SetActive(true);

        }
        else
        {
            dungeon1_option.SetActive(false);
        }
       
        
    }

    public void test_enter()
    {
        SceneManager.LoadScene("Dungeon1");
    }

    public void dungeon2_enter()
    {
        SceneManager.LoadScene("Dungeon2");
    }

    public void backtovillage()
    {
        SceneManager.LoadScene("village1");
    }


}


