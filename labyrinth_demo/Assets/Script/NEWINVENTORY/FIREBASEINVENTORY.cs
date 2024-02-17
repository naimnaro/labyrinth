using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Firestore;
using Firebase.Extensions;
using System;


public class FIREBASEINVENTORY : MonoBehaviour
{


    FirebaseFirestore db;




    public GameObject inventorypanel;
    public GameObject equipmentOptionsPanel;

    

    public Button equipButton;
    public Button unequipButton;

    

    public double playerHP;
    public double playerSpeed;
    public double playerDamage;
    public double playerArmor;
    public double playergold;



    bool activeinventory = false;
    public string playerNickname;
    public Transform buttonContainer; // Reference to the container where buttons will be created
    public GameObject inventoryButtonPrefab; // Reference to the button prefab

    private List<InventoryItem> inventory = new List<InventoryItem>(); // Your inventory data structure
    private List<InventoryItem> equippedItems = new List<InventoryItem>();

    private int maxEquippedItems = 4; // Set the maximum number of equipped items

    float initialYPosition = 0f;
    private ListenerRegistration playerStatsListener;

    private GameObject equippedItemsPanel;
    private GameObject equippedItemPrefab;

    // Start is called before the first frame update
    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        // Retrieve the player's nickname from wherever you store it
        playerNickname = FirebaseLoginManager.PlayerNickname;

        FetchPlayerInventoryFromFirestore(playerNickname);


        // Set up a Firestore listener to update in real-time
        ListenForInventoryChanges(playerNickname);
        FetchEquippedItemsFromFirestore(playerNickname);

        DocumentReference playerStatsDocRef = db.Collection(playerNickname).Document("playerstats");
        playerStatsListener = playerStatsDocRef.Listen(snapshot =>
        {
            if (snapshot.Exists)
            {
                playerHP = snapshot.GetValue<double>("hp");
                playerSpeed = snapshot.GetValue<double>("speed");
                playerDamage = snapshot.GetValue<double>("damage");
                playerArmor = snapshot.GetValue<double>("armor");
                playergold = snapshot.GetValue<double>("gold");

                // Handle the updated player stats in your game
                //Debug.Log("HP updated to: " + playerHP);
                //Debug.Log("SPEED updated to: " + playerSpeed);
                //Debug.Log("DAMAGE updated to: " + playerDamage);
                //Debug.Log("GOLD updated to: " + playergold);
            }
        });

        inventorypanel.SetActive(activeinventory);

       
        equipmentOptionsPanel.SetActive(false); // Initially, hide the equipment options panel
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }


    }

   

    private void ToggleInventory()
    {
        activeinventory = !activeinventory; // Invert the current state

        // Set the inventory GameObject's active state based on the boolean variable
        inventorypanel.SetActive(activeinventory);


    }

    public void OnItemClick(InventoryItem item)
    {
        Debug.Log($"Item clicked: {item.name}");

        ShowEquipmentOptions(item);
    }

    private void EquipItem(InventoryItem item)
    {
        // Check if the item is already equipped
        if (!equippedItems.Contains(item))
        {
            // Check if there is room to equip more items
            if (equippedItems.Count < maxEquippedItems)
            {
                // If there's room, add the item to the equipped list
                equippedItems.Add(item);

                // Update the Firestore document with the new equipped items
                UpdateEquipUIDocument(equippedItems);
            }
            else
            {
                // If the maximum limit is reached, handle it as needed (e.g., show a message)
                Debug.Log("Cannot equip more than 4 items!");
                return; // Exit the method to prevent further execution
            }

            // Update player stats based on the equipped item
           
        }
        UpdatePlayerStats(item);

        // Update UI for equipped items

    }
    private void UpdatePlayerStats(InventoryItem item)
    {
        // Modify player stats based on the equipped item
        playerArmor += item.itemArmor;
        playerDamage += item.itemDamage;
        
        playerHP += item.itemHp;
        playerSpeed += item.itemSpeed;

        // Update the Firestore document with the new player stats
        DocumentReference docRef = db.Collection(playerNickname).Document("playerstats");
        var updates = new Dictionary<string, object>
    {
        { "damage", playerDamage },
        { "armor", playerArmor },
        { "hp",    playerHP },
        { "speed" ,playerSpeed }
    };

        docRef.UpdateAsync(updates).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to update player stats: " + task.Exception);
            }
            else
            {
                Debug.Log($"Player stats updated - Damage: {playerDamage}");
            }
        });
    }

    public void UnequipItem(InventoryItem item)
    {
        equipmentOptionsPanel.SetActive(false);
        // Check if the item is equipped
        if (equippedItems.Contains(item))
        {
            Debug.Log($"Unequipping item: {item.name}");

            // If equipped, remove the item from the equipped list
            equippedItems.Remove(item);

            // Update player stats based on the remaining equipped items

            // Update the Firestore document with the new equipped items
            UpdateEquipUIDocument(equippedItems);

            // Update the UI for equipped items
            UpdateEquippedItemsUI(equippedItemsPanel);

            Debug.Log("UnequipItem method executed successfully!");
        }
        else
        {
            Debug.LogWarning($"Item {item.name} is not currently equipped!");
            Debug.Log("Equipped items in the list:");
            foreach (var equippedItem in equippedItems)
            {
                Debug.Log($"{equippedItem.name} - {equippedItem.tier}");
            }
        }
        UpdatePlayerStats_unequip(item);


    }

    private void UpdatePlayerStats_unequip(InventoryItem item)
    {

        playerDamage -= item.itemDamage;       
        playerArmor -= item.itemArmor;
        playerHP -= item.itemHp;
        playerSpeed -= item.itemSpeed;

        // Update the Firestore document with the new player stats
        DocumentReference docRef = db.Collection(playerNickname).Document("playerstats");
        Dictionary<string, object> updates = new Dictionary<string, object>
    {
        { "damage", playerDamage },
        { "armor", playerArmor },
        { "hp", playerHP },
        { "speed", playerSpeed }
        // Add other fields as needed
    };

        docRef.UpdateAsync(updates).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to update player stats: " + task.Exception);
            }
            else
            {
                Debug.Log($"Player stats updated - Damage: {playerDamage}, Armor: {playerArmor}, HP: {playerHP}, Speed: {playerSpeed}");
            }
        });
    }


    private void UpdateEquippedItemsUI(GameObject equippedItemsPanel)
    {

        // Check if equippedItemsPanel is null
        if (equippedItemsPanel == null)
        {
            Debug.Log("EquippedItemsPanel is null!");
            return;
        }
        // Assuming you have a panel or some UI element to display equipped items
        // Clear the existing UI elements
        foreach (Transform child in equippedItemsPanel.transform)
        {
            Destroy(child.gameObject);
        }

        float buttonHeight = equippedItemPrefab.GetComponent<RectTransform>().rect.height;
        float margin = 5f; // You can adjust this value to set the margin between items
        float totalHeight = 0f;

        // Iterate through equipped items and create a UI element for each item
        foreach (var item in equippedItems)
        {
            GameObject equippedItemGO = Instantiate(equippedItemPrefab, equippedItemsPanel.transform);
            Text[] equippedItemTexts = equippedItemGO.GetComponentsInChildren<Text>();

            // Assuming the first Text component is for the name and the second for the tier
            equippedItemTexts[0].text = $"Name: {item.name}";
            equippedItemTexts[1].text = $"Tier: {item.tier}";

            // Add an Image component to display the item icon
            Image itemImage = equippedItemGO.transform.Find("ItemImage").GetComponent<Image>();
            if (item.itemImage != null)
            {
                itemImage.sprite = item.itemImage;
            }
            else
            {
                Debug.LogError($"Item {item.name} has a null itemImage!");
            }

            // Set the anchored position with margin to create a list-like layout
            RectTransform buttonRect = equippedItemGO.GetComponent<RectTransform>();
            buttonRect.anchoredPosition = new Vector2(0f, -totalHeight - margin);

            totalHeight += buttonHeight + margin;
        }

        // Show the equipped items panel
        equippedItemsPanel.SetActive(true);
    }

    private void UpdateEquipUIDocument(List<InventoryItem> equippedItems)
    {
        DocumentReference equipUIDocRef = db.Collection(playerNickname).Document("equippedUI");
        Dictionary<string, object> equippedUIData = new Dictionary<string, object>();

        foreach (var item in equippedItems)
        {
            string itemID = $"{item.name}";
            equippedUIData[itemID] = new
            {
                itemName = item.name,
                tier = item.tier,
                desc = item.desc,
                quantity = item.quantity,
                itemDamage = item.itemDamage,
                itemArmor = item.itemArmor,              
                itemHp = item.itemHp,
                itemSpeed = item.itemSpeed
            };
        }

        equipUIDocRef.SetAsync(equippedUIData).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to update equipped items: " + task.Exception);
            }
            else
            {
                Debug.Log("Equipped items updated in Firestore");
            }
        });

    }

    private void ShowEquipmentOptions(InventoryItem item)
    {
        // Display a UI element with equipment options for the clicked item
        // This could be a dropdown, a separate panel, etc.
        equipmentOptionsPanel.SetActive(true);

        // Remove existing listeners to avoid multiple triggers
        equipButton.onClick.RemoveAllListeners();
        unequipButton.onClick.RemoveAllListeners();

        // Add new listeners
        equipButton.onClick.AddListener(() => OnEquipmentSelected(item));
        unequipButton.onClick.AddListener(() => UnequipItem(item));
    }

    public void OnEquipmentSelected(InventoryItem equipmentOption)
    {
        // Equip the selected item
        EquipItem(equipmentOption);

        // Update UI for equipped items
       

        // Hide the equipment options panel after selecting an option
        equipmentOptionsPanel.SetActive(false);
    }

    private void FetchEquippedItemsFromFirestore(string nickname)
    {
        DocumentReference equipUIDocRef = db.Collection(playerNickname).Document("equippedUI");

        equipUIDocRef.GetSnapshotAsync().ContinueWithOnMainThread(snapshotTask =>
        {
            if (snapshotTask.IsFaulted)
            {
                Debug.LogError("Failed to fetch equipped UI document: " + snapshotTask.Exception);
                return;
            }

            DocumentSnapshot snapshot = snapshotTask.Result;
            if (snapshot.Exists)
            {
                Dictionary<string, object> equippedUIData = snapshot.ToDictionary();

                Debug.Log("Equipped UI document on game start:");

                foreach (var entry in equippedUIData)
                {
                    Debug.Log($"{entry.Key}: {entry.Value}");
                }

                // Clear the existing equipped items list
                equippedItems.Clear();

                // Parse the snapshot and update the equipped items list
                foreach (var entry in equippedUIData)
                {
                    string itemID = entry.Key;
                    Dictionary<string, object> itemData = (Dictionary<string, object>)entry.Value;

                    string itemName = (string)itemData["itemName"];
                    string itemTier = (string)itemData["tier"];
                    string itemDesc = (string)itemData["desc"];
                    int itemQuantity = (int)(long)itemData["quantity"];
                    int itemDamage = itemData.ContainsKey("itemDamage") ? (int)(long)itemData["itemDamage"] : 0;
                    int itemArmor = itemData.ContainsKey("itemArmor") ? (int)(long)itemData["itemArmor"] : 0;
                    int itemHp = itemData.ContainsKey("itemHp") ? (int)(long)itemData["itemHp"] : 0;
                    int itemSpeed = itemData.ContainsKey("itemSpeed") ? (int)(long)itemData["itemSpeed"] : 0;

                    InventoryItem equippedItem = new InventoryItem
                    {
                        name = itemName,
                        tier = itemTier,
                        desc = itemDesc,
                        quantity = itemQuantity,
                        itemDamage = itemDamage,
                        itemArmor = itemArmor,
                        itemHp = itemHp,
                        itemSpeed = itemSpeed
                    };

                    equippedItems.Add(equippedItem);
                }

                // Update the UI for equipped items
                UpdateEquippedItemsUI(equippedItemsPanel);
            }
            else
            {
                Debug.Log("Equipped UI document does not exist on game start.");
            }
        });
    }



    private void FetchPlayerInventoryFromFirestore(string nickname)
    {
        // Your existing code to fetch the initial inventory
        DocumentReference docRef = db.Collection(nickname).Document("inventory");

        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.LogError("Failed to fetch inventory: " + task.Exception);
                return;
            }

            DocumentSnapshot snapshot = task.Result;

            if (snapshot.Exists)
            {
                Dictionary<string, object> inventoryData = snapshot.ToDictionary();
                inventory.Clear(); // Clear the existing inventory list

                foreach (var entry in inventoryData)
                {
                    string itemID = entry.Key;
                    Dictionary<string, object> itemData = (Dictionary<string, object>)entry.Value;

                    string itemName = (string)itemData["itemName"];
                    string itemTier = (string)itemData["tier"];
                    string itemDesc = (string)itemData["desc"];
                    int itemQuantity = (int)(long)itemData["quantity"];
                    int itemNumber = int.Parse(itemID); // Parse item number
                    int itemDamage = itemData.ContainsKey("itemDamage") ? (int)(long)itemData["itemDamage"] : 0;
                    int itemArmor = itemData.ContainsKey("itemArmor") ? (int)(long)itemData["itemArmor"] : 0;
                    int itemHp = itemData.ContainsKey("itemHp") ? (int)(long)itemData["itemHp"] : 0;
                    int itemSpeed = itemData.ContainsKey("itemSpeed") ? (int)(long)itemData["itemSpeed"] : 0;


                    string imagePath = $"ItemImages/Item{itemNumber}";
                    Sprite itemImage = Resources.Load<Sprite>(imagePath);

                    if (itemImage != null)
                    {
                        //Debug.Log($"Loaded image for item {itemNumber}: {imagePath}");
                    }
                    else
                    {
                        //Debug.LogError($"Failed to load image for item {itemNumber}: {imagePath}");
                    }

                    // Create an InventoryItem and add it to the inventory list
                    InventoryItem newItem = new InventoryItem
                    {
                        name = itemName,
                        tier = itemTier,
                        desc = itemDesc,
                        quantity = itemQuantity,
                        itemImage = itemImage, // Assign the loaded image
                        itemDamage = itemDamage,
                        itemArmor = itemArmor,
                        itemHp = itemHp,
                        itemSpeed = itemSpeed
                    };

                    inventory.Add(newItem);
                }

                // Update the UI with the fetched inventory data
                UpdateInventoryUI();
            }
        });
    }

    private void ListenForInventoryChanges(string nickname)
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

                    string itemName = (string)itemData["itemName"];
                    string itemTier = (string)itemData["tier"];
                    string itemDesc = (string)itemData["desc"];
                    int itemQuantity = (int)(long)itemData["quantity"];
                    int itemNumber = int.Parse(itemID); // Parse item number
                    int itemDamage = itemData.ContainsKey("itemDamage") ? (int)(long)itemData["itemDamage"] : 0;
                    int itemArmor = itemData.ContainsKey("itemArmor") ? (int)(long)itemData["itemArmor"] : 0;
                    int itemHp = itemData.ContainsKey("itemHp") ? (int)(long)itemData["itemHp"] : 0;
                    int itemSpeed = itemData.ContainsKey("itemSpeed") ? (int)(long)itemData["itemSpeed"] : 0;



                    Sprite itemImage = Resources.Load<Sprite>($"ItemImages/Item{itemNumber}");

                    // Create an InventoryItem and add it to the inventory list
                    InventoryItem newItem = new InventoryItem
                    {
                        name = itemName,
                        tier = itemTier,
                        desc = itemDesc,
                        quantity = itemQuantity,
                        itemImage = itemImage,
                        itemDamage = itemDamage,
                        itemArmor = itemArmor,
                        itemHp = itemHp,
                        itemSpeed = itemSpeed
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
        // Clear the existing buttons
        foreach (Transform child in buttonContainer)
        {
            Destroy(child.gameObject);
        }

        float totalHeight = 0f;
        // Iterate through the inventory and create a button for each item
        foreach (var item in inventory)
        {
            GameObject buttonGO = Instantiate(inventoryButtonPrefab, buttonContainer);
            Button button = buttonGO.GetComponent<Button>();
            Text buttonText = buttonGO.GetComponentInChildren<Text>();
            //Image itemImage = buttonGO.GetComponentInChildren<Image>(); // Get the Image component
            Image itemImage = buttonGO.transform.Find("ItemImage").GetComponent<Image>(); // Find the ItemImage component

            // Set the anchored position to create a list-like layout
            RectTransform buttonRect = buttonGO.GetComponent<RectTransform>();
            buttonRect.anchoredPosition = new Vector2(0f, -totalHeight);

            if (item.itemImage != null)
            {
                itemImage.sprite = item.itemImage; // Assign the loaded sprite to the Image component
            }



            buttonText.text = $"Name: {item.name}\nTier: {item.tier}";

            totalHeight += buttonRect.rect.height;
            initialYPosition -= buttonRect.rect.height;

            // Add an onClick event to the button (you can handle what happens when the button is clicked here)
            button.onClick.AddListener(() =>
            {
                OnItemClick(item);
            });
        }
    }

    [System.Serializable]
    public class InventoryItem
    {
        public string name;
        public string tier;
        public string desc;
        public int quantity;
        public Sprite itemImage;
        public int itemDamage;
        public int itemArmor;
        public int itemHp;
        public int itemSpeed;

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            InventoryItem otherItem = (InventoryItem)obj;
            return name == otherItem.name && tier == otherItem.tier;
        }

        public override int GetHashCode()
        {
            return (name + tier).GetHashCode();
        }
    }
}
