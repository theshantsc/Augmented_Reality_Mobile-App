// Copyright 2016 Google Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WFirebaseARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using Firebase.Unity.Editor;
using Google;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class LoginHandler : MonoBehaviour {

      private GoogleSignInConfiguration configuration;


    public string webClientId = "1022328572535-8uanl1nmf2e3lcfkmd63mpnlek7p11lu.apps.googleusercontent.com";

 

  protected Firebase.Auth.FirebaseAuth auth;
  private Firebase.Auth.FirebaseAuth otherAuth;
  protected Dictionary<string, Firebase.Auth.FirebaseUser> userByAuth =
  new Dictionary<string, Firebase.Auth.FirebaseUser>();
  private string logText = "";
  public Text emailText;
  public Text passwordText;
  public Text userNameText;
  public Text errorMsg;
  protected string email = "";
  protected string password = "";
  public static string displayName = "";
  private bool fetchingToken = false;
  public static  Firebase.Auth.FirebaseUser loggedUser = null; 
  public static  int loggedUserCurrentLevel = 0;
  //public GameObject username;


    const int kMaxLogSize = 16382;
  Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;

   	private DatabaseReference playerDbRef;
    private DatabaseReference playerReadRef;


  // When the app starts, check to make sure that we have
  // the required dependencies to use Firebase, and if not,
  // add them if possible.
  public void Start() {

      errorMsg.text="";
      Debug.Log("Start");
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
      dependencyStatus = task.Result;
      if (dependencyStatus == Firebase.DependencyStatus.Available) {
        InitializeFirebase();
      } else {
        Debug.LogError(
          "Could not resolve all Firebase dependencies: " + dependencyStatus);
      }
    });
 
  }

      void Awake()
    {
        Debug.Log("Awake");
        configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestEmail = true, RequestIdToken = true };
       // CheckFirebaseDependencies();
     
    }


  // Handle initialization of the necessary firebase modules:
  void InitializeFirebase() {
    DebugLog("Setting up Firebase Auth Setup");
  
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
      auth.StateChanged += AuthStateChanged;
      auth.IdTokenChanged += IdTokenChanged;
     AuthStateChanged(this, null);
      DebugLog("Setting up AuthStateChanged Completed");

     FirebaseApp app = FirebaseApp.DefaultInstance;
    // NOTE: You'll need to replace this url with your Firebase App's database
    // path in order for the database connection to work correctly in editor.
   DebugLog("Setting up Firebase Datbase Setup");
    if (app.Options.DatabaseUrl != null) {
     app.SetEditorDatabaseUrl("https://softchasers-catch-me.firebaseio.com/");
    }app.SetEditorDatabaseUrl(app.Options.DatabaseUrl);
   
    // playerReadRef.Child("eUP6BxLhsbXptwjTXUVyeksZZuj1").Child("lastlogintimestamp").SetValueAsync ("test");
      DebugLog("Setting up Firebase Datbase setup Completed");

           
  }

  // Exit if escape (or back, on mobile) is pressed.
  public void Update() {
    if (Input.GetKeyDown(KeyCode.Escape)) {
      Application.Quit();
    }
        email = emailText.text;
        password = passwordText.text;
        displayName=userNameText.text;
  }

  void OnDestroy() {
    auth.StateChanged -= AuthStateChanged;
    auth.IdTokenChanged -= IdTokenChanged;
    auth = null;
  }

  // Output text to the debug log text field, as well as the console.
  public void DebugLog(string s) {
    Debug.Log(s);
    logText += s + "\n";

    while (logText.Length > kMaxLogSize) {
      int index = logText.IndexOf("\n");
      logText = logText.Substring(index + 1);
    }
  }

  // Display user information.
  void DisplayUserInfo(Firebase.Auth.IUserInfo userInfo, int indentLevel) {
    string indent = new String(' ', indentLevel * 2);
    var userProperties = new Dictionary<string, string> {
      {"Display Name", userInfo.DisplayName},
      {"Email", userInfo.Email},
      {"Photo URL", userInfo.PhotoUrl != null ? userInfo.PhotoUrl.ToString() : null},
      {"Provider ID", userInfo.ProviderId},
      {"User ID", userInfo.UserId}
    };

        foreach (var property in userProperties) {
      if (!String.IsNullOrEmpty(property.Value)) {
        DebugLog(String.Format("{0}{1}: {2}", indent, property.Key, property.Value));
      }
    }
  }

  // Display a more detailed view of a FirebaseUser.
  void DisplayDetailedUserInfo(Firebase.Auth.FirebaseUser user, int indentLevel) {
    DisplayUserInfo(user, indentLevel);
    // StartCoroutine(writePlayer(user));
    DebugLog("  Anonymous: " + user.IsAnonymous);
    DebugLog("  Email Verified: " + user.IsEmailVerified);
  /* var providerDataList = new List<Firebase.Auth.IUserInfo>(user.ProviderData);
    if (providerDataList.Count > 0) {
      DebugLog("  Provider Data:");
      foreach (var providerData in user.ProviderData) {
        DisplayUserInfo(providerData, indentLevel + 1);
      }
    }  */
  }

  // Track state changes of the auth object.
  void AuthStateChanged(object sender, System.EventArgs eventArgs) {
     DebugLog("AuthStateChanged ");
    Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;
    Firebase.Auth.FirebaseUser user = null;
    if (senderAuth != null) userByAuth.TryGetValue(senderAuth.App.Name, out user);
      DebugLog("AuthStateChanged senderAuth.CurrentUser " +senderAuth.CurrentUser);
    if (senderAuth == auth && senderAuth.CurrentUser != user) {
       DebugLog("AuthStateChanged equals! ");
      bool signedIn = user != senderAuth.CurrentUser && senderAuth.CurrentUser != null;
      if (!signedIn && user != null) {
        DebugLog("Signed out " + user.UserId);
                //user is logged out, load login screen 
                SceneManager.LoadSceneAsync("LoginScene");
      }
      user = senderAuth.CurrentUser;
      userByAuth[senderAuth.App.Name] = user;
      if (signedIn) {
        DebugLog(" AuthStateChanged Signed in " + user.UserId);
        // GetIntailDbValues(user.UserId);
        //displayName = user.DisplayName ?? "";
        loggedUser=user;
        //DisplayDetailedUserInfo(user, 1);
      }else {
         loggedUser=null;
        }
    }
  }

  // Track ID token changes.
  void IdTokenChanged(object sender, System.EventArgs eventArgs) {
    Firebase.Auth.FirebaseAuth senderAuth = sender as Firebase.Auth.FirebaseAuth;
    if (senderAuth == auth && senderAuth.CurrentUser != null && !fetchingToken) {
      senderAuth.CurrentUser.TokenAsync(false).ContinueWith(
        task => DebugLog(String.Format("Token[0:8] = {0}", task.Result.Substring(0, 8))));
    }
  }

  // Log the result of the specified task, returning true if the task
  // completed successfully, false otherwise.
  public bool LogTaskCompletion(Task task, string operation) {
    bool complete = false;
    if (task.IsCanceled) {
      DebugLog(operation + " canceled.");
       errorMsg.text=" Operation canceld ! :188 ";
    }
    else if (task.IsFaulted)
    {
        DebugLog(operation + " encounted an error.");
        foreach (Exception exception in task.Exception.Flatten().InnerExceptions) {

                string authErrorCode = "";
                Firebase.FirebaseException firebaseEx = exception as Firebase.FirebaseException;

            if (firebaseEx != null) {

                authErrorCode = String.Format("AuthError.{0}: ",
                ((Firebase.Auth.AuthError)firebaseEx.ErrorCode).ToString());
                  errorMsg.text=authErrorCode;
            }
        DebugLog(authErrorCode + exception.ToString());
          errorMsg.text=exception.ToString();
        }
    }
    else if (task.IsCompleted) {
      DebugLog(operation + " completed");
      complete = true;
    }
    return complete;
  }

   public void navigateRegister() {
      DebugLog(String.Format("Navigate the UserRegisterScene"));
       SceneManager.LoadScene("UserRegisterScene");
   }



   public void navigateToEmailLogin() {
      DebugLog(String.Format("Navigate the LoginScene"));
       SceneManager.LoadScene("LoginScene");
   }

  public void CreateUserAsync() {
    DebugLog(String.Format("Attempting to create user {0}...", email));

    // This passes the current displayName through to HandleCreateUserAsync
    // so that it can be passed to UpdateUserProfile().  displayName will be
    // reset by AuthStateChanged() when the new user is created and signed in.
    string newDisplayName = displayName;
    auth.CreateUserWithEmailAndPasswordAsync(email, password)
      .ContinueWith((task) => {
        return HandleCreateUserAsync(task, newDisplayName: newDisplayName);
      }).Unwrap();
  }

  Task HandleCreateUserAsync(Task<Firebase.Auth.FirebaseUser> authTask,
                             string newDisplayName = null) {
    if (LogTaskCompletion(authTask, "User Creation")) {
      if (auth.CurrentUser != null) {
        DebugLog(String.Format("User Info: {0}  {1}", auth.CurrentUser.Email,
                               auth.CurrentUser.ProviderId));
        return UpdateUserProfileAsync(newDisplayName: newDisplayName);
      }
    }
    // Nothing to update, so just return a completed Task.
    return Task.FromResult(0);
  }

  // Update the user's display name with the currently selected display name.
  public Task UpdateUserProfileAsync(string newDisplayName = null) {
    if (auth.CurrentUser == null) {
      DebugLog("Not signed in, unable to update user profile");
      return Task.FromResult(0);
    }
    displayName = newDisplayName ?? displayName;
    DebugLog("Updating user profile");
    return auth.CurrentUser.UpdateUserProfileAsync(new Firebase.Auth.UserProfile {
        DisplayName = displayName,
        PhotoUrl = auth.CurrentUser.PhotoUrl,
      }).ContinueWith(HandleUpdateUserProfile);
  }

  void HandleUpdateUserProfile(Task authTask) {
    if (LogTaskCompletion(authTask, "User profile")) {
      StartCoroutine(writePlayer(auth.CurrentUser));
     //writePlayer2(auth.CurrentUser);
      
      DisplayDetailedUserInfo(auth.CurrentUser, 1);
        //loggedUser=auth.CurrentUser;
      SceneManager.LoadSceneAsync("Menu");
     //SceneManager.LoadSceneAsync("scene_01");
    }
  }

  public void SigninAsync() {
    DebugLog(String.Format("Attempting to sign in as {0}...", email));
    auth.SignInWithEmailAndPasswordAsync(email, password)
      .ContinueWith(HandleSigninResult);
  }

  void HandleSigninResult(Task<Firebase.Auth.FirebaseUser> authTask) {
            if (authTask.IsCanceled) {
            Debug.LogError("SignInWithCredentialAsync was canceled.");
            errorMsg.text="Login is Canceled!";
            return;
          }
          if (authTask.IsFaulted) {
            Debug.LogError("SignInWithCredentialAsync encountered an error: " + authTask.Exception);
              errorMsg.text="Invalid Username or Password!";
            return;
          }
        LogTaskCompletion(authTask, "Sign-in");
        Firebase.Auth.FirebaseUser newUser = authTask.Result;
        Debug.LogFormat("Firebase user login successfully: {0} ({1})",newUser.DisplayName, newUser.UserId);
        StartCoroutine(updateLastLoginTime(newUser.UserId));
        PlayerPrefs.SetString("username", newUser.DisplayName);
        SceneManager.LoadSceneAsync("Menu");
        // SceneManager.LoadSceneAsync("scene_01");

    }

    public void ReloadUser() {
    if (auth.CurrentUser == null) {
      DebugLog("Not signed in, unable to reload user.");
      return;
    }
    DebugLog("Reload User Data");
    auth.CurrentUser.ReloadAsync().ContinueWith(HandleReloadUser);
  }

  void HandleReloadUser(Task authTask) {
    if (LogTaskCompletion(authTask, "Reload")) {
      DisplayDetailedUserInfo(auth.CurrentUser, 1);
    }
  }

  public void GetUserToken() {
    if (auth.CurrentUser == null) {
      DebugLog("Not signed in, unable to get token.");
      return;
    }
    DebugLog("Fetching user token");
    fetchingToken = true;
    auth.CurrentUser.TokenAsync(false).ContinueWith(HandleGetUserToken);
  }

  void HandleGetUserToken(Task<string> authTask) {
    fetchingToken = false;
    if (LogTaskCompletion(authTask, "User token fetch")) {
      DebugLog("Token = " + authTask.Result);
    }
  }

  void GetUserInfo() {
    if (auth.CurrentUser == null) {
      DebugLog("Not signed in, unable to get info.");
    } else {
      DebugLog("Current user info:");
      DisplayDetailedUserInfo(auth.CurrentUser, 1);
      
    }
  }

  public void SignOut() {
    DebugLog("Signing out.");
    auth.SignOut();
  }

  // Show the providers for the current email address.
  public void DisplayProvidersForEmail() {
    auth.FetchProvidersForEmailAsync(email).ContinueWith((authTask) => {
        if (LogTaskCompletion(authTask, "Fetch Providers")) {
          DebugLog(String.Format("Email Providers for '{0}':", email));
          foreach (string provider in authTask.Result) {
            DebugLog(provider);
          }
        }
      });
  }


  public class Player
{
    public string playername;
    public string email;
    public int achievedlevel = 1;
    public string userId;
     // System.Uri photo_url = user.PhotoUrl;
        public string profilepicuri;

    public Player(string playername, string email, string userId,string profilepicuri) {
        this.playername = playername;
        this.email = email;
        this.userId = userId;
        this.profilepicuri = profilepicuri;
    }
} 

// writing player to the database
private IEnumerator writePlayer(Firebase.Auth.IUserInfo userInfo) {

  string userId =userInfo.UserId;
  string playername =userInfo.DisplayName;
  string email=userInfo.Email;
  string photo_url= WWW.UnEscapeURL("https://firebasestorage.googleapis.com/v0/b/softchasers-catch-me.appspot.com/o/avata.png?alt=media&token=33f08ed1-3154-49f8-892d-dfb1da8ccdce");

    //message.text = "writePlayer";
    DebugLog(String.Format("Wirting Player at Register User Id '{0}':", userId));
    DebugLog(String.Format("playername Providers for '{0}':", playername));
    DebugLog(String.Format("Email Providers for '{0}':", email));
    Player player = new Player(playername, email,userId,photo_url);
    string json = JsonUtility.ToJson(player);

        PlayerPrefs.SetString("urlInfo", photo_url.ToString());
        PlayerPrefs.SetString("username", playername);
    

    Debug.Log("original");
    Debug.Log(json);

    json = json.Substring(0, json.Length-1);
    Debug.Log("cutted");
    Debug.Log(json);

     string timestampAdd = @" , ""createdtimestamp"": {"".sv"" : ""timestamp""} } ";
    Debug.Log("adder");
    Debug.Log(timestampAdd);
    json = json + timestampAdd;
    Debug.Log("added");
    Debug.Log(json);
    playerDbRef = FirebaseDatabase.DefaultInstance.RootReference;
    DebugLog(String.Format("playerDbRef {0}...", playerDbRef));
    playerDbRef.Child("players").Child(userId).SetRawJsonValueAsync(json);

    yield return null;
}


 public void SaveNewUserGoogleAuth() {

    string userId =loggedUser.UserId;
    string playername =loggedUser.DisplayName;
    string email=loggedUser.Email;
    string photo_url = loggedUser.PhotoUrl.ToString();

    //message.text = "writePlayer";
     AddToInformation("SaveNewUserGoogleAuthr User Id '{0}': = " + loggedUser.UserId);

    Player player = new Player(playername, email,userId,photo_url);
    string json = JsonUtility.ToJson(player);
    PlayerPrefs.SetString("urlInfo", photo_url.ToString());
    PlayerPrefs.SetString("username", playername);


    Debug.Log("original");
    Debug.Log(json);

    json = json.Substring(0, json.Length-1);
    Debug.Log("cutted");
    Debug.Log(json);

     string timestampAdd = @" , ""createdtimestamp"": {"".sv"" : ""timestamp""} } ";
    Debug.Log("adder");
    Debug.Log(timestampAdd);
    json = json + timestampAdd;
    Debug.Log("added");
    Debug.Log(json);
      playerDbRef = FirebaseDatabase.DefaultInstance.RootReference;
     DebugLog(String.Format("playerDbRef {0}...", playerDbRef));
     AddToInformation("SaveNewUserGoogleAuth End ");
    playerDbRef.Child("players").Child(userId).SetRawJsonValueAsync(json);

    SceneManager.LoadSceneAsync("Menu");
     //SceneManager.LoadSceneAsync("scene_01");
  
  }


private IEnumerator updateLastLoginTime(String userId) {
  //string the_JSON_string="{'.sv' : 'timestamp'}";
  //var test="test";
 // var result = JSON.Parse(the_JSON_string);
  DebugLog(String.Format("updateLastLoginTime {0}...", userId));
  ///  Debug.Log(the_JSON_string);
     playerReadRef=FirebaseDatabase.DefaultInstance.GetReference("players");
    DebugLog(String.Format("playerReadRef {0}...", playerReadRef));
   // playerReadRef.Child(userId).Child("lastlogintimestamp").SetRawJsonValueAsync(the_JSON_string);
    playerReadRef.Child(userId).Child("lastlogin").SetValueAsync (System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss"));
    yield return null;
}



    // called first
    void OnEnable()
    {
        Debug.Log("OnEnable called");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

 // called second
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
         Debug.Log("OnSceneLoaded login controller: " + scene.name);
       // Debug.Log(mode);
        if(scene.name.Contains("scene_01")){
          Debug.Log("OnSceneLoaded:  scene_01  loaded" + scene.name);
          Debug.Log("Logged In User Id :" + loggedUser.UserId);
         //GetIntailDbValues(loggedUser.UserId);

          

        }

         if(scene.name.Contains("GoogleAuthUserNameScene")){
            AddToInformation("Firebase Google user  New User not found in  DB = " + loggedUser.UserId);
            AddToInformation("Firebase Google user  New User not found in  DB = " + loggedUser.Email);
         }


             if(scene.name.Contains("GoogleAuthScene")){
              playerReadRef=FirebaseDatabase.DefaultInstance.GetReference("players");
            /*  playerReadRef.Child("69qdRPc4tOgbfzHcmrNeWICmzdI3").GetValueAsync().ContinueWith(task2 => {
              if (task2.IsFaulted) {
                         Debug.Log("User Exist error task.IsFaulted");
                          AddToInformation("User Exist error task.IsFaulted");
                    }
                    else if (task2.IsCompleted) {
                         AddToInformation("User Exist checkTask Completed get User Detail ");
                       DataSnapshot snapshot = task2.Result;
                      if(snapshot.Value!=null){
                          AddToInformation("Firebase Google user  Database Entry Exist = " );
                         SceneManager.LoadSceneAsync("scene_01");
                      }
                      else {
                            AddToInformation("Firebase Google user  New User not found in  DB = ");
                          
                         SceneManager.LoadSceneAsync("GoogleAuthUserNameScene");
                           
                      }  
                    }else {
                        AddToInformation("Else condtion ");
                         Debug.Log("Else condtion");
                    }
          }); */
             }
        

          
    }
    

  public void BackToLogin () {
        SceneManager.LoadScene("LoginScene");

    }


  public void BackToGoogleSignIn () {
        SceneManager.LoadScene("GoogleAuthScene");

    }

public void ContinueWithGoogle()
{
    if (Application.internetReachability == NetworkReachability.NotReachable)
    {
        // Notify player about connectivity 
        Debug.Log("No Internet");
    }
    else
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }
}

// G+ auth success function
internal void OnAuthenticationFinished(Task<GoogleSignInUser> task)
{
    if (task.IsFaulted)
    {
        using (IEnumerator<System.Exception> enumerator =
                task.Exception.InnerExceptions.GetEnumerator())
        {
            if (enumerator.MoveNext())
            {
                GoogleSignIn.SignInException error =
                        (GoogleSignIn.SignInException)enumerator.Current;
                Debug.Log("Got Error: " + error.Status + " " + error.Message);
            }
            else
            {
                Debug.Log("Got Unexpected Exception?!?" + task.Exception);
            }
        }
    }
    else if (task.IsCanceled)
    {
        Debug.Log("Canceled");
    }
    else
    {
        Debug.Log("Welcome: " + task.Result.UserId + "!");
          SignInWithGoogleOnFirebase(task.Result.IdToken);
        //ContinueWithUserDetails(task.Result.DisplayName, task.Result.ImageUrl.ToString(), task.Result.Email);
        
    }
}



    private void SignInWithGoogleOnFirebase(string idToken)
    {
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
 
        auth.SignInWithCredentialAsync(credential).ContinueWith(task =>
        {
            AggregateException ex = task.Exception;
            if (ex != null)
            {
               AddToInformation("\nError code = ");
               // if (ex.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0))
               //     AddToInformation("\nError code = " + inner.ErrorCode + " Message = " + inner.Message);
            }
            else
            {
                AddToInformation("Sign In Successful Google.");
                // SceneManager.LoadSceneAsync("scene_01");
                Firebase.Auth.FirebaseUser newUser = task.Result;
                 AddToInformation("Sign In Successful. User Id "+newUser.UserId);
                playerReadRef=FirebaseDatabase.DefaultInstance.GetReference("players");
                playerReadRef.Child(newUser.UserId).GetValueAsync().ContinueWith(task2 => {
              if (task2.IsFaulted) {
                        // Handle the error...
                         Debug.Log("User Exist error task.IsFaulted");
                          AddToInformation("User Exist error task.IsFaulted");
                            SceneManager.LoadSceneAsync("GoogleAuthUserNameScene");
                    }
                    else if (task2.IsCompleted) {
                         AddToInformation("User Exist checkTask Completed get User Detail ");
                       DataSnapshot snapshot = task2.Result;
                      if(snapshot.Value!=null){
                          AddToInformation("Firebase Google user  Database Entry Exist = " + newUser.UserId);
                           StartCoroutine(updateLastLoginTime(newUser.UserId));
                          SceneManager.LoadSceneAsync("Menu");
                        // SceneManager.LoadSceneAsync("scene_01");
                      }
                      else {
                         AddToInformation("Firebase Google user  New User not found in  DB = " + newUser.UserId);
                          
                         SceneManager.LoadSceneAsync("GoogleAuthUserNameScene");
                           
                      }  
                    }else {
                        AddToInformation("Else condtion ");
                         Debug.Log("Else condtion");
                    }
          });   
            }
        });
    }

  private void AddToInformation(string str) { errorMsg.text += "\n" + str; }
  
}


    
          /* playerReadRef.Child(newUser.UserId).GetValueAsync().ContinueWith(task => {
              if (task.IsFaulted) {
                        // Handle the error...
                         Debug.Log("User Exist error task.IsFaulted");
                    }
                    else if (task.IsCompleted) {
                      Debug.Log("User Exist checkTask Completed get User Detail :");
                       DataSnapshot snapshot = task.Result;
                      if(snapshot.Value!=null){
                          AddToInformation("Firebase Google user  Database Entry Exist = " + newUser.UserId);
                          
                           if (LogTaskCompletion(authTask, "Sign-in")) {
                              SceneManager.LoadSceneAsync("scene_01");
                           }

                      }
                      else {
                            AddToInformation("Firebase Google user  New User not found in  DB = " + newUser.UserId);
                              if (LogTaskCompletion(authTask, "New User not found in DB")) {
                                 SceneManager.LoadSceneAsync("GoogleAuthUserNameScene");
                           }
                      }  
                    }else {
                         Debug.Log("Else condtion");
                    }
          });   */ 