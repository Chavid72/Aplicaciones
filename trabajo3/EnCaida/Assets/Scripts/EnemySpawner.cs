using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab del enemigo (el cuadrado)
    public float spawnInterval = 2f; // Tiempo entre spawns
    public float minX = -2f; // Límite izquierdo
    public float maxX = 2f; // Límite derecho
    public float startSpeed = 1f; // Velocidad inicial de los enemigos
    public float speedIncreaseRate = 0.1f; // Incremento de velocidad con el tiempo

    private float currentSpeed; // Velocidad actual de los enemigos

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = startSpeed;
        StartCoroutine(SpawnEnemies());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(spawnInterval);
            IncreaseSpeedOverTime();
        }
    }
    void SpawnEnemy()
    {
        // Generar una posición aleatoria en el eje X dentro de los límites
        float spawnX = Random.Range(minX, maxX);
        Vector3 spawnPosition = new Vector3(spawnX, -6f, 0f); // Debajo de la pantalla
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        // Agregar movimiento al enemigo
        EnemyMovement enemyMovement = newEnemy.GetComponent<EnemyMovement>();
        if (enemyMovement != null)
        {
            enemyMovement.SetSpeed(currentSpeed);
        }
    }

    void IncreaseSpeedOverTime()
    {
        currentSpeed += speedIncreaseRate; // Incrementa la velocidad poco a poco
    }
}
