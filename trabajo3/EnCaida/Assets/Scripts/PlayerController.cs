using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f; // Velocidad de movimiento
    public float maxX = 2.5f; // Limite máximo en X

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Obtener el valor del acelerómetro, ESTO DEVUELVE LA INCLINACIÓN DEL MOVIL!!
        float tilt = Input.acceleration.x;

        // Mover el objeto en el eje X basado en la inclinación
        Vector3 newPosition = transform.position + new Vector3(tilt * speed * Time.deltaTime, 0, 0);

        // Restringir el movimiento dentro de los límites
        newPosition.x = Mathf.Clamp(newPosition.x, -maxX, maxX);

        // Aplicar la nueva posición
        transform.position = newPosition;
    }
}
