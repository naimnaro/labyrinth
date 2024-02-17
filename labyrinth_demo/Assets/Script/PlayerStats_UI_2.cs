using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Extensions;
using Firebase.Firestore;
using UnityEngine.SceneManagement;

public class PlayerStats_UI_2 : MonoBehaviour
{
    private FirebaseFirestore db;
    public GameObject playerObject;
    private PlayerStats playerStatsScript;
    
    public TextMesh nicknameTextMesh;

    public Text Nickname;
    public Text hpText;
    public Text speedText;
    public Text damageText;
    public Text armorText;
    public Text goldText;
    public Text hpText2;

    private PlayerStats_UI_2 instance;
    // Start is called before the first frame update

   
    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        playerStatsScript = playerObject.GetComponent<PlayerStats>();


        
    }

    // Update is called once per frame
    void Update()
    {
        Nickname.text = playerStatsScript.playerNickname.ToString();
        nicknameTextMesh.text = playerStatsScript.playerNickname.ToString();
        hpText.text = "Health: " + playerStatsScript.playerHP.ToString();
        speedText.text = "Speed: " + playerStatsScript.playerSpeed.ToString("F2");
        damageText.text = "Damage: " + playerStatsScript.playerDamage.ToString();
        armorText.text = "Armor: " + playerStatsScript.playerArmor.ToString();
        goldText.text = "Gold: " + playerStatsScript.playergold.ToString();
        hpText2.text = "Health: " + playerStatsScript.playerHP.ToString();
    }
}
