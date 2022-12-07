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
    public TMP_InputField xpField;
    public TMP_InputField killsField;
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
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        Firebase.Auth.Credential credential =
            Firebase.Auth.PlayGamesAuthProvider.GetCredential(authCode);
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
        });
    }

    public void SaveDataToFirebase()
    {
        Debug.Log("on save button click");
        StartCoroutine(UpdateUsernameAuth(usernameField.text));
        StartCoroutine(UpdateUsernameDatabase(usernameField.text));

        StartCoroutine(UpdateXp(int.Parse(xpField.text)));
        StartCoroutine(UpdateKills(int.Parse(killsField.text)));
    }

    public void ScoreboardButton()
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

    private IEnumerator UpdateXp(int _xp)
    {
        //Set the currently logged in user xp
        var DBTask = dbreference.Child("users").Child(User.UserId).Child("xp").SetValueAsync(_xp);

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else
        {
            //Xp is now updated
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

    private IEnumerator LoadUserData()
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
            xpField.text = "0";
            killsField.text = "0";
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            xpField.text = snapshot.Child("xp").Value.ToString();
            killsField.text = snapshot.Child("kills").Value.ToString();

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

            //Loop through every users UID
            foreach (DataSnapshot childSnapshot in snapshot.Children.Reverse<DataSnapshot>())
            {
                string username = childSnapshot.Child("username").Value.ToString();
                int kills = int.Parse(childSnapshot.Child("kills").Value.ToString());

                int xp = int.Parse(childSnapshot.Child("xp").Value.ToString());

                //Instantiate new scoreboard elements
                GameObject scoreboardElement = Instantiate(scoreElement, leaderboardContent);
                scoreboardElement.GetComponent<ScoreElement>().NewScoreElement(username, kills, xp);
            }

            //Go to scoareboard screen
            //Scoreboardpanel.SetActive(true);
            //Userdatapanel.SetActive(false);
        }
    }

}
