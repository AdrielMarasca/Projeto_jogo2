using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public string waveName = "Wave";
    public GameObject enemyPrefab;
    public int enemyCount = 5;
    public float spawnInterval = 1.5f;
    public float timeBeforeNextWave = 5f; // Pausa após esta wave
}

public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance;

    [Header("Waves")]
    [SerializeField] private List<Wave> waves;
    [SerializeField] private Transform spawnPoint;

    [Header("Debug")]
    [SerializeField] private bool autoStart = true;
    [SerializeField] private bool showDebug = true;

    private int currentWaveIndex = 0;
    private int enemiesRemainingInWave = 0;
    private bool isWaveActive = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (autoStart)
            StartCoroutine(StartWaves());
    }

    IEnumerator StartWaves()
    {
        yield return new WaitForSeconds(2f); // Pequena pausa inicial

        while (currentWaveIndex < waves.Count)
        {
            Wave wave = waves[currentWaveIndex];
            enemiesRemainingInWave = wave.enemyCount;
            isWaveActive = true;

            if (showDebug)
                Debug.Log($"🌊 {wave.waveName} iniciada! {wave.enemyCount} inimigos.");

            for (int i = 0; i < wave.enemyCount; i++)
            {
                SpawnEnemy(wave.enemyPrefab);
                yield return new WaitForSeconds(wave.spawnInterval);
            }

            // Aguarda todos os inimigos da wave serem derrotados
            while (enemiesRemainingInWave > 0)
            {
                yield return null;
            }

            isWaveActive = false;
            if (showDebug)
                Debug.Log($"✅ {wave.waveName} concluída!");

            currentWaveIndex++;

            // Se ainda há waves, intervalo antes da próxima
            if (currentWaveIndex < waves.Count)
            {
                float wait = waves[currentWaveIndex - 1].timeBeforeNextWave;
                if (showDebug)
                    Debug.Log($"⏳ Próxima wave em {wait} segundos...");
                yield return new WaitForSeconds(wait);
            }
        }

        if (showDebug)
            Debug.Log("🏆 Todas as waves derrotadas! Vitória!");

        if (GameManager.Instance != null)
            GameManager.Instance.Victory();
    }

    void SpawnEnemy(GameObject prefab)
    {
        if (prefab == null || spawnPoint == null) return;

        GameObject enemy = Instantiate(prefab, spawnPoint.position, Quaternion.identity);
        enemy.name = $"{prefab.name}_{Time.time}";

        GameObject container = GameObject.Find("Enemies_Container");
        if (container != null)
            enemy.transform.SetParent(container.transform);

        // Inscreve-se no evento de morte do inimigo
        EnemyHealth health = enemy.GetComponent<EnemyHealth>();
        if (health != null)
        {
            health.OnDeath += OnEnemyDied;
        }
    }

    void OnEnemyDied()
    {
        enemiesRemainingInWave--;
        if (showDebug)
            Debug.Log($"💀 Inimigo abatido. Restam {enemiesRemainingInWave} na wave.");
    }
}