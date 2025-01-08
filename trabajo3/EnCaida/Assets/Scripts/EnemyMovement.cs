using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    private bool esquivado = false;
    public int damage = 1;

    public ParticleSystem particles;

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


        if (!EnemySpawner.Instance.maxLevel)
        {
            if (!esquivado && transform.position.y > 3f)
            {
                esquivado = true;
                EnemySpawner.Instance.enemiesDodge++;
                Debug.Log("dodge " + EnemySpawner.Instance.enemiesDodge);
                Debug.Log("restantes " + EnemySpawner.Instance.levels[EnemySpawner.Instance.currenLevel]);
                if (EnemySpawner.Instance.enemiesDodge >= EnemySpawner.Instance.levels[EnemySpawner.Instance.currenLevel])
                {

                    EnemySpawner.Instance.StartLevel();
                }
            }
        }
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

            if (!EnemySpawner.Instance.maxLevel)
            {
                    esquivado = true;
                    EnemySpawner.Instance.enemiesDodge++;
                    Debug.Log("dodge " + EnemySpawner.Instance.enemiesDodge);
                    Debug.Log("restantes " + EnemySpawner.Instance.levels[EnemySpawner.Instance.currenLevel]);
                    if (EnemySpawner.Instance.enemiesDodge >= EnemySpawner.Instance.levels[EnemySpawner.Instance.currenLevel])
                    {

                        EnemySpawner.Instance.StartLevel();
                    }
                
            }
            collision.gameObject.GetComponent<PlayerController>().TakeDamage(damage);
            Instantiate(particles, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
