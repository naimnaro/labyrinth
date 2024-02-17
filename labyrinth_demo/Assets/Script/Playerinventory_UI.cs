using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Firestore;
using Firebase.Extensions;

public class Playerinventory_UI : MonoBehaviour
{
    FirebaseFirestore db;
    public string playerNickname;
    public Text inventoryText; // Reference to the UI Text element

    private List<InventoryItem> inventory = new List<InventoryItem>(); // Your inventory data structure

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        // Retrieve the player's nickname from the FirebaseLoginManager script
        playerNickname = FirebaseLoginManager.PlayerNickname;

        // Call a method to fetch and update the inventory data
        FetchPlayerInventoryFromFirestore(playerNickname);
    }

    private void FetchPlayerInventoryFromFirestore(string nickname)
    {
        DocumentReference docRef = db.Collection(nickname).Document("inventory");
        docRef.Listen(snapshot =>
        {
            if (snapshot.Exists)
            {
                Dictionary<string, object> inventoryData = snapshot.ToDictionary();
                inventory.Clear(); // Clear the existing inventory list

                foreach (var entry in inventoryData)
                {
                    string itemID = entry.Key;
                    Dictionary<string, object> itemData = (Dictionary<string, object>)entry.Value;

                    string itemName = itemData.ContainsKey("itemName") ? (string)itemData["itemName"] : "";
                    string itemTier = itemData.ContainsKey("tier") ? (string)itemData["tier"] : "";
                    string itemDesc = itemData.ContainsKey("desc") ? (string)itemData["desc"] : "";
                    int itemQuantity = itemData.ContainsKey("quantity") ? (int)(long)itemData["quantity"] : 0;

                    // Create an InventoryItem and add it to the inventory list
                    InventoryItem newItem = new InventoryItem
                    {
                        name = itemName,
                        tier = itemTier,
                        desc = itemDesc,
                        quantity = itemQuantity
                    };

                    inventory.Add(newItem);
                }

                // Update the UI with the fetched inventory data
                UpdateInventoryUI();
            }
        });

    }

    private void UpdateInventoryUI()
    {
        // Clear the existing text
        inventoryText.text = "";

        // Iterate through the inventory and add each item's information to the UI Text
        foreach (var item in inventory)
        {
            inventoryText.text += $"{item.name}\n";
            inventoryText.text += $"tier: {item.tier}\n";
            inventoryText.text += "-----------------\n"; // Separate each item
        }
    }
}

// Define a class to represent an inventory item
[System.Serializable]
public class InventoryItem
{
    public string name;
    public string tier;
    public string desc;
    public int quantity;
}
