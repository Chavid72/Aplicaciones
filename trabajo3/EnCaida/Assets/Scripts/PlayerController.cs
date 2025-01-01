using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f; // Velocidad de movimiento
    public float maxX = 2.5f; // Limite m�ximo en X

    public int health = 3;

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

    void Start()
    {
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
        Vector3 newPosition = transform.position + new Vector3(tilt * speed * Time.deltaTime, 0, 0);

        // Restringir el movimiento dentro de los l�mites
        newPosition.x = Mathf.Clamp(newPosition.x, -maxX, maxX);

        // Aplicar la nueva posici�n
        transform.position = newPosition;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log("Player Health: " + health);
        StartCoroutine(BlinkRed());
        UIController.LoseHealth(damage);
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
}
