using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public GameObject[] hearth;
    public int health = 3;

    // Start is called before the first frame update
    void Start()
    {
        hearth = new GameObject[health];
    }

    public void LoseHealth(int healthPoints)
    {
        hearth[health].SetActive(false);
        //health--;
    }

    public void GainHealth(int healthPoints)
    {
        hearth[health-1].SetActive(true);
        //health++;
    }

    public void StopAndResumeGame()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0; // Pausar el juego
            Debug.Log("Juego pausado");
        }
        else
        {
            Time.timeScale = 1; // Reanudar el juego
            Debug.Log("Juego reanudado");
        }
    }
}
