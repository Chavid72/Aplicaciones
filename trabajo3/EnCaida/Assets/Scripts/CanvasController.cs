using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public GameObject[] hearth;
    public int health;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void LoseHealth(int damage)
    {
        for(int i = damage; i > 0; i--) 
        {
            hearth[health-1].SetActive(false);
            health--;
        }
    }

    public void GainHealth(int healthPoints)
    {
        for(int i = healthPoints; i > 0; i--)
        {
            hearth[health].SetActive(true);
            health++;
        }
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
    public void StartHealth(int startHealth) // Establece las vidas con las que se empieza desde el PlayerController.
    {
        health = startHealth;
        //hearth = new GameObject[health];

        for (int i = 0; i < hearth.Length; i++)
        {
            hearth[i].SetActive(true);
        }
    }

    public void PlaySoundButton()
    {
        AudioManager.PlaySound(SoundType.MenuBotones, 1f);
    }
}
