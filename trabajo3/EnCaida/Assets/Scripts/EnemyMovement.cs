using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private float speed = 1f;

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Mover al enemigo hacia arriba
        transform.position += Vector3.up * speed * Time.deltaTime;

        // Destruir el enemigo cuando sale de la pantalla
        if (transform.position.y > 6f)
        {
            Destroy(gameObject);
        }
    }
}