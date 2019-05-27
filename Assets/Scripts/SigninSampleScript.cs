// <copyright file="SigninSampleScript.cs" company="Google Inc.">
// Copyright (C) 2017 Google Inc. All Rights Reserved.
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//  http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations

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

  public class SigninSampleScript : MonoBehaviour {

    public Text statusText;
    protected Firebase.Auth.FirebaseAuth auth;
    public string webClientId = "1022328572535-8uanl1nmf2e3lcfkmd63mpnlek7p11lu.apps.googleusercontent.com";
   private DatabaseReference playerReadRef;
	

    private GoogleSignInConfiguration configuration;

    // Defer the configuration creation until Awake so the web Client ID
    // Can be set via the property inspector in the Editor.
    void Awake() {
        configuration = new GoogleSignInConfiguration { WebClientId = webClientId, RequestEmail = true, RequestIdToken = true };
    }

    public void OnSignIn() {
      GoogleSignIn.Configuration = configuration;
      GoogleSignIn.Configuration.UseGameSignIn = false;
      GoogleSignIn.Configuration.RequestIdToken = true;
      AddStatusText("Calling SignIn");

      GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
        OnAuthenticationFinished);
    }

    public void OnSignOut() {
      AddStatusText("Calling SignOut");
      GoogleSignIn.DefaultInstance.SignOut();
    }

    public void OnDisconnect() {
      AddStatusText("Calling Disconnect");
      GoogleSignIn.DefaultInstance.Disconnect();
    }

    private void OnAuthenticationFinished(Task<GoogleSignInUser> task) {
       TaskCompletionSource<FirebaseUser> signInCompleted = new TaskCompletionSource<FirebaseUser> ();
      if (task.IsFaulted) {
        using (IEnumerator<System.Exception> enumerator =
                task.Exception.InnerExceptions.GetEnumerator()) {
          if (enumerator.MoveNext()) {
            GoogleSignIn.SignInException error =
                    (GoogleSignIn.SignInException)enumerator.Current;
            AddStatusText("Got Error: " + error.Status + " " + error.Message);
          } else {
            AddStatusText("Got Unexpected Exception?!?" + task.Exception);
          }
        }
      } else if(task.IsCanceled) {
        AddStatusText("Canceled");
      } else  {
        AddStatusText("Welcome: " + task.Result.DisplayName + "!");
        //  AddStatusText("Email = " + task.Result.Email);
         
        string token=task.Result.IdToken;
        AddStatusText("Google ID Token = " + token);
       // SignInWithGoogleOnFirebase(token);
         //SceneManager.LoadSceneAsync("scene_01");

     Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential (((Task<GoogleSignInUser>)task).Result.IdToken, null);
        auth.SignInWithCredentialAsync (credential).ContinueWith (authTask => {
          if (authTask.IsCanceled) {
             AddToInformation("Login is Canceled!:");
            signInCompleted.SetCanceled();
          } else if (authTask.IsFaulted) {
               AddToInformation("Expception!");
            signInCompleted.SetException(authTask.Exception);
          } else {
            signInCompleted.SetResult(((Task<FirebaseUser>)authTask).Result);
             SceneManager.LoadSceneAsync("scene_01");
          }
        }); 


         
      }
    }

    
    private void SignInWithGoogleOnFirebase(string idToken)
    {
         AddToInformation("SignInWithGoogleOnFirebase!"+idToken);
        Credential credential = GoogleAuthProvider.GetCredential(idToken, null);
         AddToInformation("SignInWithGoogleOnFirebase credential:"+credential);

         
      auth.SignInWithCredentialAsync(credential).ContinueWith(authTask =>
        {
             if (authTask.IsCanceled) {
            Debug.LogError("HandleSigninFirebaseGoogleResult was canceled.");
             AddToInformation("Login is Canceled!:");
            return;
          }
          if (authTask.IsFaulted) {
            Debug.LogError("HandleSigninFirebaseGoogleResult encountered an error: " + authTask.Exception);
             AddToInformation("Invalid Username or Password!");
            return;
          }
           SceneManager.LoadSceneAsync("scene_01");

        });
        AddToInformation("The End!");

       /*  auth.SignInWithCredentialAsync(credential).ContinueWith(authTask =>
        {
            AggregateException ex = authTask.Exception;
            if (ex != null)
            {
                  AddToInformation("Error!");
               // if (ex.InnerExceptions[0] is FirebaseException inner && (inner.ErrorCode != 0))
                 //   AddToInformation("\nError code = " + inner.ErrorCode + " Message = " + inner.Message);
            }
            else
            {
                  Firebase.Auth.FirebaseUser newUser = authTask.Result;
                  statusText.text = "Google Sign In Successful.";
                 AddToInformation("Google Sign In Successful.");
                    // StartCoroutine(updateLastLoginTime(newUser.UserId));
                //SceneManager.LoadSceneAsync("Menu");
                  playerReadRef=FirebaseDatabase.DefaultInstance.GetReference("players");

                  playerReadRef.Child(newUser.UserId).GetValueAsync().ContinueWith(task => {
              if (task.IsFaulted) {
                        // Handle the error...
                         AddToInformation("User Exist error task.IsFaulted.");
                         Debug.Log("User Exist error task.IsFaulted");
                    }
                    else if (task.IsCompleted) {
                      Debug.Log("User Exist checkTask Completed get User Detail :");

                        AddToInformation("User Exist checkTask Completed get User Detail.");
                       DataSnapshot snapshot = task.Result;
                      if(snapshot.Value!=null){
                          AddToInformation("Firebase Google user  Database Entry Exist = " + newUser.UserId);
                      
                              SceneManager.LoadSceneAsync("scene_01");
                       

                      }
                      else {
                            AddToInformation("Firebase Google user  New User not found in  DB = " + newUser.UserId);
                             
                           SceneManager.LoadSceneAsync("GoogleAuthUserNameScene");
                          
                      }  
                    }else {
                        statusText.text = "Else Condtion";
                         Debug.Log("Else condtion");
                    }
          });
            }


        });   */
    }

      
    
    public void OnSignInSilently() {
      GoogleSignIn.Configuration = configuration;
      GoogleSignIn.Configuration.UseGameSignIn = false;
      GoogleSignIn.Configuration.RequestIdToken = true;
      AddStatusText("Calling SignIn Silently");

      GoogleSignIn.DefaultInstance.SignInSilently()
            .ContinueWith(OnAuthenticationFinished);
    }


    public void OnGamesSignIn() {
      GoogleSignIn.Configuration = configuration;
      GoogleSignIn.Configuration.UseGameSignIn = true;
      GoogleSignIn.Configuration.RequestIdToken = false;

      AddStatusText("Calling Games SignIn");

      GoogleSignIn.DefaultInstance.SignIn().ContinueWith(
        OnAuthenticationFinished);
    }

     private void AddToInformation(string str) { statusText.text += "\n" + str; }   

    private List<string> messages = new List<string>();
    void AddStatusText(string text) {
      if (messages.Count == 5) {
        messages.RemoveAt(0);
      }
      messages.Add(text);
      string txt = "";
      foreach (string s in messages) {
        txt += "\n" + s;
      }
      statusText.text = txt;
    }
  }
