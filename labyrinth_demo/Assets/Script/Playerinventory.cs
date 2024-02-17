using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using Prime31;

public class PlayerInventory : MonoBehaviour
{
    FirebaseFirestore db;
    public string playerNickname;
    public string chestTag = "CHEST";

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        // Retrieve the player's nickname from the FirebaseLoginManager script
        playerNickname = FirebaseLoginManager.PlayerNickname;
        CharacterController2D characterController = GetComponent<CharacterController2D>();
        characterController.onControllerCollidedEvent += HandleControllerCollision;
    }

    void HandleControllerCollision(RaycastHit2D hit)
    {
        if (hit.collider != null && hit.collider.CompareTag(chestTag))
        {
            // Handle chest closing logic here
            Debug.Log("Chest closed!");
        }
    }





    void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag(chestTag) && Input.GetKey(KeyCode.F))
        {
            Debug.Log("Chest opened!");
            int randomItemNumber = Random.Range(1, 11); // Adjust the range as needed

            // Determine the item name and description based on the random item number
            string itemName = "";
            string itemDesc = "";
            string Tier = "";
            int itemDamage = 0;
            int itemArmor = 0;
            int itemHp = 0;
            int itemSpeed = 0;
            switch (randomItemNumber)
            {
                case 1:
                    itemName = "BasicSword";
                    itemDesc = "just basic, need more ?";
                    Tier = "common";
                    itemDamage = 10;
                    break;

                case 2:
                    itemName = "StoneRing";
                    itemDesc = "its cold..";
                    Tier = "common";
                    itemDamage = 5;
                    itemArmor = 5;
                    break;
                case 3:
                    itemName = "LeatherArmor";
                    itemDesc = "just basic, need more ?";
                    Tier = "common";
                    itemArmor = 10;
                    break;
                case 4:
                    itemName = "PlateArmor";
                    itemDesc = "hard as a rock!";
                    Tier = "rare";
                    itemArmor = 20;
                    break;
                case 5:
                    itemName = "Pickaxe";
                    itemDesc = "warning! this is good for dig your head";
                    Tier = "rare";
                    itemDamage = 20;
                    break;
                case 6:
                    itemName = "WoodenShield";
                    itemDesc = "basic shield";
                    Tier = "common";
                    itemArmor = 5;
                    break;
                case 7:
                    itemName = "HeartRing";
                    itemDesc = "very heart lol";
                    Tier = "rare";
                    itemDamage = 5;
                    itemArmor = 5;
                    itemHp = 20;
                   
                    break;
                case 8:
                    itemName = "MagicBook_black";
                    itemDesc = "it have really strong power...";
                    Tier = "unique";
                    itemDamage = 30;
                    break;
                case 9:
                    itemName = "CombatShoes";
                    itemDesc = "just shoes";
                    Tier = "rare";
                    itemArmor = 10;
                    itemSpeed = 1;
                    break;
                case 10:
                    itemName = "ShinyRedStaff";
                    itemDesc = "very powerful..";
                    Tier = "legendary";
                    itemDamage = 30;
                    itemHp = 50;
                    itemSpeed = 1;
                    break;
               

                default:
                    Debug.Log("error");
                    break;

                   
            }



                    // Add more conditions for other item numbers as needed

                    // Add the obtained item to the inventory with its details
            AddItemToInventory(randomItemNumber.ToString(), itemName, itemDesc, Tier, itemDamage,itemArmor,itemHp, itemSpeed, 1);

            Destroy(col.gameObject);
        }
    }

    void AddItemToInventory(string itemID, string itemName, string itemDesc, string Tier, int itemDamage,int itemArmor,int itemHp, int itemSpeed, int quantity)
    {
        DocumentReference docRef = db.Collection(playerNickname).Document("inventory");

        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                Debug.LogError("Failed to retrieve inventory snapshot: " + task.Exception);
                return;
            }

            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                Dictionary<string, object> inventoryData = snapshot.ToDictionary();
                if (inventoryData.ContainsKey(itemID))
                {
                    Dictionary<string, object> itemData = (Dictionary<string, object>)inventoryData[itemID];
                    int currentQuantity = itemData.ContainsKey("quantity") ? (int)itemData["quantity"] : 0;
                    int newQuantity = currentQuantity + quantity;

                    itemData["itemName"] = itemName;
                    itemData["tier"] = Tier;
                    itemData["desc"] = itemDesc;
                    itemData["itemDamage"] = itemDamage;
                    itemData["quantity"] = newQuantity;
                    itemData["itemArmor"] = itemArmor;
                    itemData["itemHp"] = itemHp;
                    itemData["itemSpeed"] = itemSpeed;
                    
                }
                else
                {
                    inventoryData[itemID] = new Dictionary<string, object>
                    {
                        {"itemName", itemName },
                        { "tier", Tier },
                        { "desc", itemDesc },
                        { "itemDamage", itemDamage },
                        { "quantity", quantity },
                        { "itemArmor", itemArmor },
                        { "itemHp", itemHp },
                        { "itemSpeed", itemSpeed }

                    };
                }

                docRef.SetAsync(inventoryData).ContinueWithOnMainThread(updateTask =>
                {
                    if (updateTask.Exception != null)
                    {
                        Debug.LogError("Failed to update inventory: " + updateTask.Exception);
                    }
                    else
                    {
                        Debug.Log("Item added to inventory: " + itemID);
                    }
                });
            }
            else
            {
                Dictionary<string, object> inventoryData = new Dictionary<string, object>
                {
                    {
                        itemID, new Dictionary<string, object>
                        {
                            {"itemName",itemName},
                            { "tier", Tier },
                            { "desc", itemDesc },
                            { "quantity", quantity },
                            { "itemDamage", itemDamage },
                            { "itemArmor", itemArmor },
                            { "itemHp", itemHp },
                            { "itemSpeed", itemSpeed }
                        }
                    }
                };

                docRef.SetAsync(inventoryData).ContinueWithOnMainThread(updateTask =>
                {
                    if (updateTask.Exception != null)
                    {
                        Debug.LogError("Failed to update inventory: " + updateTask.Exception);
                    }
                    else
                    {
                        Debug.Log("Item added to inventory: " + itemID);
                    }
                });
            }
        });
    }
}
