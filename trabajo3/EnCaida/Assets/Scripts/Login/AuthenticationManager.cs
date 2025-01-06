using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System.Threading.Tasks;
using UnityEngine.UI;

public class AuthenticationManager : MonoBehaviour
{
    public Text logTxt;
    
    async void Start()
    {
        await UnityServices.InitializeAsync();
    }

    public async void SignIn()
    {
        await SignInAnonymous();
    }

    async Task SignInAnonymous()
    {
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            print("Sign in success");
            print("Player ID: " + AuthenticationService.Instance.PlayerId);
            logTxt.text = "Player ID: " + AuthenticationService.Instance.PlayerId;
        }
        catch(AuthenticationException ex)
        {
            print(ex);
            Debug.LogException(ex);
        }
        
    }
}
