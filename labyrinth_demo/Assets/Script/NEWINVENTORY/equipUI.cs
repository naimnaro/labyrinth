using UnityEngine;
using UnityEngine.UI;
using Firebase.Firestore;
using System.Collections.Generic;

public class equipUI : MonoBehaviour
{

    FirebaseFirestore db;
    public string playerNickname;

    public GameObject equippedItemsPanel;
    public GameObject equippedItemPrefab;
    // Start is called before the first frame update

    private List<EquippedItem> equippedItems = new List<EquippedItem>();


    public double playerHP;
    public double playerSpeed;
    public double playerDamage;
    public double playerArmor;


    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        playerNickname = FirebaseLoginManager.PlayerNickname;

        // Set up a Firestore listener to update in real-time
        ListenForEquippedItemsChanges(playerNickname);

    }
    private void ListenForEquippedItemsChanges(string nickname)
    {
        DocumentReference docRef = db.Collection(nickname).Document("equippedUI");

        docRef.Listen(snapshot =>
        {
            if (snapshot.Exists)
            {
                Dictionary<string, object> equippedUIData = snapshot.ToDictionary();
                equippedItems.Clear(); // Clear the existing equipped items list

                foreach (var entry in equippedUIData)
                {
                    string itemID = entry.Key;
                    Dictionary<string, object> itemData = (Dictionary<string, object>)entry.Value;

                    // Check if the expected keys exist in the dictionary
                    if (itemData.ContainsKey("itemName") &&
                        itemData.ContainsKey("tier") &&
                        itemData.ContainsKey("desc") &&
                        itemData.ContainsKey("quantity") &&
                        itemData.ContainsKey("itemDamage") &&
                        itemData.ContainsKey("itemArmor") &&
                        itemData.ContainsKey("itemHp") &&
                        itemData.ContainsKey("itemSpeed"))
                    {
                        string itemName = (string)itemData["itemName"];
                        string itemTier = (string)itemData["tier"];
                        string itemDesc = (string)itemData["desc"];
                        int itemQuantity = (int)(long)itemData["quantity"];
                        int itemDamage = (int)(long)itemData["itemDamage"];
                        int itemArmor = (int)(long)itemData["itemArmor"];
                        int itemHp = (int)(long)itemData["itemHp"];
                        int itemSpeed = (int)(long)itemData["itemSpeed"];




                        string imagePath = $"ItemImages/{itemName}";
                        Sprite itemImage = Resources.Load<Sprite>(imagePath);

                        if (itemImage != null)
                        {
                            Debug.Log($"Loaded sprite: {itemImage.name}");
                        }
                        else
                        {
                            Debug.LogError($"Failed to load sprite at path: {imagePath}");
                        }

                        // Create an EquippedItem and add it to the equipped items list
                        EquippedItem newItem = new EquippedItem
                        {
                            itemName = itemName,
                            itemTier = itemTier,
                            itemDesc = itemDesc,
                            itemQuantity = itemQuantity,
                            itemDamage = itemDamage,
                            itemImage = itemImage,
                            itemArmor = itemArmor,
                            itemHp = itemHp,
                            itemSpeed = itemSpeed

                        };

                        equippedItems.Add(newItem);
                    }
                    else
                    {
                        Debug.LogError($"Incomplete data for equipped item with ID {itemID}");
                    }
                }

                // Update the UI with the fetched equipped items data
                UpdateEquippedItemsUI();
            }
        });
    }
    private void UpdateEquippedItemsUI()
    {
        // Clear the existing equipped item buttons
        foreach (Transform child in equippedItemsPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Check if equippedItems is null
        if (equippedItems == null)
        {
            Debug.LogError("equippedItems is null");
            return;
        }

        float buttonHeight = equippedItemPrefab.GetComponent<RectTransform>().rect.height;
        float margin = 5f; // You can adjust this value to set the margin between items
        float totalHeight = 0f;

        // Iterate through equipped items and create a button for each item
        foreach (var item in equippedItems)
        {
            GameObject equippedItemGO = Instantiate(equippedItemPrefab, equippedItemsPanel.transform);
            Text[] equippedItemTexts = equippedItemGO.GetComponentsInChildren<Text>();

            // Assuming the first Text component is for the name and the second for the tier
            equippedItemTexts[0].text = $"{item.itemName}";
            equippedItemTexts[1].text = $"{item.itemTier}";

            // Add an Image component to display the item icon
            Image itemImage = equippedItemGO.transform.Find("ItemImage").GetComponent<Image>();

            // Set the anchored position with margin to create a list-like layout
            RectTransform buttonRect = equippedItemGO.GetComponent<RectTransform>();
            buttonRect.anchoredPosition = new Vector2(0f, -totalHeight - margin);

            // Set the sprite to the Image component
            itemImage.sprite = item.itemImage;

            totalHeight += buttonHeight + margin;
        }

        // Show the equipped items panel
        equippedItemsPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    [System.Serializable]
    public class EquippedItem
    {
        public string itemName;
        public string itemTier;
        public string itemDesc;
        public int itemQuantity;
        public int itemDamage;
        public Sprite itemImage;
        public int itemArmor;
        public int itemHp;
        public int itemSpeed;
    }
}
