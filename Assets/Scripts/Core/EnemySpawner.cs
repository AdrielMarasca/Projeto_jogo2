using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]
    [SerializeField] private GameObject goblinPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private float timeBetweenSpawns = 2.0f;
    [SerializeField] private int enemiesPerWave = 6;

    [Header("Debug")]
    [SerializeField] private bool autoStart = true;
    [SerializeField] private bool showDebugMessages = true;

    private int enemiesSpawnedThisWave = 0;
    private bool isSpawning = false;

    void Start()
    {
        if (autoStart)
            StartWave();
    }

    public void StartWave()
    {
        if (!isSpawning)
        {
            enemiesSpawnedThisWave = 0;
            isSpawning = true;
            StartCoroutine(SpawnWave());
        }
    }

    IEnumerator SpawnWave()
    {
        if (showDebugMessages)
            Debug.Log("[EnemySpawner] Iniciando Wave!");

        while (enemiesSpawnedThisWave < enemiesPerWave)
        {
            SpawnEnemy(goblinPrefab);
            enemiesSpawnedThisWave++;

            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        if (showDebugMessages)
            Debug.Log($"[EnemySpawner] Wave concluída! {enemiesSpawnedThisWave} inimigos enviados.");

        isSpawning = false;
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("EnemySpawner: Prefab do inimigo não atribuído!");
            return;
        }

        // Instancia o inimigo na posição do spawner
        GameObject newEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        
        // Opcional: colocar como filho para organização da hierarquia
        newEnemy.transform.SetParent(GameObject.Find("Enemies_Container")?.transform);
        
        // Nomeia para debug
        newEnemy.name = $"Goblin_{enemiesSpawnedThisWave + 1}";

        if (showDebugMessages)
            Debug.Log($"[EnemySpawner] Spawnado: {newEnemy.name}");
    }

    // Chamado pela UI ou GameManager para pausar
    public void StopSpawning()
    {
        StopAllCoroutines();
        isSpawning = false;
        if (showDebugMessages)
            Debug.Log("[EnemySpawner] Spawn pausado.");
    }
}