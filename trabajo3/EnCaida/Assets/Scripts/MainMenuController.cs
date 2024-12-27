using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void LoadOptions()
    {
        SceneManager.LoadScene("Options");
    }

    public void LoadMenu()
    {
        Time.timeScale = 1.0f; 
        SceneManager.LoadScene("MainMenu");
    }

    // Método para salir del juego
    public void ExitGame()
    {
        // Solo funciona en la build del juego, no en el editor
        Debug.Log("Saliendo del juego..."); // Solo para probar en el editor
        Application.Quit();
    }
}
