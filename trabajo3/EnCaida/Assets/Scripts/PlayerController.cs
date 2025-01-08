using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f; // Velocidad de movimiento
    public float maxX = 2.5f; // Limite m�ximo en X

    public int health = 3;
    public float wind = 0;
    public bool shield = false;
    public int waitShield = 10;

    Coroutine shieldC;

    public CanvasController UIController;

    ///////////////////////////////////////////////
    /// <Cosas para la corutina del parpadeo> ///
    /// // Referencia al sprite Renderer del jugador
    private SpriteRenderer spriteRenderer;
    // Color original del sprite
    private Color originalColor;
    // Tiempo de duraci�n de cada parpadeo
    public float blinkDuration = 0.1f;
    // N�mero de parpadeos r�pidos
    public int blinkCount = 5;
    ///////////////////////////////////////////////
    private static PlayerController instance;

    public static PlayerController Instance
    {
        get
        {
            return instance;
        }
    }

    void Start()
    {
        instance = this;
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;

        //UIController = GetComponent<CanvasController>();
        UIController.StartHealth(health); // Le decimos a la UI con cuantas vidas empezamos.
    }

    // Update is called once per frame
    void Update()
    {
        // Obtener el valor del aceler�metro, ESTO DEVUELVE LA INCLINACI�N DEL MOVIL!!
        float tilt = Input.acceleration.x;

        // Mover el objeto en el eje X basado en la inclinaci�n
        Vector3 newPosition = transform.position + new Vector3((tilt * speed * Time.deltaTime) + wind, 0, 0);

        // Restringir el movimiento dentro de los l�mites
        newPosition.x = Mathf.Clamp(newPosition.x, -maxX, maxX);

        // Aplicar la nueva posici�n
        transform.position = newPosition;
    }

    public void TakeDamage(int damage)
    {
        if (shield) return;
        health -= damage;
        Debug.Log("Player Health: " + health);

        if(health <= 0)
        {
            PlayerPrefs.SetInt("Puntos", GameController.points); //Guarda en preferencias los puntos.
            SceneManager.LoadScene("LoseScene");
        }

        StartCoroutine(BlinkRed());
        UIController.LoseHealth(damage);
    }

    public void Health()
    {
        if (health < 3)
        {
            health += 1;
            UIController.GainHealth(1);
        }
    }

    public void StartShield()
    {
        shield = true;
        spriteRenderer.color = Color.blue;
        if (shieldC == null)
        {
            shieldC = StartCoroutine(Shield());
        }

        else
        {
            StopCoroutine(shieldC);
            shieldC = StartCoroutine(Shield());
        }
    }

    IEnumerator BlinkRed()
    {
        for (int i = 0; i < blinkCount; i++)
        {
            // Cambiar el color a rojo
            spriteRenderer.color = Color.red;

            // Esperar un tiempo corto
            yield return new WaitForSeconds(blinkDuration);

            // Volver al color original
            spriteRenderer.color = originalColor;

            // Esperar otro tiempo corto
            yield return new WaitForSeconds(blinkDuration);
        }
    }

    IEnumerator Shield()
    {
        yield return new WaitForSeconds(waitShield);
        spriteRenderer.color = originalColor;
        shield = false;
    }
}
