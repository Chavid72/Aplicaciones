using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void PlayGame()
    {
        GameController.points = 0; //Reseteamos los puntos.
        PlaySoundButton();
        SceneManager.LoadScene("GameScene");
    }

    public void LoadOptions()
    {
        PlaySoundButton();
        SceneManager.LoadScene("Options");
    }

    public void LoadMenu()
    {
        Time.timeScale = 1.0f;
        PlaySoundButton();
        GameController.points = 0; //Reseteamos los puntos.
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadRanking()
    {
        PlaySoundButton();
        SceneManager.LoadScene("LoseScene");
    }

    public void LoadShop()
    {
        PlaySoundButton();
        SceneManager.LoadScene("Shop");
    }

    // M�todo para salir del juego
    public void ExitGame()
    {
        PlaySoundButton();
        // Solo funciona en la build del juego, no en el editor
        Debug.Log("Saliendo del juego..."); // Solo para probar en el editor
        Application.Quit();
    }

    public void PlaySoundButton()
    {
        AudioManager.PlaySound(SoundType.MenuBotones, 1f);
    }
}
