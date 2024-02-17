using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class damaged_manager : MonoBehaviour
{
    private PlayerStats instance;
    private FirebaseFirestore db;
    public string playerNickname;


    public double playerHP;
    public double playerSpeed;
    public double playerDamage;
    public double playerArmor;
    public double playergold;

    private ListenerRegistration playerStatsListener;

    public GameObject[] playerParts;

    public int blinkCount = 5;

    // Duration of each blink
    public float blinkDuration = 0.5f;

    // Alpha value when player blinks
    public float blinkAlpha = 0f;

    private Color[] originalColors;








    // Start is called before the first frame update
    void Start()
    {

        db = FirebaseFirestore.DefaultInstance;
        // Retrieve the player's nickname from wherever you store it
        playerNickname = FirebaseLoginManager.PlayerNickname;

        DocumentReference docRef = db.Collection(playerNickname).Document("playerstats");
        playerStatsListener = docRef.Listen(snapshot =>
        {
            if (snapshot.Exists)
            {
                playerHP = snapshot.GetValue<double>("hp");
                playerSpeed = snapshot.GetValue<double>("speed");
                playerDamage = snapshot.GetValue<double>("damage");
                playerArmor = snapshot.GetValue<double>("armor");
                playergold = snapshot.GetValue<double>("gold");

                // Handle the updated playerHP value in your game
                //Debug.Log("HP updated to: " + playerHP);
                // Debug.Log("SPEED updated to: " + playerSpeed);
                //Debug.Log("DAMAGE updated to: " + playerDamage);
                //Debug.Log("GOLD updated to: " + playergold);
            }
        });

        originalColors = new Color[playerParts.Length];

        for (int i = 0; i < playerParts.Length; i++)
        {
            SpriteRenderer spriteRenderer = playerParts[i].GetComponent<SpriteRenderer>();

            if (spriteRenderer != null)
            {
                originalColors[i] = spriteRenderer.color;
            }
            else
            {
                Debug.LogError($"PlayerPart {i} has no SpriteRenderer component!");
            }
        }



    }

    
    




    private void OnTriggerStay2D(Collider2D collision)
    {

        // Check if the collision is with an enemy
        if (collision.transform.CompareTag("enemy"))
        {

            // Handle the collision with the enemy (e.g., deal damage)
            stay_damaged(collision.transform.gameObject);

        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        // Check if the collision is with an enemy
        if (collision.transform.CompareTag("enemy"))
        {

            // Handle the collision with the enemy (e.g., deal damage)
            Enter_damaged(collision.transform.gameObject);
            

        }

    }
    private System.Collections.IEnumerator HitEffectCoroutine()
    {
        for (int blink = 0; blink < blinkCount; blink++)
        {
            // Toggle visibility of each part simultaneously
            for (int i = 0; i < playerParts.Length; i++)
            {
                if (originalColors[i] != null)
                {
                    Color currentColor = originalColors[i];
                    currentColor.a = (blink % 2 == 0) ? blinkAlpha : originalColors[i].a;

                    // Update the player part's color
                    playerParts[i].GetComponent<SpriteRenderer>().color = currentColor;
                }
            }

            // Wait for the specified blink duration
            yield return new WaitForSeconds(blinkDuration);
        }

        // Reset each player part to the original color
        for (int i = 0; i < playerParts.Length; i++)
        {
            if (originalColors[i] != null)
            {
                playerParts[i].GetComponent<SpriteRenderer>().color = originalColors[i];
            }
        }

        Debug.Log("Hit effect ended.");
    }

    private void stay_damaged(GameObject gameObject)
    {
        float damageReductionPercentage = (float)(playerArmor * 0.1f); // Assuming 10 armor results in a 10% reduction
        float damage = Mathf.Max(1f, 1f - (1f * damageReductionPercentage));
        if (playerArmor < 1)
        {
            playerHP -= damage;

        }
        

        // Ensure playerHP doesn't go below 0
        playerHP = Mathf.Max(0f, (float)playerHP);

        // Update the Firestore document with the new playerHP value
        DocumentReference docRef = db.Collection(playerNickname).Document("playerstats");
        docRef.UpdateAsync("hp", playerHP).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Fail HP: " + task.Exception);
            }
            else
            {
                Debug.Log("HP + 100!!");
            }
        });
    }

    private void Enter_damaged(GameObject gameObject)
    {
        float damageReductionPercentage = (float)(playerArmor * 0.1f); // Assuming 10 armor results in a 10% reduction
        float damage = Mathf.Max(1f, 10f - (10f * damageReductionPercentage));
        playerHP -= damage;

        // Ensure playerHP doesn't go below 0
        playerHP = Mathf.Max(0f, (float)playerHP);

        // Update the Firestore document with the new playerHP value
        DocumentReference docRef = db.Collection(playerNickname).Document("playerstats");
        docRef.UpdateAsync("hp", playerHP).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Fail HP: " + task.Exception);
            }
            else
            {
                Debug.Log("HP + 100!!");
            }
        });
        StartCoroutine(HitEffectCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHP == 0f)
        {
            Die();
        }
        



    }

    private void Die()
    {
        playerHP = playerHP + 100; // Increase playerHP in memory

        // Update the Firestore document with the new playerHP value
        DocumentReference docRef = db.Collection(playerNickname).Document("playerstats");
        docRef.UpdateAsync("hp", playerHP).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                //Debug.LogError("Fail HP: " + task.Exception);
            }
            else
            {
                //Debug.Log("HP + 100!!");
            }
        });
        SceneManager.LoadScene("dead");
    }
}
