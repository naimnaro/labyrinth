using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Firestore;
using Firebase.Extensions;

public class PlayerStats_UI : MonoBehaviour
{
    FirebaseFirestore db;
    public Text hpText;
    public Text speedText;
    public Text damageText;
    public Text armorText;
    public Text goldText;
    // Start is called before the first frame update
    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;

        DocumentReference docRef = db.Collection("player").Document("playerstats");
        docRef.GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            DocumentSnapshot snapshot = task.Result;
            if (snapshot.Exists)
            {
                double hp = snapshot.GetValue<double>("hp");
                double speed = snapshot.GetValue<double>("speed");
                double damage = snapshot.GetValue<double>("damage");
                double gold = snapshot.GetValue<double>("gold");
                double armor = snapshot.GetValue<double>("armor");

                hpText.text = "Hp : " + hp.ToString();
                speedText.text = "Speed : " + speed.ToString();
                damageText.text = "Damage : " + damage.ToString();
                goldText.text = "gold : " + gold.ToString();
                armorText.text = "armor : " + armor.ToString();

            }
            else
            {
                Debug.Log("Player stats document does not exist.");
            }
        });
    }
   
}
