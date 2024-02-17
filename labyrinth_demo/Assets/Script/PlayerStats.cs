using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerStats : MonoBehaviour
{
    private  PlayerStats instance;
    private FirebaseFirestore db;
    public string playerNickname;

   
    public double playerHP;
    public double playerSpeed;
    public double playerDamage;
    public double playerArmor;
    public double playergold;

   
   

    public Button hpUpButton;
    public Button hpDownButton;
    public Button speedUpButton;
    public Button speedDownButton;

    private ListenerRegistration playerStatsListener;

   




    private void Start()
    {
       

        db = FirebaseFirestore.DefaultInstance;
        // Retrieve the player's nickname from the FirebaseLoginManager script
        playerNickname = FirebaseLoginManager.PlayerNickname;

        // Use the player's nickname to fetch player stats from the database
        FetchPlayerStatsFromDatabase(playerNickname);

        Debug.Log(playerNickname + " hi");
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


       

       


        hpUpButton.onClick.AddListener(UpHP);
        hpDownButton.onClick.AddListener(DownHP);
        speedUpButton.onClick.AddListener(UpSPEED);
        speedDownButton.onClick.AddListener(DownSPEED);

       
    }

    private void OnDestroy()
    {
        // Remove the real-time listener when the script or game object is destroyed
        if (playerStatsListener != null)
        {
            playerStatsListener.Stop();
        }
    }

   


    private void FetchPlayerStatsFromDatabase(string nickname)
    {

        if (db == null)
        {
            Debug.LogError("Firebase database not initialized.");
            return;
        }

        if (string.IsNullOrEmpty(nickname))
        {
            SceneManager.LoadScene("LoadingScene");
            return;
        }

        DocumentReference playerStatsDocRef = db.Collection(nickname).Document("playerstats");
        playerStatsDocRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                DocumentSnapshot snapshot = task.Result;
                if (snapshot.Exists)
                {
                    playerHP = snapshot.GetValue<double>("hp");
                    playerSpeed = snapshot.GetValue<double>("speed");
                    playerDamage = snapshot.GetValue<double>("damage");
                    playerArmor = snapshot.GetValue<double>("armor");
                    playergold = snapshot.GetValue<double>("gold");
                    

                    // Perform any other necessary actions with the retrieved player stats

                    //Debug.Log(playerHP);
                    //Debug.Log(playerSpeed);
                    //Debug.Log(playerDamage);
                    //Debug.Log(playergold);



                }
                else
                {
                    // The player stats document does not exist
                    //Debug.Log("Player stats document does not exist for " + nickname);
                }
            }
            else if (task.IsFaulted)
            {
                // An error occurred while fetching player stats
                //Debug.LogError("Error fetching player stats: " + task.Exception);
            }
        });
    }
    void UpHP()
    {
        playerHP = playerHP + 100; // Increase playerHP in memory

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
    void DownHP()
    {
        playerHP = playerHP - 0;

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
                Debug.Log("HP - 100!!");
            }
        });
    }
    void UpSPEED()
    {
        playerSpeed = playerSpeed + 0.1; // Increase playerHP in memory

        // Update the Firestore document with the new playerHP value
        DocumentReference docRef = db.Collection(playerNickname).Document("playerstats");
        docRef.UpdateAsync("speed", playerSpeed).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Fail SPEED: " + task.Exception);
            }
            else
            {
                Debug.Log("SPEED UP!!");
            }
        });
    }
    void DownSPEED()
    {
        playerSpeed = playerSpeed - 0.1; // Increase playerHP in memory

        // Update the Firestore document with the new playerHP value
        DocumentReference docRef = db.Collection(playerNickname).Document("playerstats");
        docRef.UpdateAsync("speed", playerSpeed).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Fail SPEED: " + task.Exception);
            }
            else
            {
                Debug.Log("SPEED DOWN!!");
            }
        });
    }

 

    









    // Other methods and code...
}