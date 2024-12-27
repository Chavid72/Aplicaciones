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

    // Método para salir del juego
    public void ExitGame()
    {
        // Solo funciona en la build del juego, no en el editor
        Debug.Log("Saliendo del juego..."); // Solo para probar en el editor
        Application.Quit();
    }
}
