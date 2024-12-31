using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Prefab del enemigo
    public float initialSpawnInterval = 2f; // Intervalo inicial entre spawns
    public float minSpawnInterval = 0.5f; // Mínimo intervalo entre spawns
    public float spawnIntervalDecreaseRate = 0.05f; // Reducción del intervalo con el tiempo
    public float minX = -2.7f; // Límite izquierdo para spawnear enemigos
    public float maxX = 2.7f; // Límite derecho
    public float startSpeed = 1f; // Velocidad inicial de los enemigos
    public float speedIncreaseRate = 0.1f; // Incremento de velocidad de los enemigos

    private float currentSpeed; // Velocidad actual de los enemigos
    private float currentSpawnInterval; // Intervalo actual de spawn

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = startSpeed;
        currentSpawnInterval = initialSpawnInterval;
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
            yield return new WaitForSeconds(currentSpawnInterval);
            AdjustDifficultyOverTime();
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

    void AdjustDifficultyOverTime()
    {
        // Incrementar la velocidad de los enemigos
        currentSpeed += speedIncreaseRate;

        // Reducir el intervalo de spawn, pero no menos que el mínimo permitido
        currentSpawnInterval = Mathf.Max(minSpawnInterval, currentSpawnInterval - spawnIntervalDecreaseRate);
    }
}
