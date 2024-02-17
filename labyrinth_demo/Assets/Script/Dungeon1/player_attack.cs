using Prime31;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class player_attack : MonoBehaviour
{
    private GameObject enemy;
    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        // Check if the collision is with an enemy
        if (collision.transform.CompareTag("enemy"))
        {

            // Handle the collision with the enemy (e.g., deal damage)
            attack(collision.transform.gameObject);
        }

    }




    [SerializeField] private float knockbackForce = 12f;
    [SerializeField] private Color damageColor = new Color(1f, 0.5f, 0.5f);
    [SerializeField] private float damageDuration = 0.5f;
    private void attack(GameObject gameObject)
    {
        if (gameObject == null)
        {
            return;
        }


        // Get the SpriteRenderer component of the enemy
        SpriteRenderer enemyRenderer = gameObject.GetComponent<SpriteRenderer>();

        // Check if the enemy has a SpriteRenderer
        if (enemyRenderer != null)
        {
            // Change the color of the enemy temporarily
            enemyRenderer.color = damageColor;

            // Calculate the knockback direction (opposite to the player's weapon)
            Vector2 knockbackDirection = (gameObject.transform.position - transform.position).normalized;

            // Get the Rigidbody2D component of the enemy
            Rigidbody2D enemyRb = gameObject.GetComponent<Rigidbody2D>();

            // Apply the knockback force only if the enemy has a Rigidbody2D
            if (enemyRb != null)
            {
                enemyRb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
            }

            // Restore the original color after a delay
            StartCoroutine(RestoreColor(enemyRenderer, damageDuration));

            
        }
    }

    private IEnumerator RestoreColor(SpriteRenderer renderer, float delay)
    {
        if (renderer == null || renderer.gameObject == null)
        {
            yield break; // Exit the coroutine if the renderer is null or the object is destroyed
        }

        yield return new WaitForSeconds(delay);

        // Check again after the delay
        if (renderer == null || renderer.gameObject == null)
        {
            yield break; // Exit the coroutine if the renderer is null or the object is destroyed
        }

        renderer.color = Color.white; // Assuming the original color i
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
