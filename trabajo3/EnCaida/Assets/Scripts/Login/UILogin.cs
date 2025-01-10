using System.Collections;
using System.Collections.Generic;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UILogin : MonoBehaviour
{
    [SerializeField] private Button loginButton;

    [SerializeField] private TMP_Text userIDText;

    [SerializeField] private Transform loginPanel, userPanel;

    [SerializeField] private LoginController loginController;

    private void OnEnable()
    {
        loginButton.onClick.AddListener(LoginButtonPressed);
        loginController.OnSignedIn += LoginController_OnSignedIn;
    }

    private void LoginController_OnSignedIn(PlayerInfo playerInfo, string playerName)
    {
        //loginPanel.gameObject.SetActive(false);
        //userPanel.gameObject.SetActive(true);

        userIDText.text = $"id_{playerInfo.Id} " + " \n Player name: " + playerName;
        Debug.Log(playerName);
        SceneManager.LoadScene("MainMenu");
    }

    private async void LoginButtonPressed()
    {
        await loginController.InitSignIn();
    }

    private void OnDisable()
    {
        loginButton.onClick.RemoveAllListeners();
        loginController.OnSignedIn -= LoginController_OnSignedIn;
    }
}
