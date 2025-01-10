using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public void PlayGame()
    {
        GameController.points = 0; //Reseteamos los puntos.
        SceneManager.LoadScene("GameScene");
    }

    public void LoadOptions()
    {
        SceneManager.LoadScene("Options");
    }

    public void LoadMenu()
    {
        Time.timeScale = 1.0f;
        GameController.points = 0; //Reseteamos los puntos.
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadRanking()
    {
        SceneManager.LoadScene("LoseScene");
    }

    // Método para salir del juego
    public void ExitGame()
    {
        // Solo funciona en la build del juego, no en el editor
        Debug.Log("Saliendo del juego..."); // Solo para probar en el editor
        Application.Quit();
    }
}
