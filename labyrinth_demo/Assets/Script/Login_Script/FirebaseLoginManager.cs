 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using Firebase.Extensions;
using Firebase;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using Firebase.Firestore;

using System.Threading.Tasks;
using System.Linq;

public class FirebaseLoginManager : MonoBehaviour
{
    private FirebaseAuth auth;
    private FirebaseFirestore db;

    public GameObject emailInputField_;
    public GameObject passwordInputField_;

    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;

    public GameObject Popup_login_panel;
    public GameObject Popup_signup_panel;
    public GameObject Popup_logout_panel;

    public GameObject Login_btn;
    public GameObject Logout_btn;
    public GameObject signup_btn;

    public GameObject SignupErrorPopup;
    public GameObject LoginErrorPopup;

    public GameObject NickNamePopup;
    public InputField NickName_inputfield;

    public GameObject charac_with_nickname;
    public Text getnickname;

    public GameObject characterPrefab;
    public static string PlayerNickname { get; private set; } // Static variable to store the player's nickname
    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        db = FirebaseFirestore.DefaultInstance;

    }

    public async void SignUp()
    {
        passwordInputField.contentType = TMP_InputField.ContentType.Password;
        passwordInputField.ForceLabelUpdate();

        try
        {
            var newUser = await auth.CreateUserWithEmailAndPasswordAsync(emailInputField.text, passwordInputField.text);
            Popup_signup_panel.SetActive(true);
            Debug.Log("회원가입 성공");
        }
        catch (FirebaseException e)
        {
            SignupErrorPopup.SetActive(true);
            Debug.Log("회원가입 실패: " + e.Message);
        }
        catch (Exception e)
        {
            Debug.Log("An error occurred during signup: " + e.Message);
            SignupErrorPopup.SetActive(true);
        }
    }

    public async void GameGo()
    {

        DocumentReference userDocRef = db.Collection("users").Document(auth.CurrentUser.Email);
        DocumentSnapshot snapshot = await userDocRef.GetSnapshotAsync();
        if (snapshot.Exists)
        {
            if (snapshot.ContainsField("Nickname"))
            {
                var user = await auth.SignInWithEmailAndPasswordAsync(emailInputField.text, passwordInputField.text);
                string nickname = snapshot.GetValue<string>("Nickname");

                PlayerNickname = nickname;



                // Check if playerstats and inventory documents exist for the nickname
                DocumentReference playerStatsDocRef = db.Collection(nickname).Document("playerstats");
                DocumentReference inventoryDocRef = db.Collection(nickname).Document("inventory");
                DocumentReference equippedUIDocRef = db.Collection(nickname).Document("equippedUI");

                DocumentSnapshot playerStatsSnapshot = await playerStatsDocRef.GetSnapshotAsync();
                DocumentSnapshot inventorySnapshot = await inventoryDocRef.GetSnapshotAsync();

                if (!playerStatsSnapshot.Exists)
                {
                    // Playerstats document doesn't exist, create it
                    CreatePlayerStats(nickname);
                }

                if (!inventorySnapshot.Exists)
                {
                    // Inventory document doesn't exist, create it
                    await CreateInventory(nickname);
                }
               

                SceneManager.LoadScene("LoadingScene");

            }
        }          
    }
    public async void Login()
    {
        

        try
        {
            var user = await auth.SignInWithEmailAndPasswordAsync(emailInputField.text, passwordInputField.text);

            Debug.Log("Login!");

            DocumentReference userDocRef = db.Collection("users").Document(auth.CurrentUser.Email);
            DocumentSnapshot snapshot = await userDocRef.GetSnapshotAsync();

            if (snapshot.Exists)
            {
                if (snapshot.ContainsField("Nickname"))
                {
                    // A nickname already exists, skip showing the nickname setting box
                    // Perform any other necessary actions here
                    Popup_login_panel.SetActive(true);
                    Login_btn.SetActive(false);
                    signup_btn.SetActive(false);
                    Logout_btn.SetActive(true);

                    emailInputField_.SetActive(false);
                    passwordInputField_.SetActive(false);

                    string nickname = snapshot.GetValue<string>("Nickname");

                    getnickname.text = nickname;
                    
                   

                    // Check if playerstats and inventory documents exist for the nickname
                    DocumentReference playerStatsDocRef = db.Collection(nickname).Document("playerstats");
                    DocumentReference inventoryDocRef = db.Collection(nickname).Document("inventory");

                    DocumentSnapshot playerStatsSnapshot = await playerStatsDocRef.GetSnapshotAsync();
                    DocumentSnapshot inventorySnapshot = await inventoryDocRef.GetSnapshotAsync();

                    if (!playerStatsSnapshot.Exists)
                    {
                        // Playerstats document doesn't exist, create it
                        CreatePlayerStats(nickname);
                    }

                    if (!inventorySnapshot.Exists)
                    {
                        // Inventory document doesn't exist, create it
                        await CreateInventory(nickname);
                    }

                    PlayerNickname = snapshot.GetValue<string>("Nickname");


                }
                else
                {
                    // A nickname doesn't exist, show the nickname setting box
                    NickNamePopup.SetActive(true);
                }
            }
            else
            {
                // User document doesn't exist, show the nickname setting box
                Debug.Log("login done! but User document not found set nickname now");
                NickNamePopup.SetActive(true);
            }


            
        }
        catch (FirebaseException e)
        {
            LoginErrorPopup.SetActive(true);
            Debug.Log("login fail : " + e.Message);
        }
        catch (Exception e)
        {
            Debug.Log("An error occurred during login: " + e.Message);
            LoginErrorPopup.SetActive(true);
        }
    }

    public async void SaveNickname()
    {
        string email = auth.CurrentUser.Email;
        string nickname = NickName_inputfield.text;

        if (!string.IsNullOrEmpty(nickname))
        {
            try
            {
                // Create a data object to update the "Nickname" field
                Dictionary<string, object> updateData = new Dictionary<string, object>
                {
                    { "Nickname", nickname }
                };

                // Update the document with the new nickname
                await db.Collection("users").Document(email).SetAsync(updateData, SetOptions.MergeAll);
                getnickname.text = nickname;

                // Check if playerstats and inventory documents exist for the nickname
                DocumentReference playerStatsDocRef = db.Collection(nickname).Document("playerstats");
                DocumentReference inventoryDocRef = db.Collection(nickname).Document("inventory");

                DocumentSnapshot playerStatsSnapshot = await playerStatsDocRef.GetSnapshotAsync();
                DocumentSnapshot inventorySnapshot = await inventoryDocRef.GetSnapshotAsync();

                if (!playerStatsSnapshot.Exists)
                {
                    // Playerstats document doesn't exist, create it
                    CreatePlayerStats(nickname);
                }

                if (!inventorySnapshot.Exists)
                {
                    // Inventory document doesn't exist, create it
                   
                }

                // Start the game
                Popup_login_panel.SetActive(true);
                Login_btn.SetActive(false);
                signup_btn.SetActive(false);
                Logout_btn.SetActive(true);

                emailInputField_.SetActive(false);
                passwordInputField_.SetActive(false);
            }
            catch (Exception e)
            {
                Debug.Log("An error occurred while saving the nickname: " + e.Message);
            }
        }
        else
        {
            Debug.Log("Nickname cannot be empty");
        }

        NickNamePopup.SetActive(false);
    }

    private async void CreatePlayerStats(string nickname)
    {
        Dictionary<string, object> playerStatsData = new Dictionary<string, object>
        {
            { "hp", 100 },
            { "speed", 3 },
            { "damage", 10 },
            { "armor", 0 },
            { "gold", 10000 },

        };

        await db.Collection(nickname).Document("playerstats").SetAsync(playerStatsData);
        Debug.Log("Playerstats created for " + nickname);
    }

    private async Task CreateInventory(string nickname)
    {
        // Create an empty dictionary for the inventory data
        Dictionary<string, object> inventoryData = new Dictionary<string, object>();
        // Set the inventory data to the Firestore document
        await db.Collection(nickname).Document("inventory").SetAsync(inventoryData);
        Debug.Log("Empty inventory created for " + nickname);

        Dictionary<string, object> equippedUIData = new Dictionary<string, object>();
        await db.Collection(nickname).Document("equippedUI").SetAsync(equippedUIData);
    }

    public void Logout()
    {
        auth.SignOut();
        Popup_logout_panel.SetActive(true);
        Login_btn.SetActive(true);
        signup_btn.SetActive(true);
        Logout_btn.SetActive(false);

        emailInputField_.SetActive(true);
        passwordInputField_.SetActive(true);
        charac_with_nickname.SetActive(false);

        Debug.Log("로그아웃");
    }

    void Refresh()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Login_Done_Ok_btn_OFF()
    {
        Popup_login_panel.SetActive(false);
        charac_with_nickname.SetActive(true);
    }

    public void signup_Ok_btn_OFF()
    {
        Popup_signup_panel.SetActive(false);
    }

    public void Logout_Ok_btn_OFF()
    {
        Popup_logout_panel.SetActive(false);
    }

    public void Signup_error_OK()
    {
        SignupErrorPopup.SetActive(false);
        Refresh();
    }

    public void Login_error_OK()
    {
        LoginErrorPopup.SetActive(false);
        Refresh();
    }
  
    
}
