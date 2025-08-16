using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using Google;
using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseManager : MonoBehaviour
{
    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;

    [SerializeField]
    private TMP_Text test;

    public TMP_Text Username, UserEmail;
    public Button button;


    private void Start()
    {
        InitFirebase();
        button.onClick.AddListener(GoogleSignInClick);
    }

    void InitFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                test.text += "Firebase Auth initialized successfully." + task.Exception;
            }
            else
            {
                test.text += "Could not resolve Firebase dependencies: " + task.Result;
            }
        });
    }

    public void GoogleSignInClick()
    {
        try
        {
            GoogleSignIn.Configuration = new GoogleSignInConfiguration
            {
                WebClientId = "925887350584-abaas9pllsv00qupkok72lm69rfmtosi.apps.googleusercontent.com",
                RequestIdToken = true,
                UseGameSignIn = false,
                RequestEmail = true
            };

            GoogleSignIn.DefaultInstance.SignIn().ContinueWith(task =>
            {
                if (task.IsFaulted)
                {
                    test.text += "SignIn Error: " + task.Exception;
                }
                else if (task.IsCanceled)
                {
                    test.text += "SignIn Canceled: ";
                }
                else
                {
                    OnGoogleAuthenticatedFinished(task);
                }
            });
        }
        catch (Exception ex)
        {
            test.text += "GoogleSignInClick Exception: " + ex.Message;
        }
    }

    void OnGoogleAuthenticatedFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            Debug.LogError("Faulted");
            test.text = "Faulted";
        }
        else if (task.IsCanceled)
        {
            Debug.LogError("Cancelled");
            test.text = "Cancelled";
        }
        else
        {
            Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(task.Result.IdToken, null);

            auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    test.text = "IsCanceled";
                    return;
                }

                if (task.IsFaulted)
                {
                    test.text = "SignInWithCredentialAsync encountered an error: " + task.Exception;
                    return;
                }

                user = auth.CurrentUser;

                Username.text = user.DisplayName;
                UserEmail.text = user.Email;

                // StartCoroutine(LoadImage(CheckImageUrl(user.PhotoUrl.ToString())));
            });
        }
    }
}
