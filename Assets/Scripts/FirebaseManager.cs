using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SocialPlatforms;
using System.Threading.Tasks;
using System.Linq;
using Firebase.Database;
using Firebase.Auth;
using TMPro;

public class FirebaseManager : MonoBehaviour
{
    public DatabaseReference dbreference;
    FirebaseAuth auth;
    FirebaseUser User;


    [Header("UserData")]
    public TMP_InputField usernameField;
    public TMP_Text coinsField;
    public TMP_Text diamondsField;
    public TMP_Text pillowsField;
    public TMP_Text totalWonGameCountField;
    public TMP_Text winRateField;
    public TMP_Text multiplayerCountField;
    public TMP_Text killCountField;
    public GameObject scoreElement;
    public Transform leaderboardContent;

    // Start is called before the first frame update
    void Start()
    {
        dbreference = FirebaseDatabase.DefaultInstance.RootReference;
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
       .RequestServerAuthCode(false /* Don't force refresh */)
       .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        PlayGamesSignIn();
    }

    public void PlayGamesSignIn()
    {
        Social.localUser.Authenticate((bool success) => {
            if (success)
            {
                string authCode = PlayGamesPlatform.Instance.GetServerAuthCode();
                PlayGamesAuthentication(authCode);
                UIManager.Instance.DebugText.text = UIManager.Instance.DebugText.text + "SignInSuccessfull   ";
            }
            else
            {
                UIManager.Instance.DebugText.text = UIManager.Instance.DebugText.text + "SignInNOTSuccessfull   ";
            }
        });
    }

    void PlayGamesAuthentication(string authCode)
    {
        auth = FirebaseAuth.DefaultInstance;
        Credential credential = PlayGamesAuthProvider.GetCredential(authCode);
        auth.SignInWithCredentialAsync(credential).ContinueWith(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled."); 
                UIManager.Instance.DebugText.text = UIManager.Instance.DebugText.text + "SignInWithCredentialAsync was canceled.   ";
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                UIManager.Instance.DebugText.text = UIManager.Instance.DebugText.text + "SignInWithCredentialAsync encountered an error: " + task.Exception+"   ";
                return;
            }

            User = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})",
                User.DisplayName, User.UserId); 
            UIManager.Instance.DebugText.text = UIManager.Instance.DebugText.text + "User signed in successfully: { 0} ({ 1})  "+
                User.DisplayName+ "   "+ User.UserId;

            StartCoroutine(LoadUserData());
            onClickLeaderboardButton();

        });
    }

    public void SaveDataToFirebase(string userName,int coins, int diamonds, int pillows, int totalGameWon, int winRate, int multiplayerGame, int killCount)
    {
        Debug.Log("on save button click");
        StartCoroutine(UpdateUsernameAuth(userName));
        StartCoroutine(UpdateUsernameDatabase(userName));
        StartCoroutine(UpdateCoins(coins));
        StartCoroutine(UpdateDiamonds(diamonds));
        StartCoroutine(UpdatePillows(pillows));
        StartCoroutine(UpdateTotalGameWon(totalGameWon));
        StartCoroutine(UpdateWinRate(winRate));
        StartCoroutine(UpdateMultiplayerGames(multiplayerGame));
        StartCoroutine(UpdateKills(killCount));

        Invoke("LoadDataAfterDelay", 5f);
    }

    void LoadDataAfterDelay()
    {
        StartCoroutine(LoadUserData());
    }

    public void onClickLeaderboardButton()
    {
        Debug.Log("on leaderboard button click");
        StartCoroutine(LoadScoreboardData());
    }

    private IEnumerator UpdateUsernameAuth(string _username)
    {
        //Create a user profile and set the username
        UserProfile profile = new UserProfile { DisplayName = _username };

        //Call the Firebase auth update user profile function passing the profile with the username
        var ProfileTask = User.UpdateUserProfileAsync(profile);
        //Wait until the task completes
        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

        if (ProfileTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
        }
        else
        {
            //Auth username is now updated
        }
    }


    private IEnumerator UpdateUsernameDatabase(string _username)
    {
        //Set the currently logged in user username in the database
        var DBTask = dbreference.Child("users").Child(User.UserId).Child("username").SetValueAsync(_username);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Database username is now updated
        }
    }

    private IEnumerator UpdateCoins(int _coins)
    {
        //Set the currently logged in user xp
        var DBTask = dbreference.Child("users").Child(User.UserId).Child("coins").SetValueAsync(_coins);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //coins is now updated
        }
    }

    private IEnumerator UpdateDiamonds(int _diamonds)
    {
        //Set the currently logged in user xp
        var DBTask = dbreference.Child("users").Child(User.UserId).Child("diamonds").SetValueAsync(_diamonds);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //diamonds is now updated
        }
    }

    private IEnumerator UpdatePillows(int _pillows)
    {
        //Set the currently logged in user xp
        var DBTask = dbreference.Child("users").Child(User.UserId).Child("pillows").SetValueAsync(_pillows);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //pillows is now updated
        }
    }

    private IEnumerator UpdateTotalGameWon(int _totalGameWon)
    {
        //Set the currently logged in user xp
        var DBTask = dbreference.Child("users").Child(User.UserId).Child("totalGameWon").SetValueAsync(_totalGameWon);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //totalGameWon is now updated
        }
    }

    private IEnumerator UpdateWinRate(int _winRate)
    {
        //Set the currently logged in user xp
        var DBTask = dbreference.Child("users").Child(User.UserId).Child("winRate").SetValueAsync(_winRate);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //winRate is now updated
        }
    }

    private IEnumerator UpdateMultiplayerGames(int _multiplayerGames)
    {
        //Set the currently logged in user xp
        var DBTask = dbreference.Child("users").Child(User.UserId).Child("multiplayerGames").SetValueAsync(_multiplayerGames);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //multiplayerGames is now updated
        }
    }

    private IEnumerator UpdateKills(int _kills)
    {
        //Set the currently logged in user kills
        var DBTask = dbreference.Child("users").Child(User.UserId).Child("kills").SetValueAsync(_kills);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Kills are now updated
        }
    }

    IEnumerator LoadUserData()
    {
        //Get the currently logged in user data
        var DBTask = dbreference.Child("users").Child(User.UserId).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            //No data exists yet
            coinsField.text = "0";
            diamondsField.text = "0";
            pillowsField.text = "0";
            totalWonGameCountField.text = "0";
            winRateField.text = "0";
            multiplayerCountField.text = "0";
            killCountField.text = "0";
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            coinsField.text = snapshot.Child("coins").Value.ToString();
            diamondsField.text = snapshot.Child("diamonds").Value.ToString();
            pillowsField.text = snapshot.Child("pillows").Value.ToString();
            totalWonGameCountField.text = snapshot.Child("totalGameWon").Value.ToString();
            winRateField.text = snapshot.Child("winRate").Value.ToString();
            multiplayerCountField.text = snapshot.Child("multiplayerGames").Value.ToString();
            killCountField.text = snapshot.Child("kills").Value.ToString();

        }
    }

    private IEnumerator LoadScoreboardData()
    {
        //Get all the users data ordered by kills amount
        var DBTask = dbreference.Child("users").OrderByChild("kills").GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            //Destroy any existing scoreboard elements
            foreach (Transform child in leaderboardContent.transform)
            {
                Destroy(child.gameObject);
            }

            int i = 0;

            //Loop through every users UID
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                string username = childSnapshot.Child("username").Value.ToString();
                int coins = int.Parse(childSnapshot.Child("coins").Value.ToString());
                int diamonds = int.Parse(childSnapshot.Child("diamonds").Value.ToString());
                i++;
                //Instantiate new scoreboard elements
                GameObject scoreboardElement = Instantiate(scoreElement, leaderboardContent);
                scoreboardElement.GetComponent<ScoreElement>().NewScoreElement(username,i, coins, diamonds);
            }

            //Go to scoareboard screen
            //Scoreboardpanel.SetActive(true);
            //Userdatapanel.SetActive(false);
        }
    }

}
