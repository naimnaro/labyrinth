using Prime31;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercontroller_dungeon : MonoBehaviour
{
    private CharacterController2D characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController2D>();
        characterController.onControllerCollidedEvent += OnControllerCollided;
    }

    private void OnControllerCollided(RaycastHit2D hit)
    {
        if (hit.collider.isTrigger && IsChildWithTag(hit.collider.gameObject, "playerweapon"))
        {
            // Handle the collision with the player's weapon (e.g., deal damage)
            attack(hit.collider.gameObject);
        }




    }
   

    private void attack(GameObject gameObject)
    {
        Debug.Log("Player attack enemy!");

    }

    private bool IsChildWithTag(GameObject obj, string tag)
    {
        foreach (Transform child in obj.transform)
        {
            if (child.CompareTag(tag))
            {
                return true;
            }
        }
        return false;
    }



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
