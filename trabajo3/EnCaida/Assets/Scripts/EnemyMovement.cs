using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private float speed = 1f;
    public int damage = 1;

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Globo Spawneado");
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
        }
    }
}
